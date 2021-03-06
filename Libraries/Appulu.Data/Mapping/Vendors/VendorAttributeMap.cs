using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Vendors;

namespace Appulu.Data.Mapping.Vendors
{
    /// <summary>
    /// Represents an vendor attribute mapping configuration
    /// </summary>
    public partial class VendorAttributeMap : AppEntityTypeConfiguration<VendorAttribute>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<VendorAttribute> builder)
        {
            builder.ToTable(nameof(VendorAttribute));
            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.Name).HasMaxLength(400).IsRequired();

            builder.Ignore(attribute => attribute.AttributeControlType);

            base.Configure(builder);
        }

        #endregion
    }
}