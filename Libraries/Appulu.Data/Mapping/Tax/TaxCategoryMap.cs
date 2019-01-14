using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Tax;

namespace Appulu.Data.Mapping.Tax
{
    /// <summary>
    /// Represents a tax category mapping configuration
    /// </summary>
    public partial class TaxCategoryMap : AppEntityTypeConfiguration<TaxCategory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<TaxCategory> builder)
        {
            builder.ToTable(nameof(TaxCategory), Schemas.Tax);
            builder.HasKey(taxCategory => taxCategory.Id);

            builder.Property(taxCategory => taxCategory.Name).HasMaxLength(400).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}