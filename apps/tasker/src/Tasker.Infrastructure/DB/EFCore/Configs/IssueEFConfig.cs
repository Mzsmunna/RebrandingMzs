using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.DB.EFCore.Configs;
using Tasker.Application.Features.Issues;

namespace Tasker.Infrastructure.DB.EFCore.Configs
{
    public class IssueEFConfig : BaseEntityEFConfig<Issue>
    {
        public IssueEFConfig(string? tableName = "Issue") : base(tableName) { }

        public override void Configure(EntityTypeBuilder<Issue> builder)
        {
            //builder.HasKey(u => new { u.Id });
            builder.HasIndex(e => new { e.Id, e.AssignerId, e.AssignedId });
            builder.Property(x => x.AssignerId).IsRequired();
            builder.Property(x => x.AssignedId).IsRequired();

            builder.HasOne(f => f.Assigner)
                .WithMany()
                .HasForeignKey(f => f.AssignerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Assigned)
                .WithMany()
                .HasForeignKey(f => f.AssignedId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}

