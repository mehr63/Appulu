using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Discounts;

namespace Appulu.Data.Mapping.Discounts
{
    /// <summary>
    /// Represents a discount requirement mapping configuration
    /// </summary>
    public partial class DiscountRequirementMap : AppEntityTypeConfiguration<DiscountRequirement>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<DiscountRequirement> builder)
        {
            builder.ToTable(nameof(DiscountRequirement), Schemas.Discounts);
            builder.HasKey(requirement => requirement.Id);

            builder.HasMany(requirement => requirement.ChildRequirements)
                .WithOne()
                .HasForeignKey(requirement => requirement.ParentId);

            builder.Ignore(requirement => requirement.InteractionType);

            base.Configure(builder);
        }

        #endregion
    }
}