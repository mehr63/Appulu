using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Catalog;

namespace Appulu.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a product tag with count mapping configuration
    /// </summary>
    public partial class ProductTagWithCountMap : AppQueryTypeConfiguration<ProductTagWithCount>
    {
    }
}