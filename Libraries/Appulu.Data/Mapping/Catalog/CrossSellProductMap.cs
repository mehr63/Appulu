using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Catalog;

namespace Appulu.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a cross-sell product mapping configuration
    /// </summary>
    public partial class CrossSellProductMap : AppEntityTypeConfiguration<CrossSellProduct>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CrossSellProduct> builder)
        {
            builder.ToTable(nameof(CrossSellProduct), Schemas.Catalog);
            builder.HasKey(product => product.Id);

            base.Configure(builder);
        }

        #endregion
    }
}