using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Orders;

namespace Appulu.Data.Mapping.Orders
{
    /// <summary>
    /// Represents a return request mapping configuration
    /// </summary>
    public partial class ReturnRequestMap : AppEntityTypeConfiguration<ReturnRequest>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ReturnRequest> builder)
        {
            builder.ToTable(nameof(ReturnRequest), Schemas.Orders);
            builder.HasKey(returnRequest => returnRequest.Id);

            builder.Property(returnRequest => returnRequest.ReasonForReturn).IsRequired();
            builder.Property(returnRequest => returnRequest.RequestedAction).IsRequired();

            builder.HasOne(returnRequest => returnRequest.User)
                .WithMany(user => user.ReturnRequests)
                .HasForeignKey(returnRequest => returnRequest.UserId)
                .IsRequired();

            builder.Ignore(returnRequest => returnRequest.ReturnRequestStatus);

            base.Configure(builder);
        }

        #endregion
    }
}