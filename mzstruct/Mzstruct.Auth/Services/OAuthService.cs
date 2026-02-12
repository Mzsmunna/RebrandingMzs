using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Patterns.Errors;
using Mzstruct.Base.Patterns.Result;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mzstruct.Auth.Services
{
    public class OAuthService<TIdentity>(ILogger<OAuthService<TIdentity>> logger, 
        IHttpContextAccessor httpContextAccessor,
        IBasicAuthService basicAuthService,
        IGoogleAuthManager googleAuthManager) : IOAuthService where TIdentity : BaseUser
    {
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
            return await basicAuthService.SignInWith(payload.Email, "Google");
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
            return await basicAuthService.SignInWith(user.Email, "Google");
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
            return await basicAuthService.SignInWith(email, "GitHub"); // 🔐 Create / find user in DB + 🔑 Issue JWT
        }
    }
}
