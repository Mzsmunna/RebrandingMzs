using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Mzstruct.DB.Providers.MongoDB.Configs;
using Mzstruct.Base.Contracts.IContexts;
using Mzstruct.Common.Extensions;
using Mzstruct.DB.ORM.EFCore.Context;
using Microsoft.EntityFrameworkCore;
using Mzstruct.DB.Providers.MongoDB.Context;

namespace Mzstruct.Common.Dependencies
{
    public static class DatabaseResolver
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration config)
        {
            //builder.Services.Configure<MongoDBConfig>(config.GetSection(nameof(MongoDBConfig)));
            services.Configure(config.GetSection(nameof(MongoDBConfig)).ToConfigureAction<MongoDBConfig>());
            services.AddScoped<MongoDBConfig>(sp => sp.GetRequiredService<IOptions<MongoDBConfig>>().Value);
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            return services;
        }

        public static IServiceCollection AddSqlDB(this IServiceCollection services, IConfiguration config)
        {
            //services.AddDbContext<SqlDbContext>(ServiceLifetime.Transient);
            services.AddDbContext<EFContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DatabaseContext")));
            return services;
        }
    }
}
