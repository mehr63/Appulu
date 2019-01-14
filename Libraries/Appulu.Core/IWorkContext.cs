using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Directory;
using Appulu.Core.Domain.Localization;
using Appulu.Core.Domain.Tax;
using Appulu.Core.Domain.Vendors;

namespace Appulu.Core
{
    /// <summary>
    /// Represents work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        User CurrentUser { get; set; }

        /// <summary>
        /// Gets the original user (in case the current one is impersonated)
        /// </summary>
        User OriginalUserIfImpersonated { get; }

        /// <summary>
        /// Gets the current vendor (logged-in manager)
        /// </summary>
        Vendor CurrentVendor { get; }

        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        Language WorkingLanguage { get; set; }

        /// <summary>
        /// Gets or sets current user working currency
        /// </summary>
        Currency WorkingCurrency { get; set; }

        /// <summary>
        /// Gets or sets current tax display type
        /// </summary>
        TaxDisplayType TaxDisplayType { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }
    }
}
