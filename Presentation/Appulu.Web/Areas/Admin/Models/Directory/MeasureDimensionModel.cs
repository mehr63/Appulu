using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Directory;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Directory
{
    /// <summary>
    /// Represents a measure dimension model
    /// </summary>
    [Validator(typeof(MeasureDimensionValidator))]
    public partial class MeasureDimensionModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Shipping.Measures.Dimensions.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Measures.Dimensions.Fields.SystemKeyword")]
        public string SystemKeyword { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Measures.Dimensions.Fields.Ratio")]
        public decimal Ratio { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Measures.Dimensions.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Measures.Dimensions.Fields.IsPrimaryDimension")]
        public bool IsPrimaryDimension { get; set; }

        #endregion
    }
}