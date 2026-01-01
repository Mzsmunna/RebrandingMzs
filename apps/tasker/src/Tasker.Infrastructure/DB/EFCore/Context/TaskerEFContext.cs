using Microsoft.EntityFrameworkCore;
using Mzstruct.DB.EFCore.Context;
using Tasker.Application.Features.Issues;
using Tasker.Application.Features.Users;
using Tasker.Infrastructure.DB.EFCore.Configs;

namespace Tasker.Infrastructure.DB.EFCore.Context
{
    public class TaskerEFContext : EFContext
    {
        public TaskerEFContext(DbContextOptions<TaskerEFContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskerEFContext).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.ApplyConfiguration(new UserEFConfig());
            modelBuilder.ApplyConfiguration(new IssueEFConfig());
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
    }
}
