using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Catalog;

namespace Appulu.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a stock quantity history mapping configuration
    /// </summary>
    public partial class StockQuantityHistoryMap : AppEntityTypeConfiguration<StockQuantityHistory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<StockQuantityHistory> builder)
        {
            builder.ToTable(nameof(StockQuantityHistory), Schemas.Catalog);
            builder.HasKey(historyEntry => historyEntry.Id);

            builder.HasOne(historyEntry => historyEntry.Product)
                .WithMany()
                .HasForeignKey(historyEntry => historyEntry.ProductId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}