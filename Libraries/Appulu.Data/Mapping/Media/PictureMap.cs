using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Media;

namespace Appulu.Data.Mapping.Media
{
    /// <summary>
    /// Represents a picture mapping configuration
    /// </summary>
    public partial class PictureMap : AppEntityTypeConfiguration<Picture>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.ToTable(nameof(Picture), Schemas.Media);
            builder.HasKey(picture => picture.Id);
            
            builder.Property(picture => picture.MimeType).HasMaxLength(40).IsRequired();
            builder.Property(picture => picture.SeoFilename).HasMaxLength(300);

            base.Configure(builder);
        }

        #endregion
    }
}