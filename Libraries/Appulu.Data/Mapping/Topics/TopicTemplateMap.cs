using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Topics;

namespace Appulu.Data.Mapping.Topics
{
    /// <summary>
    /// Represents a topic template mapping configuration
    /// </summary>
    public partial class TopicTemplateMap : AppEntityTypeConfiguration<TopicTemplate>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<TopicTemplate> builder)
        {
            builder.ToTable(nameof(TopicTemplate), Schemas.Topics);
            builder.HasKey(template => template.Id);

            builder.Property(template => template.Name).HasMaxLength(400).IsRequired();
            builder.Property(template => template.ViewPath).HasMaxLength(400).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}