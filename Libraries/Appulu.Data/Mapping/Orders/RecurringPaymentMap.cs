using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Orders;

namespace Appulu.Data.Mapping.Orders
{
    /// <summary>
    /// Represents a recurring payment mapping configuration
    /// </summary>
    public partial class RecurringPaymentMap : AppEntityTypeConfiguration<RecurringPayment>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<RecurringPayment> builder)
        {
            builder.ToTable(nameof(RecurringPayment), Schemas.Orders);
            builder.HasKey(recurringPayment => recurringPayment.Id);

            builder.HasOne(recurringPayment => recurringPayment.InitialOrder)
                .WithMany()
                .HasForeignKey(recurringPayment => recurringPayment.InitialOrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(recurringPayment => recurringPayment.NextPaymentDate);
            builder.Ignore(recurringPayment => recurringPayment.CyclesRemaining);
            builder.Ignore(recurringPayment => recurringPayment.CyclePeriod);

            base.Configure(builder);
        }

        #endregion
    }
}