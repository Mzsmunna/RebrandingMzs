using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.DB.EFCore.Configs;
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

            builder.HasMany(f => f.Issues)
                .WithOne(f => f.Assigner)
                .HasForeignKey(f => f.AssignerId);

            builder.HasMany(f => f.Issues)
                .WithOne(f => f.Assigned)
                .HasForeignKey(f => f.AssignedId);
        }
    }
}

