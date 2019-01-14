using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Users;

namespace Appulu.Data.Mapping.Users
{
    /// <summary>
    /// Represents a user attribute value mapping configuration
    /// </summary>
    public partial class UserAttributeValueMap : AppEntityTypeConfiguration<UserAttributeValue>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UserAttributeValue> builder)
        {
            builder.ToTable(nameof(UserAttributeValue), Schemas.Users);
            builder.HasKey(value => value.Id);

            builder.Property(value => value.Name).HasMaxLength(400).IsRequired();

            builder.HasOne(value => value.UserAttribute)
                .WithMany(attribute => attribute.UserAttributeValues)
                .HasForeignKey(value => value.UserAttributeId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}