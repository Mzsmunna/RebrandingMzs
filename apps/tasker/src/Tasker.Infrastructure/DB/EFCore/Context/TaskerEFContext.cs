using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Enums;
using Mzstruct.DB.ORM.EFCore.Context;
using Tasker.Application.Features.Issues;
using Tasker.Application.Features.Users;

namespace Tasker.Infrastructure.DB.EFCore.Context
{
    public class TaskerEFContext : EFContext
    {
        public TaskerEFContext(DbContextOptions<TaskerEFContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
    }
}
