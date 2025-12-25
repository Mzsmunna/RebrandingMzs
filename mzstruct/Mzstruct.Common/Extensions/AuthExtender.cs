using Mzstruct.Auth.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Mzstruct.Base.Contracts.IManagers.IAuth;

namespace Mzstruct.Common.Extensions
{
    public static class AuthExtender
    {
        public static IServiceCollection AddGoogleSignIn(this IServiceCollection services)
        {
            services.AddScoped<IGoogleAuthManager, GoogleAuthManager>();
            return services;
        }
    }
}
