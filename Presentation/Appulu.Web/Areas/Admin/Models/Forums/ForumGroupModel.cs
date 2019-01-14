using System;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Forums;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Forums
{
    /// <summary>
    /// Represents a forum group model
    /// </summary>
    [Validator(typeof(ForumGroupValidator))]
    public partial class ForumGroupModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.ContentManagement.Forums.ForumGroup.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Forums.ForumGroup.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Forums.ForumGroup.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}