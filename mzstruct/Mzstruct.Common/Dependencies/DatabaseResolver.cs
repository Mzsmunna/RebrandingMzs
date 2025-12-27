using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Mzstruct.Base.Contracts.IContexts;
using Mzstruct.Base.Enums;
using Mzstruct.Common.Extensions;
using Mzstruct.DB.ORM.EFCore.Context;
using Mzstruct.DB.Providers.MongoDB.Configs;
using Mzstruct.DB.Providers.MongoDB.Context;
using System;
using System.Collections.Generic;
using System.Text;

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

        public static IServiceCollection AddSqlDBContext(this IServiceCollection services, IConfiguration config, DBType dBType)
        {
            services.AddDbContext<EFContext>(ServiceLifetime.Transient);
            if (dBType == DBType.SqlServer) 
            {
                services.AddDbContext<EFContext>(options =>
                    options.UseSqlServer(config.GetConnectionString("DatabaseContext"))
                );
            }
            return services;

           //Action configureDb = dBType switch
           //{
           //     DBType.SqlServer => () =>
           //         services.AddDbContext<EFContext>(options =>
           //             options.UseSqlServer(
           //                 config.GetConnectionString("DatabaseContext")
           //             )),
           //     _ => () => services.AddDbContext<EFContext>(ServiceLifetime.Transient)
           //};
           // configureDb();
        }

        public static IServiceCollection AddSqlDBFactory(this IServiceCollection services, IConfiguration config, DBType dBType)
        {
            if (dBType == DBType.SqlServer)
            {
               services.AddDbContextFactory<EFContext>(options =>
                    options.UseSqlServer(config.GetConnectionString("DatabaseContext"))
               );
            }
            return services;
        }
    }
}
