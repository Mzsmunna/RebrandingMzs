using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Auth.Configs;
using Mzstruct.Auth.Contracts.IHandlers;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Auth.Helpers;
using Mzstruct.Auth.Interceptors;
using Mzstruct.Auth.Managers;
using Mzstruct.Auth.Policies;
using Mzstruct.Auth.Services;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Enums;
using Mzstruct.Base.Patterns.CQRS;
using Mzstruct.DB.EFCore.Context;
using Mzstruct.DB.EFCore.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Mzstruct.Auth.Dependencies
{
    public static class AuthResolver
    {
        public static IServiceCollection AddIdentityAuth<TContext, TIdentity>(this IServiceCollection services, 
            IConfiguration config, 
            DBType db = DBType.SqlServer, 
            ServiceLifetime lifeTime = ServiceLifetime.Scoped, 
            bool includeJWT = false,
            Action<JwtTokenOptions>? jwtOptions = null) where TIdentity : UserIdentity where TContext : IdentityDBContext<TContext, TIdentity>
        {
            return AuthHelper.AddIdentityDBContext<TContext, TIdentity>(services, config, db, lifeTime, includeJWT, jwtOptions);
        }

        public static IServiceCollection AddIdentityDBContext<TContext, TIdentity>(this IServiceCollection services, IConfiguration config, DBType db = DBType.SqlServer, ServiceLifetime lifeTime = ServiceLifetime.Scoped, bool includeJWT = false,
            Action<JwtTokenOptions>? jwtOptions = null) where TIdentity : UserIdentity where TContext : IdentityDBContext<TContext, TIdentity>
        {
            //return EFCoreHelper.AddIdentityDBContext<TContext, TIdentity>(services, config, db, lifeTime);
            return AuthHelper.AddIdentityDBContext<TContext, TIdentity>(services, config, db, lifeTime, includeJWT, jwtOptions);
        }

        public static IServiceCollection AddAppAuth<TIdentity>(this IServiceCollection services, IConfiguration config) where TIdentity : BaseUser
        {
            services.AddScoped<IBasicAuthService, BasicAuthService<TIdentity>>();
            services.AddScoped<IOAuthService, OAuthService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddCommandQueryHandlers<Auth>();
            return services;
        }

        public static IServiceCollection AddMvcGitHubSignIn(this IServiceCollection services, IConfiguration config)
        {
            var gitHubAuth = config.GetSection("OAuthSignIn:GitHubAuth").Get<GitHubAuth>();
            //if (gitHubAuth is null)
            //    throw new ArgumentNullException(nameof(GitHubAuth), "GitHubAuth configuration section is missing.");
            if (gitHubAuth is null || !gitHubAuth.IsEnabled) return services;
            if (string.IsNullOrEmpty(gitHubAuth.Schema))
                    gitHubAuth.Schema = "External";
            services.AddAuthentication(gitHubAuth.Schema)
                .AddCookie(gitHubAuth.Schema)
                .AddOAuth("github", oa =>
                {
                    oa.SignInScheme = gitHubAuth.Schema;
                    // create an app on github & find ClientId & ClientSecret in https://github.com/settings/applications/appid
                    oa.ClientId = gitHubAuth.ClientId;
                    oa.ClientSecret = gitHubAuth.ClientSecret;
                    oa.CallbackPath = gitHubAuth.CallbackPath;
                    oa.AuthorizationEndpoint = gitHubAuth.AuthZEndpoint;
                    oa.TokenEndpoint = gitHubAuth.TokenEndpoint;
                    oa.UserInformationEndpoint = gitHubAuth.UserInfoEndpoint;
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
            var jwtOptions = new JwtTokenOptions();
            options?.Invoke(jwtOptions);
            if (jwtOptions.jwtAuthConfig is null)
                jwtOptions.jwtAuthConfig = config.GetSection(nameof(JWTAuth)).Get<JWTAuth>();
            if (jwtOptions.jwtAuthConfig != null && 
                !string.IsNullOrEmpty(jwtOptions.jwtAuthConfig.SecretKey) &&
                string.IsNullOrEmpty(jwtOptions.SecretKey))
                jwtOptions.SecretKey = jwtOptions.jwtAuthConfig.SecretKey;
            if (string.IsNullOrEmpty(jwtOptions.ExternalSchema))
                    jwtOptions.ExternalSchema = "External";
            var signingKey = JwtHelper.GetSymmetricSecurityKey(jwtOptions, config);
            if (jwtOptions.TokenParameters is null)
                jwtOptions.TokenParameters = JwtHelper.GetTokenValidationParameters(signingKey, jwtOptions.jwtAuthConfig);

            // Register options into DI
            services.Configure<JwtTokenOptions>(cfg =>
            {
                //cfg = jwtOptions;
                cfg.SecretConfigKey = jwtOptions.SecretConfigKey;
                cfg.SecretKey = jwtOptions.SecretKey;

                cfg.TokenExpiryValue = jwtOptions.TokenExpiryValue;
                cfg.TokenExpiryUnit = jwtOptions.TokenExpiryUnit;

                cfg.RefreshTokenExpiryValue = jwtOptions.RefreshTokenExpiryValue;
                cfg.RefreshTokenExpiryUnit = jwtOptions.RefreshTokenExpiryUnit;

                cfg.jwtAuthConfig = jwtOptions.jwtAuthConfig;
                cfg.TokenParameters = jwtOptions.TokenParameters;
                //cfg.SignInOptions = jwtOptions.SignInOptions;
            });

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
                    options.TokenValidationParameters = jwtOptions.TokenParameters;
                    options.Events = jwtEvent;
                });
            services.AddScoped<IJwtTokenManager, JwtTokenManager>();
            if (jwtOptions.EnableOAuth is false) return services;

            // 👇 SignIn With: GitHub
            var gitHubAuth = config.GetSection("OAuthSignIn:GitHubAuth").Get<GitHubAuth>();       
            if (gitHubAuth != null && gitHubAuth.IsEnabled)
            {
                if (string.IsNullOrEmpty(gitHubAuth.ClientId) ||
                    string.IsNullOrEmpty(gitHubAuth.ClientSecret) ||
                    string.IsNullOrEmpty(gitHubAuth.CallbackPath))
                    throw new ArgumentNullException(nameof(GitHubAuth), "GitHubAuth configuration section is incomplete.");
                if (string.IsNullOrEmpty(gitHubAuth.Schema))
                    gitHubAuth.Schema = jwtOptions.ExternalSchema;
                authBuilder
                    .AddCookie(gitHubAuth.Schema) // 1) Add Temporary cookie just for external login handshake
                    .AddGitHub(options =>
                    {
                        options.SignInScheme = gitHubAuth.Schema; // 👈 2) external cookie only, not your main auth

                        // 👇 3) MUST match GitHub OAuth app callback URL (path only)                      
                        options.CallbackPath = gitHubAuth.CallbackPath; // GitHub: https://localhost:7016/api/auth/RequestGitHubSignIn

                        // create an app on github & find ClientId & ClientSecret in https://github.com/settings/applications/appid
                        options.ClientId = gitHubAuth.ClientId;
                        options.ClientSecret = gitHubAuth.ClientSecret;

                        //In OAuth 2.0, a scope defines what permissions your app is asking for.
                        options.Scope.Add("read:user");
                        options.Scope.Add("user:email");

                        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

                        options.Events.OnCreatingTicket = async context =>
                        {
                            // You can access GitHub user info here
                            var accessToken = context.AccessToken;

                            // 👇 If needed, explicitly fetch user JSON:

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

            // 👇 SignIn With: Facebook
            var fbAuth = config.GetSection("OAuthSignIn:FacebookAuth").Get<FacebookAuth>();
            if (fbAuth != null && fbAuth.IsEnabled)
            {
                if (string.IsNullOrEmpty(fbAuth.ClientId) ||
                    string.IsNullOrEmpty(fbAuth.ClientSecret) ||
                    string.IsNullOrEmpty(fbAuth.CallbackPath))
                    throw new ArgumentNullException(nameof(FacebookAuth), "Facebook configuration section is incomplete.");
                if (string.IsNullOrEmpty(fbAuth.Schema))
                    fbAuth.Schema = jwtOptions.ExternalSchema;
                authBuilder
                    .AddFacebook("Facebook", options =>
                    {
                        options.AppId = fbAuth.ClientId;
                        options.AppSecret = fbAuth.ClientSecret;

                        // Where Facebook redirects back (must match in FB Console)
                        options.CallbackPath = fbAuth.CallbackPath;
                        options.SignInScheme = fbAuth.Schema; // 👈 2) external cookie only, not your main auth

                        // what we want from Facebook
                        options.SaveTokens = true;
                        options.Scope.Add("email");

                        // default fields: id,name
                        options.Fields.Add("email");
                        options.Fields.Add("first_name");
                        options.Fields.Add("last_name");
                    });
            }

            // 👇 SignIn With: Google
            var googleAuth = config.GetSection("OAuthSignIn:GoogleAuth").Get<GoogleAuth>();           
            if (googleAuth != null && googleAuth.IsEnabled)
            {
                services.AddScoped<IGoogleAuthManager, GoogleAuthManager>();
                if (string.IsNullOrEmpty(googleAuth.ClientId) ||
                    string.IsNullOrEmpty(googleAuth.ClientSecret) ||
                    string.IsNullOrEmpty(googleAuth.CallbackPath))
                    throw new ArgumentNullException(nameof(GoogleAuth), "Google configuration section is incomplete.");
                if (string.IsNullOrEmpty(googleAuth.Schema))
                    googleAuth.Schema = jwtOptions.ExternalSchema;
                authBuilder
                    .AddGoogle("Google", options =>
                    {
                        options.ClientId = googleAuth.ClientId;
                        options.ClientSecret = googleAuth.ClientSecret;
                        options.CallbackPath = googleAuth.CallbackPath;

                        options.SignInScheme = googleAuth.Schema;   // <- use the external cookie
                        options.SaveTokens = true;           // optional, but handy

                        // Map extra claims if you want
                        options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                        options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                        options.ClaimActions.MapJsonKey("picture", "picture");
                    });
            }
            
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
