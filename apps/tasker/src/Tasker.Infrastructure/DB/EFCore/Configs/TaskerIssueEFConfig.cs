using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.DB.EFCore.Configs;
using Tasker.Application.Features.Issues;

namespace Tasker.Infrastructure.DB.EFCore.Configs
{
    public class TaskerIssueEFConfig : BaseEntityEFConfig<TaskerIssue>
    {
        public TaskerIssueEFConfig(string? tableName = "Issue") : base(tableName) { }

        public override void Configure(EntityTypeBuilder<TaskerIssue> builder)
        {
            //builder.HasKey(u => new { u.Id });
            builder.HasIndex(e => new { e.Id, e.AssignerId, e.UserId });
            builder.Property(x => x.AssignerId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            base.Configure(builder);
        }
    }
}

