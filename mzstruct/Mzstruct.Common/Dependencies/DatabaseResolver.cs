using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mzstruct.Base.Enums;
using Mzstruct.DB.Contracts.IFactories;
using Mzstruct.DB.Providers.MongoDB.Configs;
using Mzstruct.DB.Providers.MongoDB.Context;
using Mzstruct.DB.Providers.MongoDB.Contracts.IContexts;
using Mzstruct.DB.SQL.Context;
using Mzstruct.DB.SQL.Factory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

/// <commands> .NET CLI </commands>
/// dotnet tool list --global | -g
/// dotnet tool install --global dotnet-ef --version 7.0.11
/// dotnet add package Microsoft.EntityFrameworkCore --version 7.0.11
/// dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.11
/// dotnet ef --version
/// 
/// 
/// dotnet ef migration add InitialCreate --project Mzstruct.DB.SQL --startup-project Mzstruct.Api --context DatabaseContext
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
            return services;
        }

        public static IServiceCollection AddSqlDBConnFactory(this IServiceCollection services, IConfiguration config, DBType dBType = DBType.SqlServer)
        {
            var conn = config.GetConnectionString("DatabaseContext");
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
            var conn = config.GetConnectionString("DatabaseContext");
            //services.AddDbContext<EFContext>(ServiceLifetime.Transient);
            if (string.IsNullOrEmpty(conn) || dBType is DBType.InMemory) 
            {
                var dbName = config.GetConnectionString("DatabaseName") ?? "AppInMemoryDb";
                services.AddDbContext<TContext>(options =>
                    options.UseInMemoryDatabase(dbName),
                    ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.SqlServer) 
            {
                services.AddDbContext<TContext>(options =>
                    options.UseSqlServer(conn),
                    ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.PostgreSql)
            {
                services.AddDbContext<TContext>(options =>
                    options.UseNpgsql(conn),
                    ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.SQLite)
            {
                conn = conn ?? "Data Source=app.db";
                services.AddDbContext<TContext>(options =>
                    options.UseSqlite(conn),
                    ServiceLifetime.Transient
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

        public static IServiceCollection AddSqlDBContextFactory<TContext>(this IServiceCollection services, IConfiguration config, DBType dBType) where TContext : DbContext
        {
            if (dBType is DBType.InMemory) 
            {
                var dbName = config.GetConnectionString("DatabaseName") ?? "InMemoryAppDb";
                services.AddDbContextFactory<TContext>(options =>
                    options.UseInMemoryDatabase(dbName),
                    ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.SqlServer) 
            {
                services.AddDbContextFactory<TContext>(options =>
                    options.UseSqlServer(config.GetConnectionString("DatabaseContext")),
                    ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.PostgreSql)
            {
                services.AddDbContextFactory<TContext>(options =>
                    options.UseNpgsql(config.GetConnectionString("DatabaseContext")),
                    ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.SQLite)
            {
                services.AddDbContextFactory<TContext>(options =>
                    options.UseSqlite(config.GetConnectionString("DatabaseContext")),
                    ServiceLifetime.Transient
                );
            }

            return services;
        }
    }
}
