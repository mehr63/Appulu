using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Affiliates;

namespace Appulu.Data.Mapping.Affiliates
{
    /// <summary>
    /// Represents an affiliate mapping configuration
    /// </summary>
    public partial class AffiliateMap : AppEntityTypeConfiguration<Affiliate>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Affiliate> builder)
        {
            builder.ToTable(nameof(Affiliate), Schemas.Affiliates);
            builder.HasKey(affiliate => affiliate.Id);

            builder.HasOne(affiliate => affiliate.Address)
                .WithMany()
                .HasForeignKey(affiliate => affiliate.AddressId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }

        #endregion
    }
}