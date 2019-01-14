using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Common;


namespace Appulu.Data.Mapping.Common
{
    /// <summary>
    /// Represents an address attribute mapping configuration
    /// </summary>
    public partial class AddressAttributeMap : AppEntityTypeConfiguration<AddressAttribute>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<AddressAttribute> builder)
        {
            builder.ToTable(nameof(AddressAttribute), Schemas.Common);
            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.Name).HasMaxLength(400).IsRequired();
            builder.Ignore(attribute => attribute.AttributeControlType);

            base.Configure(builder);
        }

        #endregion
    }
}