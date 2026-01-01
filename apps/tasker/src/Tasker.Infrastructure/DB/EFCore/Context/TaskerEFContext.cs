using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Enums;
using Mzstruct.Base.Models;
using Mzstruct.DB.EFCore.Context;
using Tasker.Application.Features.Issues;
using Tasker.Application.Features.Users;

namespace Tasker.Infrastructure.DB.EFCore.Context
{
    public class TaskerEFContext : EFContext
    {
        public TaskerEFContext(DbContextOptions<TaskerEFContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskerEFContext).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
    }
}
