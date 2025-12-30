using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.Base.Entities;

namespace Mzstruct.DB.ORM.EFCore.Configs
{
    public abstract class BaseEntityEFConfig<T>(string? tableName) : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(tableName ?? typeof(T).Name);
            builder.HasKey(u => u.Id);
        }
    }
}
