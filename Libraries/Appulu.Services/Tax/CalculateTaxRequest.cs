using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;

namespace Appulu.Services.Tax
{
    /// <summary>
    /// Represents a request for tax calculation
    /// </summary>
    public partial class CalculateTaxRequest
    {
        /// <summary>
        /// Gets or sets a user
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets a product
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Gets or sets an address
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets a tax category identifier
        /// </summary>
        public int TaxCategoryId { get; set; }

        /// <summary>
        /// Gets or sets a price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a current store identifier
        /// </summary>
        public int CurrentStoreId { get; set; }
    }
}
