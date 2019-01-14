using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Forums;

namespace Appulu.Data.Mapping.Forums
{
    /// <summary>
    /// Represents a forum post mapping configuration
    /// </summary>
    public partial class ForumPostMap : AppEntityTypeConfiguration<ForumPost>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ForumPost> builder)
        {
            builder.ToTable(AppMappingDefaults.ForumsPostTable, Schemas.Forums);
            builder.HasKey(post => post.Id);

            builder.Property(post => post.Text).IsRequired();
            builder.Property(post => post.IPAddress).HasMaxLength(100);

            builder.HasOne(post => post.ForumTopic)
                .WithMany()
                .HasForeignKey(post => post.TopicId)
                .IsRequired();

            builder.HasOne(post => post.User)
               .WithMany()
               .HasForeignKey(post => post.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }

        #endregion
    }
}