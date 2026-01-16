using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Models;
using Mzstruct.DB.Contracts.IContext;
using Mzstruct.DB.EFCore.Entities;
using Mzstruct.DB.EFCore.Helpers;

namespace Mzstruct.DB.EFCore.Context
{
    public class IdentityDBContext<TContext, TIdentity> : IdentityDbContext<TIdentity>, IAppDBContext where TContext : DbContext where TIdentity : UserIdentity
    {
        //public IdentityDBContext(DbContextOptions options) : base(options) { }
        public IdentityDBContext(DbContextOptions<TContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.LogTo(Console.WriteLine);
            //EFCoreHelper.OnDefaultConfiguring(optionsBuilder, dbType);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFContext).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            EFCoreHelper.OnDefaultModelCreating(modelBuilder);
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
        //public virtual DbSet<TEntity> Entity { get; set; }
        //public virtual DbSet<BaseUser> Users { get; set; }
        #endregion
    }
}
