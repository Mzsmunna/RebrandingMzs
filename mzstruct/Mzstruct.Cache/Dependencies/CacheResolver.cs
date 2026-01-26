using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Cache.Contracts;
using Mzstruct.Cache.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Cache.Dependencies
{
    public static class CacheResolver
    {
        public static IServiceCollection AddCustomCaching(this IServiceCollection services)
        {
            AddCustomInMemoryCaching(services);
            return services;
        }

        public static IServiceCollection AddCustomInMemoryCaching(this IServiceCollection services)
        {
            services.AddScoped<IInMemoryCacher, InMemoryCacher>();
            return services;
        }
    }
}
