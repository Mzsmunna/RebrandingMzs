using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.Base.Entities;

namespace Mzstruct.DB.EFCore.Configs
{
    public abstract class BaseEntityEFConfig<T>(string? tableName) : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        //public BaseEntityEFConfig(string? tableName)
        //{
        //    ConfigureDefaults();
        //}

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(tableName ?? typeof(T).Name);
            builder.HasKey(u => u.Id);
            //builder.Property(x => x.Status).HasConversion<string>(); // Enum to string conversion
        }

        public void CustomConfigure(EntityTypeBuilder<T> builder, string schemeName = "dbo", string? triggerName = null)
        {
            builder.ToTable(typeof(T).Name, schemeName, x =>
            {
                if (!string.IsNullOrWhiteSpace(triggerName) && !string.IsNullOrEmpty(triggerName)) x.HasTrigger(triggerName);
            });
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn();
        }
    }
}
