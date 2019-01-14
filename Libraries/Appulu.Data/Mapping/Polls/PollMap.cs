using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Polls;

namespace Appulu.Data.Mapping.Polls
{
    /// <summary>
    /// Represents a poll mapping configuration
    /// </summary>
    public partial class PollMap : AppEntityTypeConfiguration<Poll>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.ToTable(nameof(Poll), Schemas.Polls);
            builder.HasKey(poll => poll.Id);

            builder.Property(poll => poll.Name).IsRequired();

            builder.HasOne(poll => poll.Language)
                .WithMany()
                .HasForeignKey(poll => poll.LanguageId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}