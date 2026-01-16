using Microsoft.EntityFrameworkCore;
using Mzstruct.DB.Contracts.IContext;
using Mzstruct.DB.EFCore.Helpers;

/// <commands> .NET & EF CLI  </commands>
/// dotnet tool install --global dotnet-ef
/// dotnet tool list --global | -g
/// dotnet tool install --global dotnet-ef --version 10.0.1
/// dotnet add package Microsoft.EntityFrameworkCore --version 7.0.11
/// dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.11
/// dotnet ef --version
/// 
/// dotnet ef migrations add InitialCreate --output-dir App/DB/Migrations
/// dotnet ef database update
/// dotnet ef database update -p Mzstruct.DB.SQL -s Mzstruct.Api --context DatabaseContext
/// dotnet ef database update -p apps/tasker/src/Tasker.Infrastructure -s apps/tasker/src/Tasker.Presentation/Tasker.RestAPI
/// 
/// dotnet ef migrations add InitialMigrate --project apps/tasker/src/Tasker.Infrastructure --startup-project apps/tasker/src/Tasker.Presentation/Tasker.RestAPI --output-dir DB/SqlServer/Migrations/ --context TaskerEFContext
/// dotnet ef database update InitialMigrate --project apps/tasker/src/Tasker.Infrastructure --startup-project apps/tasker/src/Tasker.Presentation/Tasker.RestAPI --context TaskerEFContext
/// 
/// <commands> Package Manager Console </commands>
/// Add-Migration InitialCreate
/// Update-Databse | Update-Database -Migration Name -ConnectionString
/// Bundle-Migration -ConnectionString
/// 
namespace Mzstruct.DB.EFCore.Context
{
    public class BaseDBContext : DbContext, IAppDBContext
    {
        public BaseDBContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
