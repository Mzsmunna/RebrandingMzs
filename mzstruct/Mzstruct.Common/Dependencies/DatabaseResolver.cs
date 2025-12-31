using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mzstruct.Base.Enums;
using Mzstruct.DB.Providers.MongoDB.Configs;
using Mzstruct.DB.Providers.MongoDB.Context;
using Mzstruct.DB.Providers.MongoDB.Contracts.IContexts;
using Mzstruct.DB.SQL.Context;
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
            services.Configure<MongoDBConfig>(config.GetSection(nameof(MongoDBConfig)));
            services.AddTransient<MongoDBConfig>(sp => sp.GetRequiredService<IOptions<MongoDBConfig>>().Value);
            services.AddTransient<IMongoDBContext, MongoDBContext>();
            return services;
        }

        public static IServiceCollection AddSqlConnection(this IServiceCollection services, IConfiguration config, DBType dBType = DBType.SqlServer)
        {
            var conn = config.GetConnectionString("DatabaseContext");
                 //?? throw new ApplicationException("The connectiton string is null");
            if (conn is null) return services;
            
            if (dBType is DBType.SqlServer)
            {
                services.AddTransient(sp => new SqlServerContext(conn));
            }
            else if (dBType is DBType.PostgreSql)
            {
                services.AddTransient(sp => new PostgreSqlContext(conn));
            }
            else if (dBType is DBType.SQLite)
            {
                services.AddTransient(sp => new SqliteContext(conn));
            }
            return services;
        }

        public static IServiceCollection AddSqlDBContext<TContext>(this IServiceCollection services, IConfiguration config, DBType dBType = DBType.SqlServer) where TContext : DbContext
        {
            //services.AddDbContext<EFContext>(ServiceLifetime.Transient);
            if (dBType is DBType.InMemory) 
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
                    options.UseSqlServer(config.GetConnectionString("DatabaseContext")),
                    ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.PostgreSql)
            {
                services.AddDbContext<TContext>(options =>
                    options.UseNpgsql(config.GetConnectionString("DatabaseContext")),
                    ServiceLifetime.Transient
                );
            }
            else if (dBType is DBType.SQLite)
            {
                services.AddDbContext<TContext>(options =>
                    options.UseSqlite(config.GetConnectionString("DatabaseContext")),
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

        public static IServiceCollection AddSqlDBFactory<TContext>(this IServiceCollection services, IConfiguration config, DBType dBType) where TContext : DbContext
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
