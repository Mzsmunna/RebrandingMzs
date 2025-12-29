using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Managers;
using Mzstruct.Auth.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Common.Dependencies
{
    public static class AuthResolver
    {
        public static IServiceCollection AddGoogleSignIn(this IServiceCollection services)
        {
            services.AddScoped<IGoogleAuthManager, GoogleAuthManager>();
            return services;
        }

        public static IServiceCollection AddJwtAuth(this IServiceCollection services,
            IConfiguration config,
            Action<JwtTokenOptions>? options = null)
        {
            var opts = new JwtTokenOptions();
            options?.Invoke(opts);

            // Register options into DI
            services.Configure<JwtTokenOptions>(o =>
            {
                o.SecretConfigKey = opts.SecretConfigKey;
                o.SecretKey = opts.SecretKey;

                o.TokenExpiryValue = opts.TokenExpiryValue;
                o.TokenExpiryUnit = opts.TokenExpiryUnit;

                o.RefreshTokenExpiryValue = opts.RefreshTokenExpiryValue;
                o.RefreshTokenExpiryUnit = opts.RefreshTokenExpiryUnit;
            });

            string secret = opts.SecretKey ??
                            config.GetValue<string>(opts.SecretConfigKey ?? "JWTAuthSecretKey")
                            ?? throw new Exception("JWT secret key not provided.");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            TokenValidationParameters validationParams =
                opts.CustomTokenValidationParameters ??
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = validationParams;
                });
            services.AddScoped<IJwtTokenManager, JwtTokenManager>();
            return services;
        }
    }
}
