using Mzstruct.Base.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Configs
{
    public class JWTAuth
    {
        public string? SecretKey { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int? TokenExpiry { get; set; } // in minutes: 15
        public int? RefreshTokenExpiry { get; set; } // in days: 90
    }
}
