using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.Catalog;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a review type model
    /// </summary>
    [Validator(typeof(ReviewTypeValidator))]
    public partial class ReviewTypeModel : BaseAppEntityModel, ILocalizedModel<ReviewTypeLocalizedModel>
    {
        #region Ctor

        public ReviewTypeModel()
        {
            Locales = new List<ReviewTypeLocalizedModel>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Settings.ReviewType.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Settings.ReviewType.Fields.Description")]
        public string Description { get; set; }

        [AppResourceDisplayName("Admin.Settings.ReviewType.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Settings.ReviewType.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [AppResourceDisplayName("Admin.Settings.ReviewType.Fields.VisibleToAllUsers")]
        public bool VisibleToAllUsers { get; set; }

        public IList<ReviewTypeLocalizedModel> Locales { get; set; }

        #endregion
    }
}
