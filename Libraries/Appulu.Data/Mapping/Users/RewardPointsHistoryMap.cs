using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Orders;

namespace Appulu.Data.Mapping.Users
{
    /// <summary>
    /// Represents a reward points history mapping configuration
    /// </summary>
    public partial class RewardPointsHistoryMap : AppEntityTypeConfiguration<RewardPointsHistory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<RewardPointsHistory> builder)
        {
            builder.ToTable(nameof(RewardPointsHistory), Schemas.Users);
            builder.HasKey(historyEntry => historyEntry.Id);

            builder.Property(historyEntry => historyEntry.UsedAmount).HasColumnType("decimal(18, 4)");

            builder.HasOne(historyEntry => historyEntry.User)
                .WithMany()
                .HasForeignKey(historyEntry => historyEntry.UserId)
                .IsRequired();

            builder.HasOne(historyEntry => historyEntry.UsedWithOrder)
                .WithOne(order => order.RedeemedRewardPointsEntry)
                .HasForeignKey<Order>(order => order.RewardPointsHistoryEntryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.Configure(builder);
        }

        #endregion
    }
}