using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Directory;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Directory
{
    /// <summary>
    /// Represents a measure weight model
    /// </summary>
    [Validator(typeof(MeasureWeightValidator))]
    public partial class MeasureWeightModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Shipping.Measures.Weights.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Measures.Weights.Fields.SystemKeyword")]
        public string SystemKeyword { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Measures.Weights.Fields.Ratio")]
        public decimal Ratio { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Measures.Weights.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Measures.Weights.Fields.IsPrimaryWeight")]
        public bool IsPrimaryWeight { get; set; }

        #endregion
    }
}