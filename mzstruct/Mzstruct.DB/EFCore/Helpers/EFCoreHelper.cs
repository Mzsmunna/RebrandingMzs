using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Enums;
using Mzstruct.Base.Extensions;
using Mzstruct.Base.Models;

namespace Mzstruct.DB.EFCore.Helpers
{
    public static class EFCoreHelper
    {
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

        public static void OnDefaultConfiguring(DbContextOptionsBuilder optionsBuilder, DBType db = DBType.SqlServer)
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

        public static void OnDefaultModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            //var entity = modelBuilder.Entity<BaseEntity>();
            //entity.ToTable("BaseEntities");
            //entity.HasKey(e => e.Id);
            #region common_entities_or_models
            modelBuilder.Entity<Count>(insu => { insu.HasNoKey(); });
            modelBuilder.Entity<TotalCount>(insu => { insu.HasNoKey(); });
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
