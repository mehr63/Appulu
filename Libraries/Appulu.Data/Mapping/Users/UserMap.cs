using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Users;

namespace Appulu.Data.Mapping.Users
{
    /// <summary>
    /// Represents a user mapping configuration
    /// </summary>
    public partial class UserMap : AppEntityTypeConfiguration<User>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User), Schemas.Users);
            builder.HasKey(user => user.Id);

            builder.Property(user => user.Username).HasMaxLength(1000);
            builder.Property(user => user.Email).HasMaxLength(1000);
            builder.Property(user => user.EmailToRevalidate).HasMaxLength(1000);
            builder.Property(user => user.SystemName).HasMaxLength(400);

            builder.Property(user => user.BillingAddressId).HasColumnName("BillingAddress_Id");
            builder.Property(user => user.ShippingAddressId).HasColumnName("ShippingAddress_Id");

            builder.HasOne(user => user.BillingAddress)
                .WithMany()
                .HasForeignKey(user => user.BillingAddressId);

            builder.HasOne(user => user.ShippingAddress)
                .WithMany()
                .HasForeignKey(user => user.ShippingAddressId);

            builder.Ignore(user => user.UserRoles);
            builder.Ignore(user => user.Addresses);

            base.Configure(builder);
        }

        #endregion
    }
}