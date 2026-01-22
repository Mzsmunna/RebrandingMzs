using Microsoft.AspNetCore.Authentication;
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
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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

        public static IServiceCollection AddMvcGitHubSignIn(this IServiceCollection services, IConfiguration config)
        {
            var gitHubAuth = config.GetSection(nameof(GitHubAuth)).Get<GitHubAuth>();

            if (gitHubAuth is null)
                throw new ArgumentNullException(nameof(GitHubAuth), "GitHubAuth configuration section is missing.");

            services.AddAuthentication(gitHubAuth.Schema)
                .AddCookie(gitHubAuth.Schema)
                .AddOAuth("github", oa =>
                {
                    oa.SignInScheme = gitHubAuth.Schema;
                    // create an app on github & find ClientId & ClientSecret in https://github.com/settings/applications/appid
                    oa.ClientId = gitHubAuth.ClientId;
                    oa.ClientSecret = gitHubAuth.ClientSecret;
                    oa.CallbackPath = gitHubAuth.CallbackPath;
                    oa.AuthorizationEndpoint = gitHubAuth.AuthorizationEndpoint;
                    oa.TokenEndpoint = gitHubAuth.TokenEndpoint;
                    oa.UserInformationEndpoint = gitHubAuth.UserInformationEndpoint;
                });
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
            if (opts.SignInOptions is null)
                opts.SignInOptions = config.GetSection(nameof(SignInWith)).Get<SignInWith>();

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
                opts.TokenParameters ??
                JwtHelper.GetTokenValidationParameters(signingKey, opts.jwtAuthConfig);
            var jwtEvent = JwtHelper.GetJwtBearerEvents();
            var authBuilder = services
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
            if (opts.SignInOptions is null) return services;

            // SignIn With: GitHub, Google, Facebook, etc.
            if (opts.SignInOptions.GitHub)
            {
                var gitHubAuth = config.GetSection(nameof(GitHubAuth)).Get<GitHubAuth>();
                
                if (gitHubAuth is null)
                    throw new ArgumentNullException(nameof(GitHubAuth), "GitHubAuth configuration section is missing.");

                authBuilder
                    .AddCookie(gitHubAuth.Schema) // 1) Add Temporary cookie just for external login handshake
                    .AddGitHub(options =>
                    {
                        options.SignInScheme = gitHubAuth.Schema; // 👈 2) external cookie only, not your main auth

                        // 👇 3) MUST match GitHub OAuth app callback URL (path only)
                        // GitHub: https://localhost:7016/api/auth/RequestGitHubSignIn
                        options.CallbackPath = gitHubAuth.CallbackPath;
                        options.ClientId = gitHubAuth.ClientId;
                        options.ClientSecret = gitHubAuth.ClientSecret;

                        //In OAuth 2.0, a scope defines what permissions your app is asking for.
                        options.Scope.Add("read:user");
                        options.Scope.Add("user:email");

                        //options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                        //options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                        //options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

                        options.Events.OnCreatingTicket = async context =>
                        {
                            // You can access GitHub user info here
                            var accessToken = context.AccessToken;

                            // If needed, explicitly fetch user JSON:

                            //var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            //request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.AccessToken);

                            //var response = await context.Backchannel.SendAsync(request);
                            //response.EnsureSuccessStatusCode();

                            //var json = await response.Content.ReadAsStringAsync();
                            //using var doc = JsonDocument.Parse(json);
                            //context.RunClaimActions(doc.RootElement);
                        };
                    });
            }
             
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
