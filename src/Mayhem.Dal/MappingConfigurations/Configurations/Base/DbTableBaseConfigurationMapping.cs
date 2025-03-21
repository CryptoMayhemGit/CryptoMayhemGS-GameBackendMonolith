using Mayhem.Dal.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Base
{
    /// <summary>
    /// Base configuration, Uses ITableBase as a generic contract    
    /// </summary>
    public abstract class DbTableBaseConfigurationMapping<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, ITableBase
    {
        protected DbTableBaseConfigurationMapping() { }

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.CreationDate).HasColumnType("DATETIME");
            builder.Property(x => x.LastModificationDate).HasColumnType("DATETIME");
        }
    }
}
