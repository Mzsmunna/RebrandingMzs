using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Base.Enums;
using Mzstruct.Cache.Contracts;
using Mzstruct.Cache.InMemory;
using Mzstruct.Cache.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Cache.Dependencies
{
    public static class CacheResolver
    {
        public static IServiceCollection AddCustomCaching(this IServiceCollection services, IConfiguration config)
        {
            AddCustomInMemoryCaching(services, config);
            return services;
        }

        public static IServiceCollection AddCustomInMemoryCaching(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IInMemoryCacher, InMemoryCacher>();
            return services;
        }

        public static IServiceCollection AddCustomOutputCache(this IServiceCollection services, IConfiguration config, CacheType cacheType = CacheType.InMemory)
        {
            services.AddOutputCache(); // -> ok for unauth requests; need to include custom policy or tags for auth claim checks
            
            //services.AddOutputCache(options =>
            //{
            //    options.AddBasePolicy(b => b.AddPolicy<CustomPolicy>().SetCacheKeyPrefix("custom-"), true);
            //    options.AddBasePolicy(b => b.Tag("all"), true);
            //});

            if (cacheType == CacheType.Redis)
            {
                var redisConfig = config.GetSection(nameof(RedisConfig)).Get<RedisConfig>();
                services. AddStackExchangeRedisOutputCache(options =>
                {
                    options. Configuration = config.GetConnectionString("Redis");
                    options. InstanceName = redisConfig?.PrefixName ?? "app-redis-";
                });
            }

            return services;
        }
    }
}
