using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Blogs;

namespace Appulu.Data.Mapping.Blogs
{
    /// <summary>
    /// Represents a blog comment mapping configuration
    /// </summary>
    public partial class BlogCommentMap : AppEntityTypeConfiguration<BlogComment>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<BlogComment> builder)
        {
            builder.ToTable(nameof(BlogComment), Schemas.Blogs);
            builder.HasKey(comment => comment.Id);

            builder.HasOne(comment => comment.BlogPost)
                .WithMany(blog => blog.BlogComments)
                .HasForeignKey(comment => comment.BlogPostId)
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