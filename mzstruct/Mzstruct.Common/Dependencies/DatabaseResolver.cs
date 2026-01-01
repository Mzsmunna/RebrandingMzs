using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mzstruct.Base.Enums;
using Mzstruct.DB.Contracts.IFactories;
using Mzstruct.DB.Contracts.IRepos;
using Mzstruct.DB.EFCore.Repo;
using Mzstruct.DB.Providers.MongoDB.Configs;
using Mzstruct.DB.Providers.MongoDB.Context;
using Mzstruct.DB.Providers.MongoDB.Contracts.IContexts;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using Mzstruct.DB.Providers.MongoDB.Mappers;
using Mzstruct.DB.Providers.MongoDB.Repos;
using Mzstruct.DB.SQL.Context;
using Mzstruct.DB.SQL.Factory;
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
            var conn = config.GetConnectionString("DefaultConnection");
                 //?? throw new ApplicationException("The connectiton string is null");
            if (conn is null) return services;
            
            if (dBType is DBType.SqlServer)
            {
                services.AddSingleton<IDbSqlConnFactory>(sp => new SqlServerFactory(conn));
            }
            else if (dBType is DBType.PostgreSql)
            {
                services.AddSingleton<IDbSqlConnFactory>(sp => new PostgreSqlFactory(conn));
            }
            else if (dBType is DBType.SQLite)
            {
                services.AddSingleton<IDbSqlConnFactory>(sp => new SqliteFactory(conn));
            }
            return services;
        }

        public static IServiceCollection AddSqlDBContext<TContext>(this IServiceCollection services, IConfiguration config, DBType dBType = DBType.SqlServer) where TContext : DbContext
        {
            var conn = config.GetConnectionString("DefaultConnection");
            //services.AddDbContext<EFContext>(ServiceLifetime.Transient);
            if (string.IsNullOrEmpty(conn) || dBType is DBType.InMemory) 
            {
                var dbName = config.GetConnectionString("DatabaseName") ?? "AppInMemoryDb";
                services.AddDbContext<TContext>(options =>
                    options.UseInMemoryDatabase(dbName)
                    //,ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.SqlServer) 
            {
                services.AddDbContext<TContext>(options =>
                    options.UseSqlServer(conn)
                    //,ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.PostgreSql)
            {
                services.AddDbContext<TContext>(options =>
                    options.UseNpgsql(conn)
                    //,ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.SQLite)
            {
                conn = conn ?? "Data Source=app.db";
                services.AddDbContext<TContext>(options =>
                    options.UseSqlite(conn)
                    //,ServiceLifetime.Transient
                );
            }

            //services.AddScoped(typeof(IEFCoreBaseRepo<>), typeof(EFCoreBaseRepo<>));
            //services.AddScoped(typeof(IEFCoreBaseRepo<,>), typeof(EFCoreBaseRepo<,>));
            //services.AddScoped(typeof(IEFCoreBaseRepo<>), typeof(EFCoreBaseRepo<,>));
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

        public static IServiceCollection AddSqlDBContextFactory<TContext>(this IServiceCollection services, IConfiguration config, DBType dBType) where TContext : DbContext
        {
            var conn = config.GetConnectionString("DefaultConnection");
            if (dBType is DBType.InMemory) 
            {
                var dbName = config.GetConnectionString("DatabaseName") ?? "InMemoryAppDb";
                services.AddDbContextFactory<TContext>(options =>
                    options.UseInMemoryDatabase(dbName)
                    //,ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.SqlServer) 
            {
                services.AddDbContextFactory<TContext>(options =>
                    options.UseSqlServer(conn)
                    //,ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.PostgreSql)
            {
                services.AddDbContextFactory<TContext>(options =>
                    options.UseNpgsql(conn)
                    //,ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.SQLite)
            {
                services.AddDbContextFactory<TContext>(options =>
                    options.UseSqlite(conn)
                    //,ServiceLifetime.Transient
                );
            }

            return services;
        }

        public static IServiceScope ApplyDBMigration<TContext>(this IServiceScope scope) where TContext : DbContext
        {
            using var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            dbContext.Database.Migrate();
            return scope;
        }
    }
}
