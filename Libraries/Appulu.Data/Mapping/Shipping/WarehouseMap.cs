using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Shipping;

namespace Appulu.Data.Mapping.Shipping
{
    /// <summary>
    /// Represents a warehouse mapping configuration
    /// </summary>
    public partial class WarehouseMap : AppEntityTypeConfiguration<Warehouse>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable(nameof(Warehouse), Schemas.Shipping);
            builder.HasKey(warehouse => warehouse.Id);

            builder.Property(warehouse => warehouse.Name).HasMaxLength(400).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}