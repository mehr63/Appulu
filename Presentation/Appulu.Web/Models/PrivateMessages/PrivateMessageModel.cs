using System;
using FluentValidation.Attributes;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.PrivateMessages;

namespace Appulu.Web.Models.PrivateMessages
{
    [Validator(typeof(SendPrivateMessageValidator))]
    public partial class PrivateMessageModel : BaseAppEntityModel
    {
        public int FromUserId { get; set; }
        public string UserFromName { get; set; }
        public bool AllowViewingFromProfile { get; set; }

        public int ToUserId { get; set; }
        public string UserToName { get; set; }
        public bool AllowViewingToProfile { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public bool IsRead { get; set; }
    }
}