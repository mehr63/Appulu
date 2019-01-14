using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Users;

namespace Appulu.Data.Mapping.Users
{
    /// <summary>
    /// Represents a user-address mapping configuration
    /// </summary>
    public partial class UserAddressMap : AppEntityTypeConfiguration<UserAddressMapping>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UserAddressMapping> builder)
        {
            builder.ToTable(AppMappingDefaults.UserAddressesTable, Schemas.Users);
            builder.HasKey(mapping => new { mapping.UserId, mapping.AddressId });

            builder.Property(mapping => mapping.UserId).HasColumnName("User_Id");
            builder.Property(mapping => mapping.AddressId).HasColumnName("Address_Id");

            builder.HasOne(mapping => mapping.User)
                .WithMany(user => user.UserAddressMappings)
                .HasForeignKey(mapping => mapping.UserId)
                .IsRequired();

            builder.HasOne(mapping => mapping.Address)
                .WithMany()
                .HasForeignKey(mapping => mapping.AddressId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }

        #endregion
    }
}