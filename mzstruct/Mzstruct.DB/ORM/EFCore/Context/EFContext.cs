using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Enums;
using Mzstruct.Base.Models;
using Mzstruct.DB.Helpers;
using Mzstruct.DB.ORM.EFCore.Helpers;
using System.Reflection.Emit;

namespace Mzstruct.DB.ORM.EFCore.Context
{
    public class EFContext : DbContext
    {
        protected readonly DBType _dbType = DBType.SqlServer;

        public EFContext(DBType dbType = DBType.SqlServer) 
        {
            _dbType = dbType;
        }
        public EFContext(DbContextOptions options, DBType dbType = DBType.SqlServer) : base(options) { }
        public EFContext(DbContextOptions<EFContext> options, DBType dbType = DBType.SqlServer) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(Console.WriteLine);
            EFCoreHelper.OnConfiguring(optionsBuilder, _dbType);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EFCoreHelper.OnModelCreating(modelBuilder, _dbType);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            EFCoreHelper.ModifyDateTime(ChangeTracker, _dbType);
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

        #region common
        public virtual DbSet<Count> Counts { get; set; }
        public virtual DbSet<TotalCount> TotalCounts { get; set; }
        //public virtual DbSet<BaseUser> Users { get; set; }
        #endregion
    }
}
