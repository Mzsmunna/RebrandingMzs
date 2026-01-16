using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Enums;
using Mzstruct.Base.Extensions;
using Mzstruct.Base.Models;
using Mzstruct.DB.Contracts.IContext;
using Mzstruct.DB.Contracts.IRepos;
using Mzstruct.DB.EFCore.Context;
using Mzstruct.DB.EFCore.Entities;
using Mzstruct.DB.EFCore.Repo;
using System.Data;

namespace Mzstruct.DB.EFCore.Helpers
{
    public static class EFCoreHelper
    {
        public static void AddDBContext(DbContextOptionsBuilder optionsBuilder, DBType db = DBType.SqlServer)
        {
            if (db == DBType.PostgreSql)
            {
                optionsBuilder
                    .UseNpgsql(ConfigExtender.GetNewConfig().GetConnectionString("PostgreSQL"));
            }
            //optionsBuilder.UseSeeding((context, _) =>
            //{
            //    if (!context.Set<BaseEntity>().Any())
            //    {
            //        context.Set<BaseEntity>().Add(new BaseUser
            //        {
            //            Id = Guid.CreateVersion7().ToString(),
            //            Name = "Mzs Munna",
            //            Email = "mzs.munna@gmail.com",
            //            Username = "mzsmunna",
            //            Password = "P@ssw0rd123",
            //            Role = "Admin",
            //        });
            //    }
            //})
            //.UseAsyncSeeding(async (context, _, cancellationToken) =>
            //{
            //    if (!context.Set<BaseEntity>().Any())
            //    {
            //        context.Set<BaseEntity>().Add(new BaseUser
            //        {
            //            Id = Guid.CreateVersion7().ToString(),
            //            Name = "Mzs Munna",
            //            Email = "mzs.munna@gmail.com",
            //            Username = "mzsmunna",
            //            Password = "P@ssw0rd123",
            //            Role = "Admin",
            //        });
            //    }
            //});
            //optionsBuilder.LogTo(Console.WriteLine);
        }

        public static IServiceCollection AddDBContext<TContext>(IServiceCollection services, IConfiguration config, DBType db = DBType.SqlServer, ServiceLifetime lifeTime = ServiceLifetime.Scoped) where TContext : BaseDBContext //DbContext
        {
            var conn = config.GetConnectionString("DefaultConnection");
            //services.AddDbContext<EFContext>(lifeTime);

            if (string.IsNullOrEmpty(conn) || db is DBType.InMemory) 
            {
                var dbName = config.GetConnectionString("DatabaseName") ?? "AppInMemoryDb";
                services.AddDbContext<TContext>(options =>
                    options.UseInMemoryDatabase(dbName)
                   ,lifeTime
                );
            }
            else if (db is DBType.SqlServer) 
            {
                services.AddDbContext<TContext>(options =>
                    options.UseSqlServer(conn)
                    //), sqlOption =>
                    //{
                    //    sqlOption.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    //})
                    ,lifeTime
                );
            }
            else if (db is DBType.PostgreSql)
            {
                services.AddDbContext<TContext>(options =>
                    options.UseNpgsql(conn)
                    ,lifeTime
                );
            }
            else if (db is DBType.SQLite)
            {
                conn = conn ?? "Data Source=app.db";
                services.AddDbContext<TContext>(options =>
                    options.UseSqlite(conn)
                    ,lifeTime
                );
            }

            services.AddScoped<IAppDBContext, TContext>();
            services.AddScoped(typeof(IEFCoreBaseRepo<>), typeof(EFCoreBaseRepo<>));
            //services.AddScoped(typeof(IEFCoreBaseRepo<,>), typeof(EFCoreBaseRepo<,>));
            //services.AddScoped(typeof(IEFCoreBaseRepo<>), typeof(EFCoreBaseRepo<,>));
            return services;
        }

        public static IServiceCollection AddAppDBContext<TContext>(IServiceCollection services, IConfiguration config, DBType db = DBType.SqlServer, ServiceLifetime lifeTime = ServiceLifetime.Scoped) where TContext : AppDBContext<TContext>
        {
            var conn = config.GetConnectionString("DefaultConnection");
            //services.AddDbContext<EFContext>(lifeTime);

            if (string.IsNullOrEmpty(conn) || db is DBType.InMemory) 
            {
                var dbName = config.GetConnectionString("DatabaseName") ?? "AppInMemoryDb";
                services.AddDbContext<TContext>(options =>
                    options.UseInMemoryDatabase(dbName)
                    ,lifeTime
                );
            }
            else if (db is DBType.SqlServer) 
            {
                services.AddDbContext<TContext>(options =>
                    options.UseSqlServer(conn)
                    //), sqlOption =>
                    //{
                    //    sqlOption.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    //})
                    ,lifeTime
                );
            }
            else if (db is DBType.PostgreSql)
            {
                services.AddDbContext<TContext>(options =>
                    options.UseNpgsql(conn)
                    ,lifeTime
                );
            }
            else if (db is DBType.SQLite)
            {
                conn = conn ?? "Data Source=app.db";
                services.AddDbContext<TContext>(options =>
                    options.UseSqlite(conn)
                    ,lifeTime
                );
            }

            services.AddScoped<IAppDBContext, TContext>();
            services.AddScoped(typeof(IEFCoreBaseRepo<>), typeof(EFCoreBaseRepo<>));
            return services;
        }

