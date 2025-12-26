using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Models;
using Mzstruct.DB.ORM.EFCore.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.PostgreSQL.Context
{
    public class PostgreEFContext : EFContext
    {
        public PostgreEFContext(DbContextOptions options) : base(options) {}

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
