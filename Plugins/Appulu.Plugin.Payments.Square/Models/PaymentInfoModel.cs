using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Payments.Square.Models
{
    public class PaymentInfoModel : BaseAppModel
    {
        #region Ctor

        public PaymentInfoModel()
        {
            StoredCards = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public bool IsGuest { get; set; }

        public string CardNonce { get; set; }
        public string Errors { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.PostalCode")]
        public string PostalCode { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.SaveCard")]
        public bool SaveCard { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.StoredCard")]
        public string StoredCardId { get; set; }
        public IList<SelectListItem> StoredCards { get; set; }

        #endregion
    }
}