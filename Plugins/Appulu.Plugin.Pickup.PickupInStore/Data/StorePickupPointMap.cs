using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Data.Mapping;
using Appulu.Plugin.Pickup.PickupInStore.Domain;

namespace Appulu.Plugin.Pickup.PickupInStore.Data
{
    /// <summary>
    /// Represents a store pickup point mapping configuration
    /// </summary>
    public partial class StorePickupPointMap : AppEntityTypeConfiguration<StorePickupPoint>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<StorePickupPoint> builder)
        {
            builder.ToTable(nameof(StorePickupPoint));
            builder.HasKey(point => point.Id);

            builder.Property(point => point.PickupFee).HasColumnType("decimal(18, 4)");
        }

        #endregion
    }
}