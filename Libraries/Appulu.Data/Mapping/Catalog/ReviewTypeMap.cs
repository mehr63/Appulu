using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Catalog;


namespace Appulu.Data.Mapping.Catalog
{
    /// <summary>
    /// Represent a review type mapping class
    /// </summary>
    public partial class ReviewTypeMap : AppEntityTypeConfiguration<ReviewType>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ReviewType> builder)
        {
            builder.ToTable(nameof(ReviewType), Schemas.Catalog);
            builder.HasKey(reviewType => reviewType.Id);

            builder.Property(reviewType => reviewType.Name).IsRequired().HasMaxLength(400);
            builder.Property(reviewType => reviewType.Description).IsRequired().HasMaxLength(400);            

            base.Configure(builder);
        }

        #endregion
    }
}
