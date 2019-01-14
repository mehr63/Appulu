using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Users;

namespace Appulu.Data.Mapping.Users
{
    /// <summary>
    /// Represents an external authentication record mapping configuration
    /// </summary>
    public partial class ExternalAuthenticationRecordMap : AppEntityTypeConfiguration<ExternalAuthenticationRecord>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ExternalAuthenticationRecord> builder)
        {
            builder.ToTable(nameof(ExternalAuthenticationRecord), Schemas.Users);
            builder.HasKey(record => record.Id);

            builder.HasOne(record => record.User)
                .WithMany(user => user.ExternalAuthenticationRecords)
                .HasForeignKey(record => record.UserId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}