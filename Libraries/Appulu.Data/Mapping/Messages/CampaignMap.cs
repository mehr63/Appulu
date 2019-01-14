using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Messages;

namespace Appulu.Data.Mapping.Messages
{
    /// <summary>
    /// Represents a campaign mapping configuration
    /// </summary>
    public partial class CampaignMap : AppEntityTypeConfiguration<Campaign>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable(nameof(Campaign), Schemas.Messages);
            builder.HasKey(campaign => campaign.Id);

            builder.Property(campaign => campaign.Name).IsRequired();
            builder.Property(campaign => campaign.Subject).IsRequired();
            builder.Property(campaign => campaign.Body).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}