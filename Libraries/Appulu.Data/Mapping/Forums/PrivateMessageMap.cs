using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Forums;

namespace Appulu.Data.Mapping.Forums
{
    /// <summary>
    /// Represents a private message mapping configuration
    /// </summary>
    public partial class PrivateMessageMap : AppEntityTypeConfiguration<PrivateMessage>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PrivateMessage> builder)
        {
            builder.ToTable(AppMappingDefaults.PrivateMessageTable, Schemas.Forums);
            builder.HasKey(message => message.Id);

            builder.Property(message => message.Subject).HasMaxLength(450).IsRequired();
            builder.Property(message => message.Text).IsRequired();

            builder.HasOne(message => message.FromUser)
               .WithMany()
               .HasForeignKey(message => message.FromUserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(message => message.ToUser)
               .WithMany()
               .HasForeignKey(message => message.ToUserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }

        #endregion
    }
}