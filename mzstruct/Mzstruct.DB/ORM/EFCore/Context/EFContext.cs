using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Mzstruct.DB.ORM.EFCore.Configs;

namespace Mzstruct.DB.ORM.EFCore.Context
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region common
            modelBuilder.Entity<Count>(insu => { insu.HasNoKey(); });
            modelBuilder.Entity<TotalCount>(insu => { insu.HasNoKey(); });

            modelBuilder.ApplyConfiguration(new BaseUserEFConfig());
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFContext).Assembly);
            #endregion
            base.OnModelCreating(modelBuilder);
        }

        #region common
        public virtual DbSet<Count> Counts { get; set; }
        public virtual DbSet<TotalCount> TotalCounts { get; set; }
        #endregion
    }
}
