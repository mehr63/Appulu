using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.Polls;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Polls
{
    /// <summary>
    /// Represents a poll model
    /// </summary>
    [Validator(typeof(PollValidator))]
    public partial class PollModel : BaseAppEntityModel, IStoreMappingSupportedModel
    {
        #region Ctor

        public PollModel()
        {
            this.AvailableLanguages = new List<SelectListItem>();
            this.AvailableStores = new List<SelectListItem>();
            this.SelectedStoreIds = new List<int>();
            this.PollAnswerSearchModel = new PollAnswerSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.Language")]
        public int LanguageId { get; set; }

        public IList<SelectListItem> AvailableLanguages { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.Language")]
        public string LanguageName { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.SystemKeyword")]
        public string SystemKeyword { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.Published")]
        public bool Published { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.ShowOnHomePage")]
        public bool ShowOnHomePage { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.AllowGuestsToVote")]
        public bool AllowGuestsToVote { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }
        
        [AppResourceDisplayName("Admin.ContentManagement.Polls.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public PollAnswerSearchModel PollAnswerSearchModel { get; set; }

        #endregion
    }
}