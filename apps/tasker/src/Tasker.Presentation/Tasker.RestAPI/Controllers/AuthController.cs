using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Tasker.Application.Dtos;
using Tasker.Application.Errors;
using Tasker.Application.Extensions;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.RestAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;

        public AuthController(IConfiguration configuration, ILogger<UsersController> logger, IUserRepository userRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _userRepository = userRepository;
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
                        CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                        if (!VerifyPasswordHash(user.Password, user.PasswordHash, user.PasswordSalt))
                        {
                            //return StatusCode(StatusCodes.Status403Forbidden, "Wrong credential.");
                            return Error.Validation("Login.Credential.Wrong", "Wrong credential.").ToProblem(this);
                        }
                        else
                        {
                            string token = CreateToken(existingUser);
                            var refreshToken = GenerateRefreshToken();
                            SetRefreshToken(refreshToken, existingUser);
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
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { "729270420162-eqgm0blm2u34lgu9m9ck0b6cq6q47oi3.apps.googleusercontent.com" }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

            if (payload != null)
            {
                var result = _userRepository.LoginUser(payload.Email).Result;
                return result.Map<IActionResult>(
                    Ok: existingUser =>
                    {
                        CreatePasswordHash(existingUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        existingUser.PasswordHash = passwordHash;
                        existingUser.PasswordSalt = passwordSalt;

                        if (!VerifyPasswordHash(existingUser.Password, existingUser.PasswordHash, existingUser.PasswordSalt))
                        {
                            return Error.Validation("Login.Google.Error", "Wrong credential.").ToProblem(this); //StatusCode(StatusCodes.Status403Forbidden, "Wrong credential.");
                        }
                        else
                        {
                            string token = CreateToken(existingUser);
                            var refreshToken = GenerateRefreshToken();
                            SetRefreshToken(refreshToken, existingUser);
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

                    string token = CreateToken(user);
                    var newRefreshToken = GenerateRefreshToken();
                    SetRefreshToken(newRefreshToken, user);

                    return Ok(token);
                },
                Err: error => NotFound()
            );    
        }

        /// ---
        
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(User user)
        {
            var tokenExpiredOn = DateTime.UtcNow.AddMinutes(15);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email.ToLower()),
                new Claim(ClaimTypes.Role, user.Role.ToLower()),
                new Claim(ClaimTypes.Expiration, tokenExpiredOn.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetValue<string>("JWTAuthSecretKey")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpiredOn,
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
