using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Catalog;

namespace Appulu.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a product warehouse inventory mapping configuration
    /// </summary>
    public partial class ProductWarehouseInventoryMap : AppEntityTypeConfiguration<ProductWarehouseInventory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProductWarehouseInventory> builder)
        {
            builder.ToTable(nameof(ProductWarehouseInventory), Schemas.Catalog);
            builder.HasKey(productWarehouseInventory => productWarehouseInventory.Id);

            builder.HasOne(productWarehouseInventory => productWarehouseInventory.Product)
                .WithMany(product => product.ProductWarehouseInventory)
                .HasForeignKey(productWarehouseInventory => productWarehouseInventory.ProductId)
                .IsRequired();

            builder.HasOne(productWarehouseInventory => productWarehouseInventory.Warehouse)
                .WithMany()
                .HasForeignKey(productWarehouseInventory => productWarehouseInventory.WarehouseId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}