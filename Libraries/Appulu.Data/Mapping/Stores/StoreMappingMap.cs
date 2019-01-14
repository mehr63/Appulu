using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Stores;

namespace Appulu.Data.Mapping.Stores
{
    /// <summary>
    /// Represents a store mapping mapping configuration
    /// </summary>
    public partial class StoreMappingMap : AppEntityTypeConfiguration<StoreMapping>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<StoreMapping> builder)
        {
            builder.ToTable(nameof(StoreMapping), Schemas.Stores);
            builder.HasKey(storeMapping => storeMapping.Id);

            builder.Property(storeMapping => storeMapping.EntityName).HasMaxLength(400).IsRequired();

            builder.HasOne(storeMapping => storeMapping.Store)
                .WithMany()
                .HasForeignKey(storeMapping => storeMapping.StoreId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}