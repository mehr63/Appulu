using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Seo;

namespace Appulu.Data.Mapping.Seo
{
    /// <summary>
    /// Represents an URL record mapping configuration
    /// </summary>
    public partial class UrlRecordMap : AppEntityTypeConfiguration<UrlRecord>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UrlRecord> builder)
        {
            builder.ToTable(nameof(UrlRecord), Schemas.Seo);
            builder.HasKey(record => record.Id);

            builder.Property(record => record.EntityName).HasMaxLength(400).IsRequired();
            builder.Property(record => record.Slug).IsRequired().HasMaxLength(400);

            base.Configure(builder);
        }

        #endregion
    }
}