﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Logging;

namespace Appulu.Data.Mapping.Logging
{
    /// <summary>
    /// Represents an activity log type mapping configuration
    /// </summary>
    public partial class ActivityLogTypeMap : AppEntityTypeConfiguration<ActivityLogType>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ActivityLogType> builder)
        {
            builder.ToTable(nameof(ActivityLogType), Schemas.Localization);
            builder.HasKey(logType => logType.Id);

            builder.Property(logType => logType.SystemKeyword).HasMaxLength(100).IsRequired();
            builder.Property(logType => logType.Name).HasMaxLength(200).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}