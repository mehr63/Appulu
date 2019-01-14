using System.ComponentModel.DataAnnotations;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Newsletter
{
    public partial class NewsletterBoxModel : BaseAppModel
    {
        [DataType(DataType.EmailAddress)]
        public string NewsletterEmail { get; set; }
        public bool AllowToUnsubscribe { get; set; }
    }
}