using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mzstruct.Base.Enums;
using Mzstruct.DB.EFCore.Context;
using Mzstruct.DB.EFCore.Helpers;
using Mzstruct.DB.Providers.MongoDB.Configs;
using Mzstruct.DB.Providers.MongoDB.Context;
using Mzstruct.DB.Providers.MongoDB.Contracts.IContexts;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using Mzstruct.DB.Providers.MongoDB.Mappers;
using Mzstruct.DB.Providers.MongoDB.Repos;
using Mzstruct.DB.SQL.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Mzstruct.Common.Dependencies
{
    public static class DatabaseResolver
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration config)
        {
            //var readConn = config.GetSection("Database:Mongo:ReadConn").Get<ReadConn>();
            services.Configure<MongoDBConfig>(config.GetSection(nameof(MongoDBConfig)));
            services.AddTransient<MongoDBConfig>(sp => sp.GetRequiredService<IOptions<MongoDBConfig>>().Value);
            services.AddTransient<IMongoDBContext, MongoDBContext>();
            AddMongoEntities(services);
            AddMongoRepositories(services);
            return services;
        }

        private static IServiceCollection AddMongoEntities(IServiceCollection services)
        {
            services.AddScoped<BaseUserEntityMap>();
            return services;
        }

        private static IServiceCollection AddMongoRepositories(IServiceCollection services)
        {
            services.AddScoped<IBaseUserRepository, BaseUserRepository>();
            return services;
        }

        public static IServiceCollection AddSqlDBConnFactory(this IServiceCollection services, IConfiguration config, DBType dBType = DBType.SqlServer)
        {
            return SqlHelper.AddDBConnFactory(services, config, dBType);
        }

        public static IServiceCollection AddEFCoreDBContext<TContext>(this IServiceCollection services, IConfiguration config, DBType dBType = DBType.SqlServer) where TContext : AppDBContext<TContext> //DbContext
        {
            return EFCoreHelper.AddDBContext<TContext>(services, config, dBType);
        }

        public static IServiceCollection AddEFCoreDBContextFactory<TContext>(this IServiceCollection services, IConfiguration config, DBType dBType) where TContext : DbContext
        {
            return EFCoreHelper.AddDBContextFactory<TContext>(services, config, dBType);
        }

        public static IServiceScope ApplyDBMigration<TContext>(this IServiceScope scope) where TContext : DbContext
        {
            using var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            dbContext.Database.Migrate();
            return scope;
        }
    }
}
