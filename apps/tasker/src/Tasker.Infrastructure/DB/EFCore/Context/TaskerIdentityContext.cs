using Microsoft.EntityFrameworkCore;
using Mzstruct.DB.Contracts.IContext;
using Mzstruct.DB.EFCore.Context;
using Tasker.Application.Features.Issues;
using Tasker.Application.Features.Users;
using Tasker.Infrastructure.DB.EFCore.Configs;

namespace Tasker.Infrastructure.DB.EFCore.Context
{
    public class TaskerIdentityContext : IdentityDBContext<TaskerIdentityContext, TaskerUser>, IAppDBContext
    {
        public TaskerIdentityContext(DbContextOptions<TaskerIdentityContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskerIdentityContext).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.ApplyConfiguration(new TaskerUserEFConfig());
            modelBuilder.ApplyConfiguration(new TaskerIssueEFConfig());
            base.OnModelCreating(modelBuilder);
        }

        //public virtual DbSet<TaskerUser> Users { get; set; }
        public virtual DbSet<TaskerIssue> Issues { get; set; }
    }
}
