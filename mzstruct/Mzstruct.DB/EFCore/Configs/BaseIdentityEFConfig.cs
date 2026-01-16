using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.DB.EFCore.Entities;

namespace Mzstruct.DB.EFCore.Configs
{
    public class BaseIdentityEFConfig<TUser> : IEntityTypeConfiguration<TUser> where TUser : UserIdentity
    {
        //public BaseIdentityUserEFConfig(string? tableName = "User") { }

        public virtual void Configure(EntityTypeBuilder<TUser> builder)
        {
            //builder.ToTable(tableName ?? typeof(TUser).Name);
            //builder.HasIndex(e => new { e.Id, e.Email }).IsUnique();
            //builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            //builder.Property(x => x.Email).IsRequired();
       
            builder.Property(e => e.CreatedAt)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            builder.Property(e => e.ModifiedAt)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : v,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            //builder.Property(x => x.RefreshToken).HasColumnType("text");
            //builder.Property(x => x.Img).HasColumnType("text");
            //builder.Property(x => x.Name).HasColumnType("varchar(50)").IsRequired();
            //builder.Property(u => u.Email).HasColumnName(nameof(BaseUser.Email));
            //builder.Property(u => u.Password).HasColumnName(nameof(BaseUser.Password));
            //builder.Ignore(u => u.PasswordHash);
            //builder.Ignore(u => u.PasswordSalt);
            //builder.Ignore(u => u.TokenCreated);
            //builder.Ignore(u => u.TokenExpires);
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
        }
    }
}

