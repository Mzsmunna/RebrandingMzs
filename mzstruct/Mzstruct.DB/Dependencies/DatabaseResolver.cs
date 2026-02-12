using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Enums;
using Mzstruct.DB.Contracts.IRepos;
using Mzstruct.DB.EFCore.Context;
using Mzstruct.DB.EFCore.Helpers;
using Mzstruct.DB.Providers.MongoDB.Configs;
using Mzstruct.DB.Providers.MongoDB.Context;
using Mzstruct.DB.Providers.MongoDB.Contracts.IContexts;
using Mzstruct.DB.Providers.MongoDB.Contracts.IMappers;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using Mzstruct.DB.Providers.MongoDB.Mappers;
using Mzstruct.DB.Providers.MongoDB.Repos;
using Mzstruct.DB.SQL.Helper;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Dependencies
{
    public static class DatabaseResolver
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration config)
        {
            //var readConn = config.GetSection("Database:Mongo:ReadConn").Get<ReadConn>();
            services.Configure<MongoDBConfig>(config.GetSection(nameof(MongoDBConfig)));
            services.AddTransient<MongoDBConfig>(sp => sp.GetRequiredService<IOptions<MongoDBConfig>>().Value);
            services.AddTransient<IMongoDBContext, MongoDBContext>();
            AddCoreMongoRepos(services);
            return services;
        }

        private static IServiceCollection AddCoreMongoRepos(IServiceCollection services)
        {
            services.AddScoped<IMongoEntityMap, MongoEntityMap>();
            services.AddScoped(typeof(IMongoDBRepo<>), typeof(MongoDBRepo<>));
            //services.AddScoped(typeof(IBaseUserRepository<>), typeof(BaseUserRepository<>));
            //services.AddScoped(typeof(IAuthUserRepo<>), typeof(BaseUserRepository<>));
            return services;
        }

        public static IServiceCollection AddSqlDBConnFactory(this IServiceCollection services, IConfiguration config, DBType db = DBType.SqlServer)
        {
            return SqlHelper.AddDBConnFactory(services, config, db);
        }

        public static IServiceCollection AddDBContext<TContext>(this IServiceCollection services, IConfiguration config, DBType db = DBType.SqlServer, ServiceLifetime lifeTime = ServiceLifetime.Scoped) where TContext : BaseDBContext //DbContext
        {
            return EFCoreHelper.AddDBContext<TContext>(services, config, db, lifeTime);
        }

        public static IServiceCollection AddAppDBContext<TContext>(this IServiceCollection services, IConfiguration config, DBType db = DBType.SqlServer, ServiceLifetime lifeTime = ServiceLifetime.Scoped) where TContext : AppDBContext<TContext>
        {
            return EFCoreHelper.AddAppDBContext<TContext>(services, config, db, lifeTime);
        }

        public static IServiceCollection AddDBContextFactory<TContext>(this IServiceCollection services, IConfiguration config, DBType db, ServiceLifetime lifeTime = ServiceLifetime.Scoped) where TContext : DbContext
        {
            return EFCoreHelper.AddDBContextFactory<TContext>(services, config, db, lifeTime);
        }

        public static IServiceScope ApplyDBMigration<TContext>(this IServiceScope scope) where TContext : DbContext
        {
            using var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            dbContext.Database.Migrate();
            return scope;
        }
    }
}
