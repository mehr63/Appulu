using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Catalog;

namespace Appulu.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a tier price mapping configuration
    /// </summary>
    public partial class TierPriceMap : AppEntityTypeConfiguration<TierPrice>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<TierPrice> builder)
        {
            builder.ToTable(nameof(TierPrice), Schemas.Catalog);
            builder.HasKey(price => price.Id);

            builder.Property(price => price.Price).HasColumnType("decimal(18, 4)");

            builder.HasOne(price => price.Product)
                .WithMany(product => product.TierPrices)
                .HasForeignKey(price => price.ProductId)
                .IsRequired();

            builder.HasOne(price => price.UserRole)
                .WithMany()
                .HasForeignKey(price => price.UserRoleId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }

        #endregion
    }
}