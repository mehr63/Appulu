using FluentValidation.Attributes;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.PrivateMessages;

namespace Appulu.Web.Models.PrivateMessages
{
    [Validator(typeof(SendPrivateMessageValidator))]
    public partial class SendPrivateMessageModel : BaseAppEntityModel
    {
        public int ToUserId { get; set; }
        public string UserToName { get; set; }
        public bool AllowViewingToProfile { get; set; }

        public int ReplyToMessageId { get; set; }
        
        public string Subject { get; set; }
        
        public string Message { get; set; }
    }
}