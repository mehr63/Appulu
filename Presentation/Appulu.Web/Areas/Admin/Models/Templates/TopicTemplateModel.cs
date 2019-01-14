using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Templates;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a topic template model
    /// </summary>
    [Validator(typeof(TopicTemplateValidator))]
    public partial class TopicTemplateModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.System.Templates.Topic.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.System.Templates.Topic.ViewPath")]
        public string ViewPath { get; set; }

        [AppResourceDisplayName("Admin.System.Templates.Topic.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}