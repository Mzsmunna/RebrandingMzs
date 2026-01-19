using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Auth.Models.Configs;
using Mzstruct.Base.Enums;
using Mzstruct.Common.Dependencies;
using Mzstruct.DB.EFCore.Context;
using Mzstruct.DB.EFCore.Entities;
using Mzstruct.DB.EFCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Common.Helpers
{
    public static class AuthCommonHelper
    {
        public static IServiceCollection AddIdentityDBContext<TContext, TIdentity>(IServiceCollection services, IConfiguration config, DBType db = DBType.SqlServer, ServiceLifetime lifeTime = ServiceLifetime.Scoped, bool includeJWT = false,
            Action<JwtTokenOptions>? jwtOptions = null) where TIdentity : UserIdentity where TContext : IdentityDBContext<TContext, TIdentity>
        {
            EFCoreHelper.AddIdentityDBContext<TContext, TIdentity>(services, config, db, lifeTime);
            services
                    //.AddIdentityCore<TIdentity>()
                    .AddIdentityCore<TIdentity>(options =>
                    {
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
