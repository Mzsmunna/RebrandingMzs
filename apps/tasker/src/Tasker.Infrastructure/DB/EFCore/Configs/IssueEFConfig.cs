using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.Base.Entities;
using Mzstruct.DB.ORM.EFCore.Configs;
using Tasker.Application.Features.Issues;

namespace Tasker.Infrastructure.DB.EFCore.Configs
{
    public class IssueEFConfig : BaseEntityEFConfig<Issue>
    {
        public IssueEFConfig(string? tableName = "User") : base(tableName) { }

        public override void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.HasKey(u => new { u.Id, u.AssignerId, u.AssignedId });

            builder.HasOne(f => f.Assigner)
                .WithMany()
                .HasForeignKey(f => f.AssignerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Assigned)
                .WithMany()
                .HasForeignKey(f => f.AssignedId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

