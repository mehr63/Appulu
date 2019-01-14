using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Catalog;

namespace Appulu.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a category template mapping configuration
    /// </summary>
    public partial class CategoryTemplateMap : AppEntityTypeConfiguration<CategoryTemplate>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CategoryTemplate> builder)
        {
            builder.ToTable(nameof(CategoryTemplate), Schemas.Catalog);
            builder.HasKey(template => template.Id);

            builder.Property(template => template.Name).HasMaxLength(400).IsRequired();
            builder.Property(template => template.ViewPath).HasMaxLength(400).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}