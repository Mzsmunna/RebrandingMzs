using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.Base.Entities;


namespace Mzstruct.DB.ORM.EFCore.Configs
{
    public class BaseUserEFConfig : BaseEntityEFConfig<BaseUser>
    {
        public override void Configure(EntityTypeBuilder<BaseUser> builder)
        {
            builder.HasKey(u => u.Id);
            //builder.Property(u => u.Name).HasColumnName(nameof(BaseUser.Name));
            //builder.Property(u => u.Email).HasColumnName(nameof(BaseUser.Email));
            //builder.Property(u => u.Password).HasColumnName(nameof(BaseUser.Password));
            builder.Ignore(u => u.PasswordHash);
            builder.Ignore(u => u.PasswordSalt);
            builder.Ignore(u => u.TokenCreated);
            builder.Ignore(u => u.TokenExpires);
        }
    }
}

