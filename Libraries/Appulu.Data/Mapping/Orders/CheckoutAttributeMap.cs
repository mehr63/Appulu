using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Orders;

namespace Appulu.Data.Mapping.Orders
{
    /// <summary>
    /// Represents a checkout attribute mapping configuration
    /// </summary>
    public partial class CheckoutAttributeMap : AppEntityTypeConfiguration<CheckoutAttribute>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CheckoutAttribute> builder)
        {
            builder.ToTable(nameof(CheckoutAttribute), Schemas.Orders);
            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.Name).HasMaxLength(400).IsRequired();

            builder.Ignore(attribute => attribute.AttributeControlType);

            base.Configure(builder);
        }

        #endregion
    }
}