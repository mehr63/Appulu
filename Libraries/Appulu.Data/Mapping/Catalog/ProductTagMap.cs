using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Catalog;

namespace Appulu.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a product tag mapping configuration
    /// </summary>
    public partial class ProductTagMap : AppEntityTypeConfiguration<ProductTag>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.ToTable(nameof(ProductTag), Schemas.Catalog);
            builder.HasKey(productTag => productTag.Id);

            builder.Property(productTag => productTag.Name).HasMaxLength(400).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}