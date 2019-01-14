using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Common;

namespace Appulu.Data.Mapping.Common
{
    /// <summary>
    /// Represents a generic attribute mapping configuration
    /// </summary>
    public partial class GenericAttributeMap : AppEntityTypeConfiguration<GenericAttribute>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<GenericAttribute> builder)
        {
            builder.ToTable(nameof(GenericAttribute), Schemas.Common);
            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.KeyGroup).HasMaxLength(400).IsRequired();
            builder.Property(attribute => attribute.Key).HasMaxLength(400).IsRequired();
            builder.Property(attribute => attribute.Value).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}