using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Shipping;

namespace Appulu.Data.Mapping.Shipping
{
    /// <summary>
    /// Represents a shipping method mapping configuration
    /// </summary>
    public partial class ShippingMethodMap : AppEntityTypeConfiguration<ShippingMethod>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ShippingMethod> builder)
        {
            builder.ToTable(nameof(ShippingMethod), Schemas.Shipping);
            builder.HasKey(method => method.Id);

            builder.Property(method => method.Name).HasMaxLength(400).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}