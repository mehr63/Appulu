using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Orders;

namespace Appulu.Data.Mapping.Orders
{
    /// <summary>
    /// Represents a shopping cart item mapping configuration
    /// </summary>
    public partial class ShoppingCartItemMap : AppEntityTypeConfiguration<ShoppingCartItem>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.ToTable(nameof(ShoppingCartItem), Schemas.Orders);
            builder.HasKey(item => item.Id);

            builder.Property(item => item.UserEnteredPrice).HasColumnType("decimal(18, 4)");

            builder.HasOne(item => item.User)
                .WithMany(user => user.ShoppingCartItems)
                .HasForeignKey(item => item.UserId)
                .IsRequired();

            builder.HasOne(item => item.Product)
                .WithMany()
                .HasForeignKey(item => item.ProductId)
                .IsRequired();

            builder.Ignore(item => item.ShoppingCartType);

            base.Configure(builder);
        }

        #endregion
    }
}