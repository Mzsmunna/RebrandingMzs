using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.DB.ORM.EFCore.Configs;
using Tasker.Application.Features.Users;

namespace Tasker.Infrastructure.DB.EFCore.Configs
{
    public class UserEFConfig : BaseEntityEFConfig<User>
    {
        public UserEFConfig(string? tableName = "User") : base(tableName) { }

        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => new { u.Id, u.Name, u.Email, u.Password });
            //builder.HasKey(u => u.Id);
            //builder.Property(u => u.Name).HasColumnName(nameof(User.Name));
            //builder.Property(u => u.Email).HasColumnName(nameof(User.Email));
            //builder.Property(u => u.Password).HasColumnName(nameof(User.Password));
            builder.Ignore(u => u.PasswordHash);
            builder.Ignore(u => u.PasswordSalt);
            builder.Ignore(u => u.TokenCreated);
            builder.Ignore(u => u.TokenExpires);

            builder.HasMany(x => x.Issues)
                .WithOne(x => x.Assigner)
                .HasForeignKey(x => x.AssignerId);

            builder.HasMany(x => x.Issues)
                .WithOne(x => x.Assigned)
                .HasForeignKey(x => x.AssignedId);
        }
    }
}

