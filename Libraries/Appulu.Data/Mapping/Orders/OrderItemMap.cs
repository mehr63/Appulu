using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Orders;

namespace Appulu.Data.Mapping.Orders
{
    /// <summary>
    /// Represents an order item mapping configuration
    /// </summary>
    public partial class OrderItemMap : AppEntityTypeConfiguration<OrderItem>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable(nameof(OrderItem), Schemas.Orders);
            builder.HasKey(orderItem => orderItem.Id);

            builder.Property(orderItem => orderItem.UnitPriceInclTax).HasColumnType("decimal(18, 4)");
            builder.Property(orderItem => orderItem.UnitPriceExclTax).HasColumnType("decimal(18, 4)");
            builder.Property(orderItem => orderItem.PriceInclTax).HasColumnType("decimal(18, 4)");
            builder.Property(orderItem => orderItem.PriceExclTax).HasColumnType("decimal(18, 4)");
            builder.Property(orderItem => orderItem.DiscountAmountInclTax).HasColumnType("decimal(18, 4)");
            builder.Property(orderItem => orderItem.DiscountAmountExclTax).HasColumnType("decimal(18, 4)");
            builder.Property(orderItem => orderItem.OriginalProductCost).HasColumnType("decimal(18, 4)");
            builder.Property(orderItem => orderItem.ItemWeight).HasColumnType("decimal(18, 4)");

            builder.HasOne(orderItem => orderItem.Order)
                .WithMany(order => order.OrderItems)
                .HasForeignKey(orderItem => orderItem.OrderId)
                .IsRequired();

            builder.HasOne(orderItem => orderItem.Product)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.ProductId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}