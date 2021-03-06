using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Discounts;

namespace Appulu.Data.Mapping.Discounts
{
    /// <summary>
    /// Represents a discount usage history mapping configuration
    /// </summary>
    public partial class DiscountUsageHistoryMap : AppEntityTypeConfiguration<DiscountUsageHistory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<DiscountUsageHistory> builder)
        {
            builder.ToTable(nameof(DiscountUsageHistory), Schemas.Discounts);
            builder.HasKey(historyEntry => historyEntry.Id);

            builder.HasOne(historyEntry => historyEntry.Discount)
                .WithMany()
                .HasForeignKey(historyEntry => historyEntry.DiscountId)
                .IsRequired();

            builder.HasOne(historyEntry => historyEntry.Order)
                .WithMany(order => order.DiscountUsageHistory)
                .HasForeignKey(historyEntry => historyEntry.OrderId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}