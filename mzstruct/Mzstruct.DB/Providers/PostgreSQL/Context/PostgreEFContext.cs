using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mzstruct.Base.Extensions;
using Mzstruct.DB.ORM.EFCore.Context;

namespace Mzstruct.DB.Providers.PostgreSQL.Context
{
    public class PostgreEFContext : EFContext
    {
        public PostgreEFContext(DbContextOptions options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(ConfigExtender.GetNewConfig().GetConnectionString("PostgreSQL"));
            //optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region default
            //modelBuilder.Entity<Count>(insu => { insu.HasNoKey(); });
            //modelBuilder.Entity<TotalCount>(insu => { insu.HasNoKey(); });
            #endregion
            base.OnModelCreating(modelBuilder);
        }

        #region default
        //public virtual DbSet<Count> Counts { get; set; }
        //public virtual DbSet<TotalCount> TotalCounts { get; set; }
        #endregion
    }
}
