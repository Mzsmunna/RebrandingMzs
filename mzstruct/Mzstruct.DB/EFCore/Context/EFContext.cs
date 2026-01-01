using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Enums;
using Mzstruct.Base.Models;
using Mzstruct.DB.EFCore.Helpers;
using Mzstruct.DB.Helpers;

namespace Mzstruct.DB.EFCore.Context
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions options) : base(options) { }
        //public EFContext(DbContextOptions<EFContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(Console.WriteLine);
            //EFCoreHelper.OnConfiguring(optionsBuilder, dbType);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EFCoreHelper.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFContext).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            EFCoreHelper.ModifyDateTime(ChangeTracker);
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            EFCoreHelper.ModifyDateTime(ChangeTracker);
            return base.SaveChangesAsync(cancellationToken);
        }

        //private void ModifyDateTime()
        //{
        //    var entries = ChangeTracker.Entries()
        //        .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
        //    foreach (var entry in entries)
        //    {
        //        var entity = (BaseEntity)entry.Entity;
        //        entity.ModifiedAt = DateTime.UtcNow;
        //        if (entry.State == EntityState.Added) entity.CreatedAt = DateTime.UtcNow;
        //    }
        //}

        #region common_dbsets
        public virtual DbSet<Count> Counts { get; set; }
        public virtual DbSet<TotalCount> TotalCounts { get; set; }
        //public virtual DbSet<BaseUser> Users { get; set; }
        #endregion
    }
}
