using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Security;

namespace Appulu.Data.Mapping.Security
{
    /// <summary>
    /// Represents an ACL record mapping configuration
    /// </summary>
    public partial class AclRecordMap : AppEntityTypeConfiguration<AclRecord>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<AclRecord> builder)
        {
            builder.ToTable(nameof(AclRecord), Schemas.Security);
            builder.HasKey(record => record.Id);

            builder.Property(record => record.EntityName).HasMaxLength(400).IsRequired();

            builder.HasOne(record => record.UserRole)
                .WithMany()
                .HasForeignKey(record => record.UserRoleId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}