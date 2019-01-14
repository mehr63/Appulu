using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Messages;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Messages
{
    /// <summary>
    /// Represents a newsletter subscription model
    /// </summary>
    [Validator(typeof(NewsLetterSubscriptionValidator))]
    public partial class NewsletterSubscriptionModel : BaseAppEntityModel
    {
        #region Properties

        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Email")]
        public string Email { get; set; }

        [AppResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Active")]
        public bool Active { get; set; }

        [AppResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Store")]
        public string StoreName { get; set; }

        [AppResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}