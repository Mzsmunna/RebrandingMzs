using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Models;
using Mzstruct.DB.Contracts.IContext;
using Mzstruct.DB.EFCore.Helpers;

/// <commands> .NET & EF CLI  </commands>
/// dotnet tool list --global | -g
/// dotnet tool install --global dotnet-ef --version 7.0.11
/// dotnet add package Microsoft.EntityFrameworkCore --version 7.0.11
/// dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.11
/// dotnet ef --version
/// 
/// dotnet ef migrations add InitialCreate --output-dir Infrastructure/DB/Migrations
/// dotnet ef migrations add InitialCreate --project apps/tasker/src/Tasker.Infrastructure --startup-project apps/tasker/src/Tasker.Presentation/Tasker.RestAPI --output-dir DB/SqlServer/Migrations/
/// dotnet ef database update -p apps/tasker/src/Tasker.Infrastructure -s apps/tasker/src/Tasker.Presentation/Tasker.RestAPI
/// dotnet ef database update
/// dotnet ef database update -p Mzstruct.DB.SQL -s Mzstruct.Api --context DatabaseContext
/// 
/// <commands> Package Manager Console </commands>
/// Add-Migration InitialCreate
/// Update-Databse | Update-Database -Migration Name -ConnectionString
/// Bundle-Migration -ConnectionString
namespace Mzstruct.DB.EFCore.Context
{
    public class AppEFContext<TContext> : DbContext, IAppDBContext  where TContext : DbContext
    {
        //public EFContext(DbContextOptions options) : base(options) { }
        public AppEFContext(DbContextOptions<TContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(Console.WriteLine);
            //EFCoreHelper.OnDefaultConfiguring(optionsBuilder, dbType);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            EFCoreHelper.OnDefaultModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFContext).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
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
