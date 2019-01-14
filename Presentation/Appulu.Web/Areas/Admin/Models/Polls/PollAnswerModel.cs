using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Polls;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Polls
{
    /// <summary>
    /// Represents a poll answer model
    /// </summary>
    [Validator(typeof(PollAnswerValidator))]
    public partial class PollAnswerModel : BaseAppEntityModel
    {
        #region Properties

        public int PollId { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Answers.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Answers.Fields.NumberOfVotes")]
        public int NumberOfVotes { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Answers.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}