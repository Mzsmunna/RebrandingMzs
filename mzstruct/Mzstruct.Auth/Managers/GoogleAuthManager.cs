using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Mzstruct.Auth.Managers
{
    public class GoogleAuthManager(ILogger<GoogleAuthManager> logger) : IGoogleAuthManager
    {
        public async Task<BaseUserModel?> ValidateClaim(AuthenticateResult authResult)
        {
            if (authResult is null || !authResult.Succeeded || authResult.Principal is null)
            {
                logger.LogWarning("SignIn.Google: Bad Request");
                return null;
            }
            var externalUser = authResult.Principal;
            var providerKey = externalUser.FindFirstValue(ClaimTypes.NameIdentifier); // Google unique ID
            if (providerKey == null) return null;
            return new BaseUserModel
            {
                // Standard claims from Google
                Name = externalUser.FindFirstValue(ClaimTypes.GivenName) ?? "",
                FirstName = externalUser.FindFirstValue(ClaimTypes.GivenName) ?? "",
                LastName = externalUser.FindFirstValue(ClaimTypes.Surname) ?? "",
                Email = externalUser.FindFirstValue(ClaimTypes.Email) ?? "",
                Img = externalUser.FindFirst("picture")?.Value ?? ""
            };
        }

        public async Task<Payload?> ValidateToken(string credential)
        {
            var settings = new ValidationSettings()
            {
                Audience = new List<string> { "729270420162-eqgm0blm2u34lgu9m9ck0b6cq6q47oi3.apps.googleusercontent.com" }
            };
            var payload = await ValidateAsync(credential, settings);
            return payload;
        }
    }
}
