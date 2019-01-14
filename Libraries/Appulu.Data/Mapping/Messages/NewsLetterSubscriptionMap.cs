using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Messages;

namespace Appulu.Data.Mapping.Messages
{
    /// <summary>
    /// Represents a newsLetter subscription mapping configuration
    /// </summary>
    public partial class NewsLetterSubscriptionMap : AppEntityTypeConfiguration<NewsLetterSubscription>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<NewsLetterSubscription> builder)
        {
            builder.ToTable(nameof(NewsLetterSubscription), Schemas.Messages);
            builder.HasKey(subscription => subscription.Id);

            builder.Property(subscription => subscription.Email).HasMaxLength(255).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}