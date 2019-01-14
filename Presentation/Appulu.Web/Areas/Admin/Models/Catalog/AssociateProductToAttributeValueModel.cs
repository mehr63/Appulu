using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product model to associate to the product attribute value
    /// </summary>
    public partial class AssociateProductToAttributeValueModel : BaseAppModel
    {
        #region Properties
        
        public int AssociatedToProductId { get; set; }

        #endregion
    }
}