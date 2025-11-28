using Google.Apis.Auth;
using Kernel.Drivers.Dtos;
using Kernel.Drivers.Errors;
using Kernel.Managers.Auth;
using Kernel.Managers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Tasker.Application.Errors;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.RestAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenManager _jwtTokenManager;
        private readonly GoogleAuthManager _googleAuthManager;

        public AuthController(IConfiguration config, ILogger<UsersController> logger, IUserRepository userRepository, JwtTokenManager jwtTokenManager)
        {
            _config = config;
            _logger = logger;
            _userRepository = userRepository;
            _jwtTokenManager = jwtTokenManager;
            _googleAuthManager = new GoogleAuthManager();
        }

        [HttpPost]
        [ActionName("Register")]
        public IActionResult Register(User user)
        {
            user.IsActive = true;
            var result = _userRepository.RegisterUser(user).Result;
            return result.Map<IActionResult>(
                Ok: data => Ok(data),
                Err: error => error.ToProblem(this)
            );
        }

        [HttpPost]
        [ActionName("Login")]
        public IActionResult Login(User user)
        {
            if (user != null)
            {
                var result = _userRepository.LoginUser(user.Email, user.Password).Result;
                return result.Map<IActionResult>(
                    Ok: existingUser =>
                    {
                        _jwtTokenManager.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                        if (!_jwtTokenManager.VerifyPasswordHash(user.Password, user.PasswordHash, user.PasswordSalt))
                        {
                            //return StatusCode(StatusCodes.Status403Forbidden, "Wrong credential.");
                            return Error.Validation("Login.Credential.Wrong", "Wrong credential.").ToProblem(this);
                        }
                        else
                        {
                            string token = _jwtTokenManager.CreateToken(existingUser);
                            var refreshToken = _jwtTokenManager.GenerateRefreshToken();
                            _jwtTokenManager.SetRefreshToken(refreshToken, existingUser);
                            return Ok(token);
                        }
                    },
                    Err: error => Error.NotFound("Login.Credential.NotFound", "User doesn't exist.").ToProblem(this) //StatusCode(StatusCodes.Status204NoContent, "User doesn't exist.")
                );
            }
            else
            {
                return ClientError.BadRequest.ToProblem(this);
            }
        }

        [HttpPost]
        [ActionName("LoginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
        {
            var payload = await _googleAuthManager.ValidateToken(credential);
            if (payload != null)
            {
                var result = _userRepository.LoginUser(payload.Email).Result;
                return result.Map<IActionResult>(
                    Ok: existingUser =>
                    {
                        _jwtTokenManager.CreatePasswordHash(existingUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        existingUser.PasswordHash = passwordHash;
                        existingUser.PasswordSalt = passwordSalt;

                        if (!_jwtTokenManager.VerifyPasswordHash(existingUser.Password, existingUser.PasswordHash, existingUser.PasswordSalt))
                        {
                            return Error.Validation("Login.Google.Error", "Wrong credential.").ToProblem(this); //StatusCode(StatusCodes.Status403Forbidden, "Wrong credential.");
                        }
                        else
                        {
                            string token = _jwtTokenManager.CreateToken(existingUser);
                            var refreshToken = _jwtTokenManager.GenerateRefreshToken();
                            _jwtTokenManager.SetRefreshToken(refreshToken, existingUser);
                            return Ok(token);
                        }
                    },
                    Err: error => Error.NotFound("Login.Google.NotLinkned", "User doesn't exist.").ToProblem(this) //StatusCode(StatusCodes.Status204NoContent, "User doesn't exist.")
                );
            }
            else
            {
                return ClientError.BadRequest.ToProblem(this);
            }
        }

        [HttpPost]
        [ActionName("RefreshToken")]
        public IActionResult RefreshToken(string userId)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = _userRepository.GetUser(userId).Result;
            return result.Map<IActionResult>(
                Ok: user =>
                {
                    if (!user.RefreshToken.Equals(refreshToken))
                    {
                        return Error.Unauthorized("Token.Refresh.Invalid", "Invalid Refresh Token.").ToProblem(this); //Unauthorized("Invalid Refresh Token.");
                    }
                    else if (user.TokenExpires < DateTime.UtcNow)
                    {
                        return Error.Unauthorized("Token.Refresh.Expired", "Token expired.").ToProblem(this); //Unauthorized("Token expired.");
                    }

                    string token = _jwtTokenManager.CreateToken(user);
                    var newRefreshToken = _jwtTokenManager.GenerateRefreshToken();
                    _jwtTokenManager.SetRefreshToken(newRefreshToken, user);

                    return Ok(token);
                },
                Err: error => NotFound()
            );    
        }
    }
}
