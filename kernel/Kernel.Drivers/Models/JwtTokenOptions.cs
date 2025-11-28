using Kernel.Drivers.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Drivers.Models
{
    public sealed class JwtTokenOptions
    {
        public string? SecretKey { get; set; }
        public string? SecretConfigKey { get; set; }

        public int TokenExpiryValue { get; set; } = 60;
        public TimeUnit TokenExpiryUnit { get; set; } = TimeUnit.Minutes;

        public int RefreshTokenExpiryValue { get; set; } = 7;
        public TimeUnit RefreshTokenExpiryUnit { get; set; } = TimeUnit.Days;

        public TokenValidationParameters? CustomTokenValidationParameters { get; set; }
    }
}
