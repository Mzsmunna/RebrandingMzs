using Kernel.Drivers.Interfaces;
using Kernel.Resources.DAL.MongoDB;
using Kernel.Resources.DAL.MongoDB.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Managers.Extensions
{
    public static class MongoDbExtender
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            //builder.Services.Configure<MongoDBConfig>(_configuration.GetSection(nameof(MongoDBConfig)));
            services.Configure(configuration.GetSection(nameof(MongoDBConfig)).ToConfigureAction<MongoDBConfig>());
            services.AddScoped<MongoDBConfig>(sp => sp.GetRequiredService<IOptions<MongoDBConfig>>().Value);
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            return services;
        }
    }
}
