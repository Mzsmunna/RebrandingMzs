using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.DB.EFCore.Configs;
using Tasker.Application.Features.Users;

namespace Tasker.Infrastructure.DB.EFCore.Configs
{
    public class UserEFConfig : BaseUserEFConfig<User> //BaseEntityEFConfig<User>
    {
        public UserEFConfig(string? tableName = "User") : base(tableName) { }

        public override void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.HasKey(u => u.Id);
            //builder.Property(x => x.Id).ValueGeneratedNever();
            //builder.HasKey(u => new { u.Id, u.Email, u.Username });
            //builder.HasIndex(e => new { e.Id, e.Email, e.Username }).IsUnique();

            builder.HasMany(f => f.AssignerIssues)
                .WithOne(f => f.Assigner)
                .HasForeignKey(f => f.AssignerId);

            builder.HasMany(f => f.AssignedIssues)
                .WithOne(f => f.Assigned)
                .HasForeignKey(f => f.AssignedId);

            base.Configure(builder);
        }
    }
}

