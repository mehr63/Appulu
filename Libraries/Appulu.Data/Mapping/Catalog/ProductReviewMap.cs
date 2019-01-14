using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Catalog;

namespace Appulu.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a product review mapping configuration
    /// </summary>
    public partial class ProductReviewMap : AppEntityTypeConfiguration<ProductReview>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            builder.ToTable(nameof(ProductReview), Schemas.Catalog);
            builder.HasKey(productReview => productReview.Id);

            builder.HasOne(productReview => productReview.Product)
                .WithMany(product => product.ProductReviews)
                .HasForeignKey(productReview => productReview.ProductId)
                .IsRequired();

            builder.HasOne(productReview => productReview.User)
                .WithMany()
                .HasForeignKey(productReview => productReview.UserId)
                .IsRequired();

            builder.HasOne(productReview => productReview.Store)
                .WithMany()
                .HasForeignKey(productReview => productReview.StoreId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}