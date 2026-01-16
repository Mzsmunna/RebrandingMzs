using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Mzstruct.Auth.Configs;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Managers;
using Mzstruct.Base.Enums;
using Mzstruct.Common.Helpers;
using Mzstruct.DB.EFCore.Context;
using Mzstruct.DB.EFCore.Entities;
using Mzstruct.DB.EFCore.Helpers;
using System.Text;

namespace Mzstruct.Common.Dependencies
{
    public static class AuthResolver
    {
        public static IServiceCollection AddIdentityAuth<TContext, TIdentity>(this IServiceCollection services, IConfiguration config, DBType db = DBType.SqlServer, ServiceLifetime lifeTime = ServiceLifetime.Scoped, bool includeJWT = false,
            Action<JwtTokenOptions>? jwtOptions = null) where TIdentity : UserIdentity where TContext : IdentityDBContext<TContext, TIdentity>
        {
            return AuthCommonHelper.AddIdentityDBContext<TContext, TIdentity>(services, config, db, lifeTime, includeJWT, jwtOptions);
        }

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
            if (opts.jwtAuthConfig is null)
                opts.jwtAuthConfig = config.GetSection(nameof(JWTAuth)).Get<JWTAuth>();
            if (opts.jwtAuthConfig != null && 
                string.IsNullOrEmpty(opts.SecretKey))
                opts.SecretKey = opts.jwtAuthConfig.SecretKey;

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

            var validationParams =
                opts.CustomTokenValidationParameters ??
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero, //TimeSpan.FromSeconds(30)
                    IssuerSigningKey = signingKey,
                    ValidIssuer = opts.jwtAuthConfig?.Issuer,
                    ValidAudience = opts.jwtAuthConfig?.Audience,
                };

            var jwtEvent = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) 
                        && (path.StartsWithSegments("/jwtevents"))) context.Token = accessToken;
                        return Task.CompletedTask;
                    }
                };

            services
                //.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = validationParams;
                    options.Events = jwtEvent;
                });
            services.AddScoped<IJwtTokenManager, JwtTokenManager>();
            return services;
        }
    }
}
