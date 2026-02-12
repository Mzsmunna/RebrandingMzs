using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.Base.Entities;
using Mzstruct.DB.EFCore.Helpers;
using System.Data;

namespace Mzstruct.DB.EFCore.Configs
{
    public class BaseUserEFConfig<TUser> : BaseEntityEFConfig<TUser> where TUser : BaseUser
    {
        public BaseUserEFConfig(string? tableName = "User") : base(tableName) { }

        public override void Configure(EntityTypeBuilder<TUser> builder)
        {
            //builder.HasKey(u => u.Id);
            //builder.Property(x => x.Id).ValueGeneratedNever();
            //builder.HasKey(u => new { u.Id, u.Email, u.Username });
            //builder.HasIndex(x => x.Email).IsUnique();
            //builder.HasIndex(x => x.Username).IsUnique();
            builder.HasIndex(e => new { e.Id, e.Email, e.Username }).IsUnique();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.Password).IsRequired();

            //var comparer = EFCoreHelper.VirtualListCompare<string>();
            //builder.Property(x => x.Roles)
            //.HasConversion(
            //    v => v == null ? null : string.Join(",", v),
            //    v => string.IsNullOrWhiteSpace(v)
            //        ? new List<string>()
            //        : v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            //            .ToList()
            //)
            //.Metadata.SetValueComparer(comparer);

            //builder.Property(x => x.RefreshToken).HasColumnType("text");
            
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
            //builder.Property(u => u.Password).HasColumnName(nameof(BaseUser.Password));
            
            //builder.Ignore(u => u.PasswordHash);
            //builder.Ignore(u => u.PasswordSalt);
            builder.Ignore(u => u.UserDetails);
            builder.Ignore(u => u.RefreshJWT);
            
            //builder.Ignore(u => u.Created);
            //builder.Ignore(u => u.Modified);

            //builder.HasOne(x => x.Address)
            //.WithOne(x => x.User)
            //.HasForeignKey<Address>(x => x.AddressId);

            //builder.HasData([new BaseUser
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
            //}]);

            base.Configure(builder);
        }
    }
}

