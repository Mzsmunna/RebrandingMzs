using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Mzstruct.DB.Providers.MongoDB;
using Mzstruct.DB.Providers.MongoDB.Configs;
using Mzstruct.Base.Contracts.IContexts;
using Mzstruct.Common.Extensions;

namespace Mzstruct.Common.Dependencies
{
    public static class DatabaseResolver
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
