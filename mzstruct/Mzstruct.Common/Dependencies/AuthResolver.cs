using Mzstruct.Auth.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Mzstruct.Auth.Contracts.IManagers;

namespace Mzstruct.Common.Dependencies
{
    public static class AuthResolver
    {
        public static IServiceCollection AddGoogleSignIn(this IServiceCollection services)
        {
            services.AddScoped<IGoogleAuthManager, GoogleAuthManager>();
            return services;
        }
    }
}
