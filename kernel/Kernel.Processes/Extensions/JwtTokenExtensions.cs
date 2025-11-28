using Kernel.Drivers.Enums;
using Kernel.Drivers.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Processes.Extensions
{
    public static class JwtTokenExtensions
    {
        public static IServiceCollection AddJwtAuth(
            this IServiceCollection services,
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
            //services.AddScoped<JwtTokenManager>();

            return services;
        }

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
            opts.CustomTokenValidationParameters = parameters;
            return opts;
        }
    }
}
