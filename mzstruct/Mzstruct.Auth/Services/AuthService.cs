using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Auth.Models.Dtos;
using Mzstruct.Auth.Validators;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Extensions;
using Mzstruct.Base.Mappings;
using Mzstruct.Base.Models;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mzstruct.Auth.Services
{
    public class AuthService(ILogger<AuthService> logger, 
        IHttpContextAccessor httpContextAccessor,
        //IValidator<SignUpDto> signUpValidator,
        IBaseUserRepository userRepository,
        IJwtTokenManager jwtTokenManager,
        IGoogleAuthManager googleAuthManager) : IAuthService
    {
        public async Task<Result<BaseUserModel?>> SignUp(SignUpDto signUpDto)
        {
            //var validation = signUpValidator.Validate(signUpDto);
            var validation = await AuthValidator.ValidateSignUp(signUpDto);
            if (validation.IsValid is false)
                return Error.Validation("AuthCommand.SignUp.InvalidInput",
                    "SignUp form is invalid", validation.ToErrorDictionary());
            
            var user = signUpDto.ToEntity<BaseUser, SignUpDto>();
            if (user is null)
            {
                logger.LogWarning("SignUp: Bad Request");
                return Error.Bad("AuthCommand.SignUp.BadRequest", "Requested body payload seems invalid");
            }
            
            var registered = await userRepository.RegisterUser(user);
            if (registered is null)
                return Error.Conflict("User.Exists", "This email already exists.");

            return registered.ToModel<BaseUserModel, BaseUser>();
        }

        public async Task<Result<string>> SignIn(SignInDto signInDto)
        {
            if (signInDto is null)
            {
                logger.LogWarning("SignIn: Bad Request");
                return ClientError.BadRequest;
            }

            var validation = await AuthValidator.ValidateSignIn(signInDto);
            if (validation.IsValid is false)
                return Error.Validation("AuthCommand.SignIn.InvalidForm",
                    "SignIn form is invalid", validation.ToErrorDictionary());
            
            var signInUser = await userRepository.LoginUser(signInDto.Email, signInDto.Password);
            if (signInUser is null)
                return Error.NotFound("SignIn.Credential.NotFound", "User credential didn't match"); //StatusCode(StatusCodes.Status204NoContent, "User doesn't exist.");
            
            var passHashWithSalt = jwtTokenManager.CreatePasswordHash(signInDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            //signInUser.Password = passHashWithSalt;
            signInUser.PasswordHash = passwordHash;
            signInUser.PasswordSalt = passwordSalt;
            signInUser.Name = signInUser.FirstName + " " + signInUser.LastName;
            
            if (!jwtTokenManager.VerifyPasswordHash(signInUser.Password, passwordHash, passwordSalt))
                return Error.Validation("SignIn.Credential.Wrong", "Wrong credential.");
            
            string token = jwtTokenManager.CreateNewToken(signInUser);
            return token;
        }

        public async Task<Result<string>> SignInWithGoogle(string credential)
        {
            if (string.IsNullOrEmpty(credential))
                return await SignInWithGoogle();
            var payload = await googleAuthManager.ValidateToken(credential);
            if (payload is null)
            {
                logger.LogWarning("SignIn.Google: Bad Request");
                return ClientError.BadRequest;
            }
            return await SignInWith(payload.Email, "Google");
        }

        public async Task<Result<string>> SignInWithGoogle()
        {
            // Read the external identity from the "External" cookie
            if (httpContextAccessor.HttpContext is null)
            {
                logger.LogWarning("SignIn.Google: Bad Request");
                return ClientError.BadRequest;
            }
            var authResult = await httpContextAccessor.HttpContext.AuthenticateAsync("External");
            var user = await googleAuthManager.ValidateClaim(authResult);
            if (user is null)
                return Error.Bad("SignIn.Google.BadRequest", "Google did not provide user info");
            if (string.IsNullOrEmpty(user.Email)) return "";
            // Cleanup the external cookie
            await httpContextAccessor.HttpContext.SignOutAsync("External");
            return await SignInWith(user.Email, "Google");
        }

        public async Task<Result<string>> SignInWithGitHub()
        {
            if (httpContextAccessor.HttpContext is null)
            {   logger.LogWarning("SignIn.GitHub: Bad Request");
                return ClientError.BadRequest;
            }
            var authResult = await httpContextAccessor.HttpContext.AuthenticateAsync("External");
            if (!authResult.Succeeded) return "";
            var claims = authResult.Principal!.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var githubId = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(email)) return "";
            // Cleanup the external cookie
            await httpContextAccessor.HttpContext.SignOutAsync("External");
            return await SignInWith(email, "GitHub"); // 🔐 Create / find user in DB + 🔑 Issue JWT
        }

        public async Task<Result<string>> SignInWith(string email, string option = "Mail")
        {
            var signInUser = await userRepository.LoginUser(email);
            if (signInUser is null)
                return Error.NotFound($"SignIn.{option}.NotLinkned", "User doesn't exist."); //StatusCode(StatusCodes.Status204NoContent, "User doesn't exist.");

            var passHashWithSalt = jwtTokenManager.CreatePasswordHash(signInUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
            //signInUser.Password = passHashWithSalt;
            signInUser.PasswordHash = passwordHash;
            signInUser.PasswordSalt = passwordSalt;
            if (!jwtTokenManager.VerifyPasswordHash(signInUser.Password, passwordHash, passwordSalt))
                return Error.Validation($"SignIn.{option}.Error", "Wrong credential."); //StatusCode(StatusCodes.Status403Forbidden, "Wrong credential.");
            if (string.IsNullOrEmpty(signInUser.Name) && 
                !string.IsNullOrEmpty(signInUser.FirstName))
                signInUser.Name = string.IsNullOrEmpty(signInUser.LastName)
                ? signInUser.FirstName
                : $"{signInUser.FirstName} {signInUser.LastName}";
            string token = jwtTokenManager.CreateNewToken(signInUser);
            return token;
        }

        public async Task<Result<string>> RefreshToken(string userId, string token, string refreshToken)
        {
            if (string.IsNullOrEmpty(userId) 
                || string.IsNullOrEmpty(token)
                || string.IsNullOrEmpty(refreshToken))
            {
                logger.LogWarning("RefreshToken: Bad Request");
                return ClientError.BadRequest;
            }

            var (principal, validatedToken) = jwtTokenManager.GetPrincipalFromToken();
            if (principal is null || validatedToken is null)
                return Error.Unauthorized("Token.Principal.Invalid", "Invalid Principal Token.");

            var jti = principal.Claims.FirstOrDefault(c =>
                c.Type == JwtRegisteredClaimNames.Jti ||
                c.Type == "jti");

            if (jwtTokenManager.ValidateToken())
                return Error.Bad("Token.Refresh.Invalid", "Token is still valid."); //BadRequest("Token is still valid.");

            var signInUser = await userRepository.GetById(userId);           
            if (signInUser is null)
                return Error.NotFound("Token.Refresh.NotFound", "User token unavailable");

            if (signInUser.RefToken is null || string.IsNullOrEmpty(signInUser.RefreshToken))
                return Error.Unauthorized("Token.Refresh.Missing", "Refresh Token missing. Please SignIn"); //Unauthorized("Refresh Token missing.");
           
            if (jti is null || !jti.Value.Equals(signInUser.RefToken.JtiId))
                return Error.Unauthorized("Token.Refresh.InvalidJti", "Invalid JTI in Refresh Token."); //Unauthorized("Invalid JTI in Refresh Token.");

            if (signInUser.RefToken.IsRevoked || signInUser.RefToken.RevokedAt.HasValue)
                return Error.Unauthorized("Token.Refresh.Revoked", "Refresh Token revoked. Please SignIn"); //Unauthorized("Refresh Token revoked.");

            if (!signInUser.RefreshToken.Equals(refreshToken))
                return Error.Unauthorized("Token.Refresh.Invalid", "Invalid Refresh Token."); //Unauthorized("Invalid Refresh Token.");
            else if (signInUser.RefToken.ExpiresAt < DateTime.UtcNow ||
                signInUser.TokenExpires < DateTime.UtcNow)
                return Error.Unauthorized("Token.Refresh.Expired", "Token expired."); //Unauthorized("Token expired.");

            string jwt = jwtTokenManager.CreateNewToken(signInUser);
            return jwt;
        }
    }
}
