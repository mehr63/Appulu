using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Media;

namespace Appulu.Data.Mapping.Media
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class PictureBinaryMap : AppEntityTypeConfiguration<PictureBinary>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PictureBinary> builder)
        {
            builder.ToTable(nameof(PictureBinary), Schemas.Media);
            builder.HasKey(pictureBinary => pictureBinary.Id);
            
            builder.HasOne(pictureBinary => pictureBinary.Picture)
                .WithOne(picture => picture.PictureBinary)
                .HasForeignKey<PictureBinary>(pictureBinary => pictureBinary.PictureId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }

        #endregion
    }
}