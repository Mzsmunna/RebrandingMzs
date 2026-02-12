using Microsoft.Extensions.Logging;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Auth.Features.Commands;
using Mzstruct.Auth.Validators;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Extensions;
using Mzstruct.Base.Mappings;
using Mzstruct.Base.Patterns.Errors;
using Mzstruct.Base.Patterns.Result;
using Mzstruct.DB.Contracts.IRepos;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mzstruct.Auth.Services
{
    public class BasicAuthService<TIdentity>(ILogger<BasicAuthService<TIdentity>> logger, 
        //IValidator<SignUpDto> signUpValidator,
        IAuthUserRepo<TIdentity> userRepository, //IBaseUserRepository<TIdentity> userRepository,
        IJwtTokenManager jwtTokenManager) : IBasicAuthService where TIdentity : BaseUser
    {
        public async Task<Result<string>> SignUp(SignUpCommand request)
        {
            //var validation = signUpValidator.Validate(request);
            var validation = await AuthValidator.ValidateSignUp(request);
            if (validation.IsValid is false)
                return Error.Validation("AuthCommand.SignUp.InvalidInput",
                    "SignUp form is invalid", validation.ToErrorDictionary());
            
            var user = request.ToEntity<TIdentity, SignUpCommand>();
            if (user is null)
            {
                logger.LogWarning("SignUp: Bad Request");
                return Error.Bad("AuthCommand.SignUp.BadRequest", "Requested body payload seems invalid");
            }
            
            var registered = await userRepository.RegisterUser(user);
            if (registered is null)
                return Error.Conflict("User.Exists", "This email already exists.");

            return registered.Id;
        }

        public async Task<Result<string>> SignIn(SignInCommand request)
        {
            if (request is null)
            {
                logger.LogWarning("SignIn: Bad Request");
                return ClientError.BadRequest;
            }

            var validation = await AuthValidator.ValidateSignIn(request);
            if (validation.IsValid is false)
                return Error.Validation("AuthCommand.SignIn.InvalidForm",
                    "SignIn form is invalid", validation.ToErrorDictionary());
            
            var signInUser = await userRepository.LoginUser(request.Email, request.Password);
            if (signInUser is null)
                return Error.NotFound("SignIn.Credential.NotFound", "User credential didn't match"); //StatusCode(StatusCodes.Status204NoContent, "User doesn't exist.");
            
            var passHashWithSalt = jwtTokenManager.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            //signInUser.Password = passHashWithSalt;
            signInUser.PasswordHash = passwordHash;
            signInUser.PasswordSalt = passwordSalt;
            signInUser.Name = signInUser.FirstName + " " + signInUser.LastName;
            
            if (!jwtTokenManager.VerifyPasswordHash(signInUser.Password, passwordHash, passwordSalt))
                return Error.Validation("SignIn.Credential.Wrong", "Wrong credential.");
            
            string token = jwtTokenManager.CreateNewToken(signInUser);
            return token;
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

        public async Task<Result<bool>> SignOut(string token = "")
        {
            if (string.IsNullOrEmpty(token)) token = jwtTokenManager.GetHeaderToken();
            if (!jwtTokenManager.ValidateToken()) return true;
            var (principal, validatedToken) = jwtTokenManager.GetPrincipalFromToken(token);
            if (principal is null || validatedToken is null) return true;
            var userIdClaim = principal.Claims.FirstOrDefault(c =>
                c.Type == JwtRegisteredClaimNames.Sub ||
                c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null ||
                string.IsNullOrEmpty(userIdClaim.Value)) return true;
            var user = await userRepository.GetById(userIdClaim.Value);           
            if (user is null) return true;
            user.RefreshToken = string.Empty;
            user.RefToken = null;
            await userRepository.SaveAsync(user);           
            return true;
        }

        public async Task<Result<string>> RefreshToken(string token = "", string refreshToken = "")
        {
            if (string.IsNullOrEmpty(token)) token = jwtTokenManager.GetHeaderToken();
            if (string.IsNullOrEmpty(refreshToken)) refreshToken = jwtTokenManager.GetHeaderRefreshToken();
            if (string.IsNullOrEmpty(token)
                || string.IsNullOrEmpty(refreshToken))
            {
                logger.LogWarning("RefreshToken: Bad Request");
                return ClientError.BadRequest;
            }

            var (principal, validatedToken) = jwtTokenManager.GetPrincipalFromToken();
            if (principal is null || validatedToken is null)
                return Error.Unauthorized("Token.Principal.Invalid", "Invalid Principal Token.");

            if (jwtTokenManager.ValidateToken())
                return Error.Bad("Token.Refresh.Invalid", "Token is still valid."); //BadRequest("Token is still valid.");

            var userIdClaim = principal.Claims.FirstOrDefault(c =>
                c.Type == JwtRegisteredClaimNames.Sub ||
                c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null ||
                string.IsNullOrEmpty(userIdClaim.Value)) return Error.NotFound("Token.Refresh.ClaimsNotFound", "Wrong User token or claims");
            
            var user = await userRepository.GetById(userIdClaim.Value);
            if (user is null)
                return Error.NotFound("Token.Refresh.UserNotFound", "User unavailable");
            if (user.RefToken is null || string.IsNullOrEmpty(user.RefreshToken))
                return Error.Unauthorized("Token.Refresh.Missing", "Refresh Token missing. Please SignIn"); //Unauthorized("Refresh Token missing.");
           
            var jti = principal.Claims.FirstOrDefault(c =>
                c.Type == JwtRegisteredClaimNames.Jti ||
                c.Type == "jti");

            if (jti is null || !jti.Value.Equals(user.RefToken.JtiId))
                return Error.Unauthorized("Token.Refresh.InvalidJti", "Invalid JTI in Refresh Token."); //Unauthorized("Invalid JTI in Refresh Token.");

            if (user.RefToken.IsRevoked || user.RefToken.RevokedAt.HasValue)
                return Error.Unauthorized("Token.Refresh.Revoked", "Refresh Token revoked. Please SignIn"); //Unauthorized("Refresh Token revoked.");

            if (!user.RefreshToken.Equals(refreshToken))
                return Error.Unauthorized("Token.Refresh.Invalid", "Invalid Refresh Token."); //Unauthorized("Invalid Refresh Token.");
            else if (user.RefToken.ExpiresAt < DateTime.UtcNow ||
                user.TokenExpires < DateTime.UtcNow)
                return Error.Unauthorized("Token.Refresh.Expired", "Token expired."); //Unauthorized("Token expired.");

            string jwt = jwtTokenManager.CreateNewToken(user);
            await userRepository.SaveAsync(user);
            return jwt;
        }
    }
}
