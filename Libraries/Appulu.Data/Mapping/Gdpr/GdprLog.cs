using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Gdpr;

namespace Appulu.Data.Mapping.Gdpr
{
    /// <summary>
    /// Represents a GDPR log mapping configuration
    /// </summary>
    public partial class GdprLogMap : AppEntityTypeConfiguration<GdprLog>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<GdprLog> builder)
        {
            builder.ToTable(nameof(GdprLog), Schemas.Gdpr);
            builder.HasKey(gdpr => gdpr.Id);

            builder.Ignore(gdpr => gdpr.RequestType);

            base.Configure(builder);
        }

        #endregion
    }
}