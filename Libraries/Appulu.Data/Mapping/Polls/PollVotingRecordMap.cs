using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Polls;

namespace Appulu.Data.Mapping.Polls
{
    /// <summary>
    /// Represents a poll voting record mapping configuration
    /// </summary>
    public partial class PollVotingRecordMap : AppEntityTypeConfiguration<PollVotingRecord>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PollVotingRecord> builder)
        {
            builder.ToTable(nameof(PollVotingRecord), Schemas.Polls);
            builder.HasKey(record => record.Id);

            builder.HasOne(record => record.PollAnswer)
                .WithMany(pollAnswer => pollAnswer.PollVotingRecords)
                .HasForeignKey(record => record.PollAnswerId)
                .IsRequired();

            builder.HasOne(record => record.User)
                .WithMany()
                .HasForeignKey(record => record.UserId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}