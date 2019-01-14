using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Forums;

namespace Appulu.Data.Mapping.Forums
{
    /// <summary>
    /// Represents a forum subscription mapping configuration
    /// </summary>
    public partial class ForumSubscriptionMap : AppEntityTypeConfiguration<ForumSubscription>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ForumSubscription> builder)
        {
            builder.ToTable(AppMappingDefaults.ForumsSubscriptionTable, Schemas.Forums);
            builder.HasKey(subscription => subscription.Id);

            builder.HasOne(subscription => subscription.User)
                .WithMany()
                .HasForeignKey(subscription => subscription.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }

        #endregion
    }
}