using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Kernel.Managers.Auth
{
    public class GoogleAuthManager
    {
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
