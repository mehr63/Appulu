using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Vendors;

namespace Appulu.Data.Mapping.Vendors
{
    /// <summary>
    /// Represents a vendor note mapping configuration
    /// </summary>
    public partial class VendorNoteMap : AppEntityTypeConfiguration<VendorNote>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<VendorNote> builder)
        {
            builder.ToTable(nameof(VendorNote), Schemas.Vendors);
            builder.HasKey(note => note.Id);

            builder.Property(note => note.Note).IsRequired();

            builder.HasOne(note => note.Vendor)
                .WithMany(vendor => vendor.VendorNotes)
                .HasForeignKey(note => note.VendorId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}