        public static IServiceCollection AddIdentityDBContext<TContext, TIdentity>(IServiceCollection services, IConfiguration config, DBType db = DBType.SqlServer, ServiceLifetime lifeTime = ServiceLifetime.Scoped) where TIdentity : UserIdentity where TContext : IdentityDBContext<TContext, TIdentity>
        {
            var conn = config.GetConnectionString("DefaultConnection");
            //services.AddDbContext<EFContext>(lifeTime);

            if (string.IsNullOrEmpty(conn) || db is DBType.InMemory) 
            {
                var dbName = config.GetConnectionString("DatabaseName") ?? "AppInMemoryDb";
                services.AddDbContext<TContext>(options =>
                    options.UseInMemoryDatabase(dbName)
                    ,lifeTime
                );
            }
            else if (db is DBType.SqlServer) 
            {
                services.AddDbContext<TContext>(options =>
                    options.UseSqlServer(conn)
                    //), sqlOption =>
                    //{
                    //    sqlOption.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    //})
                    ,lifeTime
                );
            }
            else if (db is DBType.PostgreSql)
            {
                services.AddDbContext<TContext>(options =>
                    options.UseNpgsql(conn)
                    ,lifeTime
                );
            }
            else if (db is DBType.SQLite)
            {
                conn = conn ?? "Data Source=app.db";
                services.AddDbContext<TContext>(options =>
                    options.UseSqlite(conn)
                    ,lifeTime
                );
            }
            
            services.AddScoped<IAppDBContext, TContext>();
            services.AddScoped(typeof(IEFCoreBaseRepo<>), typeof(EFCoreBaseRepo<>));
            return services;
        }

        public static IServiceCollection AddDBContextFactory<TContext>(IServiceCollection services, IConfiguration config, DBType dBType, ServiceLifetime lifeTime = ServiceLifetime.Scoped) where TContext : DbContext
        {
            var conn = config.GetConnectionString("DefaultConnection");
            if (dBType is DBType.InMemory) 
            {
                var dbName = config.GetConnectionString("DatabaseName") ?? "AppInMemoryDb";
                services.AddDbContextFactory<TContext>(options =>
                    options.UseInMemoryDatabase(dbName)
                    ,lifeTime
                );
            }
            else if (dBType is DBType.SqlServer) 
            {
                services.AddDbContextFactory<TContext>(options =>
                    options.UseSqlServer(conn)
                    ,lifeTime
                );
            }
            else if (dBType is DBType.PostgreSql)
            {
                services.AddDbContextFactory<TContext>(options =>
                    options.UseNpgsql(conn)
                    ,lifeTime
                );
            }
            else if (dBType is DBType.SQLite)
            {
                services.AddDbContextFactory<TContext>(options =>
                    options.UseSqlite(conn)
                    ,lifeTime
                );
            }
            return services;
        }

        public static ValueComparer<List<T>> VirtualListCompare<T>()
        {
            var comparer = new ValueComparer<List<T>>(
                (a, b) =>
                    ReferenceEquals(a, b) ||
                    (a != null && b != null && a.SequenceEqual(b)),
                v => v == null
                    ? 0
                    : v.Aggregate(0, (h, e) => HashCode.Combine(h, e)),
                v => v == null ? null! : v.ToList()
            );
            return comparer;
        }

        public static void OnDefaultModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            //var entity = modelBuilder.Entity<BaseEntity>();
            //entity.ToTable("BaseEntities");
            //entity.HasKey(e => e.Id);
            #region common_entities_or_models
            modelBuilder.Entity<Count>(insu => { insu.HasNoKey(); });
            modelBuilder.Entity<Count>().ToTable("Count", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<TotalCount>(insu => { insu.HasNoKey(); });
            modelBuilder.Entity<TotalCount>().ToTable("TotalCount", t => t.ExcludeFromMigrations());
            //modelBuilder.ApplyConfiguration(new BaseUserEFConfig());
            #endregion
        }

        public static void ModifyDateTime(ChangeTracker changeTracker)
        {
            var entries = changeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                var entity = entry.Entity as BaseEntity;
                if (entity is null) continue;
                entity.ModifiedAt = DateTime.UtcNow;
                if (entry.State == EntityState.Added) entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}
