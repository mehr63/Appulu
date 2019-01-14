using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Shipping;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Shipping
{
    /// <summary>
    /// Represents a delivery date model
    /// </summary>
    [Validator(typeof(DeliveryDateValidator))]
    public partial class DeliveryDateModel : BaseAppEntityModel, ILocalizedModel<DeliveryDateLocalizedModel>
    {
        #region Ctor

        public DeliveryDateModel()
        {
            Locales = new List<DeliveryDateLocalizedModel>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Shipping.DeliveryDates.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.DeliveryDates.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<DeliveryDateLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class DeliveryDateLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.DeliveryDates.Fields.Name")]
        public string Name { get; set; }
    }
}