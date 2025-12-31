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
        public static void OnConfiguring(DbContextOptionsBuilder optionsBuilder, DBType db = DBType.SqlServer)
        {
            if (db == DBType.PostgreSql)
            {
                optionsBuilder
                    .UseNpgsql(ConfigExtender.GetNewConfig().GetConnectionString("PostgreSQL"));
            }
            //optionsBuilder.LogTo(Console.WriteLine);
        }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region common_entities
            modelBuilder.Entity<Count>(insu => { insu.HasNoKey(); });
            modelBuilder.Entity<TotalCount>(insu => { insu.HasNoKey(); });
            //modelBuilder.ApplyConfiguration(new BaseUserEFConfig());
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFContext).Assembly);
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
