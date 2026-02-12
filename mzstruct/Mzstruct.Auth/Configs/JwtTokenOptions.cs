using Mzstruct.Base.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Configs
{
    public sealed class JwtTokenOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public string SecretConfigKey { get; set; } = "JWTAuth.SecretKey";
        public string ExternalSchema { get; set; } = "External";

        public int TokenExpiryValue { get; set; } = 15;
        public TimeUnit TokenExpiryUnit { get; set; } = TimeUnit.Minutes;

        public int RefreshTokenExpiryValue { get; set; } = 90;
        public TimeUnit RefreshTokenExpiryUnit { get; set; } = TimeUnit.Days;

        public JWTAuth? jwtAuthConfig { get; set; }
        public TokenValidationParameters? TokenParameters { get; set; }

        public bool EnableOAuth { get; set; } = true;
    }
}
