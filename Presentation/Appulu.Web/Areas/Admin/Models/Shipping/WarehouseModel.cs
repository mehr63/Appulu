using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Models.Common;
using Appulu.Web.Areas.Admin.Validators.Shipping;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Shipping
{
    /// <summary>
    /// Represents a warehouse model
    /// </summary>
    [Validator(typeof(WarehouseValidator))]
    public partial class WarehouseModel : BaseAppEntityModel
    {
        #region Ctor

        public WarehouseModel()
        {
            this.Address = new AddressModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Shipping.Warehouses.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Warehouses.Fields.AdminComment")]
        public string AdminComment { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Warehouses.Fields.Address")]
        public AddressModel Address { get; set; }

        #endregion
    }
}