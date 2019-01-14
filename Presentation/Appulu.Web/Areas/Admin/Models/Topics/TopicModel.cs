using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.Topics;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Topics
{
    /// <summary>
    /// Represents a topic model
    /// </summary>
    [Validator(typeof(TopicValidator))]
    public partial class TopicModel : BaseAppEntityModel, IAclSupportedModel, ILocalizedModel<TopicLocalizedModel>, IStoreMappingSupportedModel
    {
        #region Ctor

        public TopicModel()
        {
            AvailableTopicTemplates = new List<SelectListItem>();
            Locales = new List<TopicLocalizedModel>();

            SelectedUserRoleIds = new List<int>();
            AvailableUserRoles = new List<SelectListItem>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.SystemName")]
        public string SystemName { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInSitemap")]
        public bool IncludeInSitemap { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInTopMenu")]
        public bool IncludeInTopMenu { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn1")]
        public bool IncludeInFooterColumn1 { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn2")]
        public bool IncludeInFooterColumn2 { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn3")]
        public bool IncludeInFooterColumn3 { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.AccessibleWhenStoreClosed")]
        public bool AccessibleWhenStoreClosed { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.IsPasswordProtected")]
        public bool IsPasswordProtected { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.Password")]
        public string Password { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.URL")]
        public string Url { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.Title")]
        public string Title { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.Body")]
        public string Body { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.Published")]
        public bool Published { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.TopicTemplate")]
        public int TopicTemplateId { get; set; }

        public IList<SelectListItem> AvailableTopicTemplates { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.SeName")]
        public string SeName { get; set; }

        public IList<TopicLocalizedModel> Locales { get; set; }

        //store mapping
        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        //ACL (user roles)
        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.AclUserRoles")]
        public IList<int> SelectedUserRoleIds { get; set; }

        public IList<SelectListItem> AvailableUserRoles { get; set; }

        #endregion
    }

    public partial class TopicLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.Title")]
        public string Title { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.Body")]
        public string Body { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Topics.Fields.SeName")]
        public string SeName { get; set; }
    }
}