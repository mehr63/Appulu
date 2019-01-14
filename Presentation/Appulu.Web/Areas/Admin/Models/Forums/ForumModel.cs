using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Forums;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Forums
{
    /// <summary>
    /// Represents a forum list model
    /// </summary>
    [Validator(typeof(ForumValidator))]
    public partial class ForumModel : BaseAppEntityModel
    {
        #region Ctor

        public ForumModel()
        {
            ForumGroups = new List<ForumGroupModel>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.ContentManagement.Forums.Forum.Fields.ForumGroupId")]
        public int ForumGroupId { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Forums.Forum.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Forums.Forum.Fields.Description")]
        public string Description { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Forums.Forum.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Forums.Forum.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        public List<ForumGroupModel> ForumGroups { get; set; }

        #endregion
    }
}