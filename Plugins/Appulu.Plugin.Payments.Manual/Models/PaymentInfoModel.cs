using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Payments.Manual.Models
{
    public class PaymentInfoModel : BaseAppModel
    {
        public PaymentInfoModel()
        {
            CreditCardTypes = new List<SelectListItem>();
            ExpireMonths = new List<SelectListItem>();
            ExpireYears = new List<SelectListItem>();
        }

        [AppResourceDisplayName("Payment.SelectCreditCard")]
        public string CreditCardType { get; set; }

        [AppResourceDisplayName("Payment.SelectCreditCard")]
        public IList<SelectListItem> CreditCardTypes { get; set; }

        [AppResourceDisplayName("Payment.CardholderName")]
        public string CardholderName { get; set; }

        [AppResourceDisplayName("Payment.CardNumber")]
        public string CardNumber { get; set; }

        [AppResourceDisplayName("Payment.ExpirationDate")]
        public string ExpireMonth { get; set; }

        [AppResourceDisplayName("Payment.ExpirationDate")]
        public string ExpireYear { get; set; }

        public IList<SelectListItem> ExpireMonths { get; set; }

        public IList<SelectListItem> ExpireYears { get; set; }

        [AppResourceDisplayName("Payment.CardCode")]
        public string CardCode { get; set; }
    }
}