using Kernel.Drivers.Interfaces;
using Kernel.Drivers.Interfaces.Auth;
using Kernel.Managers.Auth;
using Kernel.Resources.DAL.MongoDB;
using Kernel.Resources.DAL.MongoDB.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Managers.Extensions
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
