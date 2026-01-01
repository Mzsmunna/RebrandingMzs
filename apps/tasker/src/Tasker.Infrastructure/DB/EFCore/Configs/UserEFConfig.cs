using Microsoft.EntityFrameworkCore;
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
            //builder.HasKey(u => u.Id);
            //builder.Property(x => x.Id).ValueGeneratedNever();
            //builder.HasKey(u => new { u.Id, u.Email, u.Username });
            builder.HasIndex(e => new { e.Id, e.Email, e.Username }).IsUnique();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Roles).HasConversion(
                c => string.Join(',', c ?? new List<string>()),
                c => c.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            builder.Property(x => x.RefreshToken).HasColumnType("text");
            builder.Property(e => e.CreatedAt)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            builder.Property(e => e.ModifiedAt)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : v,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
            //builder.Property(x => x.Img).HasColumnType("text");
            //builder.Property(x => x.Name).HasColumnType("varchar(50)").IsRequired();
            //builder.Property(u => u.Email).HasColumnName(nameof(BaseUser.Email));
            builder.Ignore(u => u.PasswordHash);
            builder.Ignore(u => u.PasswordSalt);
            builder.Ignore(u => u.TokenCreated);
            builder.Ignore(u => u.TokenExpires);       

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

