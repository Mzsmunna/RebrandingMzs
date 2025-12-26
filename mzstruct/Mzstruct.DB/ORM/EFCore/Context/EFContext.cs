using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Models;

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
            #endregion
            base.OnModelCreating(modelBuilder);
        }

        #region common
        public virtual DbSet<Count> Counts { get; set; }
        public virtual DbSet<TotalCount> TotalCounts { get; set; }
        #endregion
    }
}
