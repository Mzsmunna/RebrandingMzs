using Mzstruct.Base.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using Mzstruct.Auth.Models.Configs;

namespace Mzstruct.Common.Dependencies
{
    public static class JwtOptionsResolver
    {
        public static JwtTokenOptions WithSecretKey(this JwtTokenOptions opts, string key)
        {
            opts.SecretKey = key;
            return opts;
        }

        public static JwtTokenOptions WithSecretConfig(this JwtTokenOptions opts, string configKey)
        {
            opts.SecretConfigKey = configKey;
            return opts;
        }

        public static JwtTokenOptions WithTokenExpiry(this JwtTokenOptions opts, int value, TimeUnit unit)
        {
            opts.TokenExpiryValue = value;
            opts.TokenExpiryUnit = unit;
            return opts;
        }

        public static JwtTokenOptions WithRefreshTokenExpiry(this JwtTokenOptions opts, int value, TimeUnit unit)
        {
            opts.RefreshTokenExpiryValue = value;
            opts.RefreshTokenExpiryUnit = unit;
            return opts;
        }

        public static JwtTokenOptions WithValidation(this JwtTokenOptions opts, TokenValidationParameters parameters)
        {
            opts.TokenParameters = parameters;
            return opts;
        }

        public static JwtTokenOptions WithSignIn(this JwtTokenOptions opts, SignInWith SignInOptions)
        {
            opts.SignInOptions = SignInOptions;
            return opts;
        }
    }
}
