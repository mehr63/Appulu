using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.News;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news item model
    /// </summary>
    [Validator(typeof(NewsItemValidator))]
    public partial class NewsItemModel : BaseAppEntityModel, IStoreMappingSupportedModel
    {
        #region Ctor

        public NewsItemModel()
        {
            this.AvailableLanguages = new List<SelectListItem>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Language")]
        public int LanguageId { get; set; }

        public IList<SelectListItem> AvailableLanguages { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Language")]
        public string LanguageName { get; set; }

        //store mapping
        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Title")]
        public string Title { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Short")]
        public string Short { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Full")]
        public string Full { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.AllowComments")]
        public bool AllowComments { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.SeName")]
        public string SeName { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Published")]
        public bool Published { get; set; }

        public int ApprovedComments { get; set; }

        public int NotApprovedComments { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}