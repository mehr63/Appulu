using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Orders;

namespace Appulu.Data.Mapping.Orders
{
    /// <summary>
    /// Represents a return request reason mapping configuration
    /// </summary>
    public partial class ReturnRequestReasonMap : AppEntityTypeConfiguration<ReturnRequestReason>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ReturnRequestReason> builder)
        {
            builder.ToTable(nameof(ReturnRequestReason), Schemas.Orders);
            builder.HasKey(reason => reason.Id);

            builder.Property(reason => reason.Name).HasMaxLength(400).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}