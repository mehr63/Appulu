using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Orders;

namespace Appulu.Data.Mapping.Orders
{
    /// <summary>
    /// Represents a gift card usage history mapping configuration
    /// </summary>
    public partial class GiftCardUsageHistoryMap : AppEntityTypeConfiguration<GiftCardUsageHistory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<GiftCardUsageHistory> builder)
        {
            builder.ToTable(nameof(GiftCardUsageHistory), Schemas.Orders);
            builder.HasKey(historyEntry => historyEntry.Id);

            builder.Property(historyEntry => historyEntry.UsedValue).HasColumnType("decimal(18, 4)");

            builder.HasOne(historyEntry => historyEntry.GiftCard)
                .WithMany(giftCard => giftCard.GiftCardUsageHistory)
                .HasForeignKey(historyEntry => historyEntry.GiftCardId)
                .IsRequired();

            builder.HasOne(historyEntry => historyEntry.UsedWithOrder)
                .WithMany(order => order.GiftCardUsageHistory)
                .HasForeignKey(historyEntry => historyEntry.UsedWithOrderId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}