using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Mzstruct.Auth.Contracts.IHandlers;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Helpers;
using Mzstruct.Auth.Interceptors;
using Mzstruct.Auth.Managers;
using Mzstruct.Auth.Models.Configs;
using Mzstruct.Auth.Policies;
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

        public static IServiceCollection AddCookieAuth(this IServiceCollection services, string? cookieName = "")
        {
            if (string.IsNullOrEmpty(cookieName)) cookieName = "AppCookieAuth";
            services.AddAuthentication(cookieName)
                .AddCookie(cookieName, options =>
                {
                    options.Cookie.Name = cookieName;
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });
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

            var signingKey = JwtHelper.GetSymmetricSecurityKey(opts, config);
            var validationParams =
                opts.CustomTokenValidationParameters ??
                JwtHelper.GetTokenValidationParameters(signingKey, opts.jwtAuthConfig);
            var jwtEvent = JwtHelper.GetJwtBearerEvents();

            services
                //.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = validationParams;
                    options.Events = jwtEvent;
                });
            services.AddScoped<IJwtTokenManager, JwtTokenManager>();
            return services;
        }

        public static IServiceCollection AddDefaultAuthPolicies(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireRole(config["AdminRoles"]?.Split(',') ?? []);
                });
                options.AddPolicy("UserPolicy", policy =>
                {
                    policy.RequireRole(config["UserRoles"]?.Split(',') ?? []);
                });
            });
            return services;
        }

        public static IServiceCollection AddCustomAuthPolicies(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<ICustomAuthorizer, CustomAuthorizer>();
			services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomPolicy", policy =>
                {
                    policy.Requirements.Add(new CustomAuthorizationRequirement());
                });
            });
            return services;
        }
    }
}
