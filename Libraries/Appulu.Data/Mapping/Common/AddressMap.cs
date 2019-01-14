using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Common;

namespace Appulu.Data.Mapping.Common
{
    /// <summary>
    /// Represents an address mapping configuration
    /// </summary>
    public partial class AddressMap : AppEntityTypeConfiguration<Address>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable(nameof(Address), Schemas.Common);
            builder.HasKey(address => address.Id);

            builder.HasOne(address => address.Country)
                .WithMany()
                .HasForeignKey(address => address.CountryId);

            builder.HasOne(address => address.StateProvince)
                .WithMany()
                .HasForeignKey(address => address.StateProvinceId);

            base.Configure(builder);
        }

        #endregion
    }
}