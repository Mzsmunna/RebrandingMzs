using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Auth.Dependencies;
using Mzstruct.Auth.Models.Configs;
using Mzstruct.Base.Enums;
using Mzstruct.DB.EFCore.Context;
using Mzstruct.DB.EFCore.Entities;
using Mzstruct.DB.EFCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Helpers
{
    public static class AuthHelper
    {
        public static IServiceCollection AddIdentityDBContext<TContext, TIdentity>(IServiceCollection services, IConfiguration config, DBType db = DBType.SqlServer, ServiceLifetime lifeTime = ServiceLifetime.Scoped, bool includeJWT = false,
            Action<JwtTokenOptions>? jwtOptions = null) where TIdentity : UserIdentity where TContext : IdentityDBContext<TContext, TIdentity>
        {
            EFCoreHelper.AddIdentityDBContext<TContext, TIdentity>(services, config, db, lifeTime);
            services
                    //.AddIdentityCore<TIdentity>()
                    .AddIdentityCore<TIdentity>(options =>
                    {
                        //options.SignIn.RequireConfirmedAccount = false;
                        //options.SignIn.RequireConfirmedEmail = false;

                        //options.Lockout.AllowedForNewUsers = true;
                        //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                        //options.Lockout.MaxFailedAccessAttempts = 5;

                        options.Password.RequiredLength = 6;
                        options.Password.RequireNonAlphanumeric = false;
                        options.User.RequireUniqueEmail = true;
                    })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<TContext>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();
            if (includeJWT)
                services.AddJwtAuth(config, jwtOptions);
            else
            {
                services
                .AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddIdentityCookies();
            }
            return services;
        }
    }
}
