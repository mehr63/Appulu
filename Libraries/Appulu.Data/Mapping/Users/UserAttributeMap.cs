using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Users;

namespace Appulu.Data.Mapping.Users
{
    /// <summary>
    /// Represents a user attribute mapping configuration
    /// </summary>
    public partial class UserAttributeMap : AppEntityTypeConfiguration<UserAttribute>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UserAttribute> builder)
        {
            builder.ToTable(nameof(UserAttribute), Schemas.Users);
            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.Name).HasMaxLength(400).IsRequired();

            builder.Ignore(attribute => attribute.AttributeControlType);

            base.Configure(builder);
        }

        #endregion
    }
}