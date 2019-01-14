using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Orders;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a return request reason model
    /// </summary>
    [Validator(typeof(ReturnRequestReasonValidator))]
    public partial class ReturnRequestReasonModel : BaseAppEntityModel, ILocalizedModel<ReturnRequestReasonLocalizedModel>
    {
        #region Ctor

        public ReturnRequestReasonModel()
        {
            Locales = new List<ReturnRequestReasonLocalizedModel>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestReasons.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestReasons.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<ReturnRequestReasonLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class ReturnRequestReasonLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestReasons.Name")]
        public string Name { get; set; }
    }
}