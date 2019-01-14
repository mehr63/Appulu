using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.News;

namespace Appulu.Data.Mapping.News
{
    /// <summary>
    /// Represents a news comment mapping configuration
    /// </summary>
    public partial class NewsCommentMap : AppEntityTypeConfiguration<NewsComment>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<NewsComment> builder)
        {
            builder.ToTable(nameof(NewsComment), Schemas.News);
            builder.HasKey(comment => comment.Id);

            builder.HasOne(comment => comment.NewsItem)
                .WithMany(news => news.NewsComments)
                .HasForeignKey(comment => comment.NewsItemId)
                .IsRequired();

            builder.HasOne(comment => comment.User)
                .WithMany()
                .HasForeignKey(comment => comment.UserId)
                .IsRequired();

            builder.HasOne(comment => comment.Store)
                .WithMany()
                .HasForeignKey(comment => comment.StoreId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}