using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Mzstruct.DB.ORM.EFCore.Configs;

namespace Mzstruct.DB.ORM.EFCore.Context
{
    public class EFContext : DbContext
    {
        public EFContext() { }
        public EFContext(DbContextOptions options) : base(options) { }
        public EFContext(DbContextOptions<EFContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region common_entities
            modelBuilder.Entity<Count>(insu => { insu.HasNoKey(); });
            modelBuilder.Entity<TotalCount>(insu => { insu.HasNoKey(); });
            modelBuilder.ApplyConfiguration(new BaseUserEFConfig());
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFContext).Assembly);
            #endregion
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ModifyDateTime();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ModifyDateTime();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ModifyDateTime()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                entity.ModifiedAt = DateTime.UtcNow;
                if (entry.State == EntityState.Added) entity.CreatedAt = DateTime.UtcNow;
            }
        }

        #region common
        public virtual DbSet<Count> Counts { get; set; }
        public virtual DbSet<TotalCount> TotalCounts { get; set; }
        #endregion
    }
}
