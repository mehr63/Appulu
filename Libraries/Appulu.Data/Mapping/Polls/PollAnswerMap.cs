using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Polls;

namespace Appulu.Data.Mapping.Polls
{
    /// <summary>
    /// Represents a poll answer mapping configuration
    /// </summary>
    public partial class PollAnswerMap : AppEntityTypeConfiguration<PollAnswer>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PollAnswer> builder)
        {
            builder.ToTable(nameof(PollAnswer), Schemas.Polls);
            builder.HasKey(pollAnswer => pollAnswer.Id);

            builder.Property(pollAnswer => pollAnswer.Name).IsRequired();

            builder.HasOne(pollAnswer => pollAnswer.Poll)
                .WithMany(poll => poll.PollAnswers)
                .HasForeignKey(pollAnswer => pollAnswer.PollId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}