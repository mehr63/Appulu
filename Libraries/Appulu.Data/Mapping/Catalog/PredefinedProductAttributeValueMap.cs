using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Catalog;

namespace Appulu.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a predefined product attribute value mapping configuration
    /// </summary>
    public partial class PredefinedProductAttributeValueMap : AppEntityTypeConfiguration<PredefinedProductAttributeValue>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PredefinedProductAttributeValue> builder)
        {
            builder.ToTable(nameof(PredefinedProductAttributeValue), Schemas.Catalog);
            builder.HasKey(value => value.Id);

            builder.Property(value => value.Name).HasMaxLength(400).IsRequired();
            builder.Property(value => value.PriceAdjustment).HasColumnType("decimal(18, 4)");
            builder.Property(value => value.WeightAdjustment).HasColumnType("decimal(18, 4)");
            builder.Property(value => value.Cost).HasColumnType("decimal(18, 4)");

            builder.HasOne(value => value.ProductAttribute)
                .WithMany()
                .HasForeignKey(value => value.ProductAttributeId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}