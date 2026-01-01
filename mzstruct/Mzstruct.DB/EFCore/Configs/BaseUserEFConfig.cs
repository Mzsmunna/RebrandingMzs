using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.Base.Entities;

namespace Mzstruct.DB.EFCore.Configs
{
    public class BaseUserEFConfig : BaseEntityEFConfig<BaseUser>
    {
        public BaseUserEFConfig(string? tableName = "User") : base(tableName) { }

        public override void Configure(EntityTypeBuilder<BaseUser> builder)
        {
            builder.HasKey(u => u.Id);
            //builder.HasKey(u => new { u.Id, u.Email, u.Username });
            //builder.HasIndex(x => x.Email).IsUnique();
            //builder.HasIndex(x => x.Username).IsUnique();
            builder.HasIndex(e => new { e.Email, e.Username }).IsUnique();
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Roles).HasConversion(
                c => string.Join(',', c ?? new List<string>()),
                c => c.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            builder.Property(x => x.RefreshToken).HasColumnType("text");
            //builder.Property(e => e.CreatedAt).HasConversion(v => v.ToUniversalTime(),v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            //builder.Property(e => e.UpdatedAt).HasConversion(v => v.ToUniversalTime(),v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            //builder.Property(x => x.Img).HasColumnType("text");
            //builder.Property(x => x.Name).HasColumnType("varchar(50)").IsRequired();
            //builder.Property(u => u.Email).HasColumnName(nameof(BaseUser.Email));
            //builder.Property(u => u.Password).HasColumnName(nameof(BaseUser.Password));
            builder.Ignore(u => u.PasswordHash);
            builder.Ignore(u => u.PasswordSalt);
            builder.Ignore(u => u.TokenCreated);
            builder.Ignore(u => u.TokenExpires);
            builder.Ignore(u => u.Created);
            builder.Ignore(u => u.Modified);

            //builder.HasOne(x => x.Address)
            //.WithOne(x => x.User)
            //.HasForeignKey<Address>(x => x.AddressId);

            //builder.HasData(new BaseUser
            //{
            //    Id = Guid.CreateVersion7().ToString(),
            //    Name = "Mzs Munna",
            //    Email = "mzs.munna@gmail.com",
            //    Username = "mzsmunna",
            //    Password = "P@ssw0rd123",
            //    Role = "Admin",
            //},
            //new BaseUser
            //{
            //    Id = Guid.CreateVersion7().ToString(),
            //    Name = "Mamunuz Zaman",
            //    Email = "mzaman@insightintechnology.com",
            //    Username = "mzaman",
            //    Password = "P@ssw0rd321",
            //    Role = "User",
            //});
        }
    }
}

