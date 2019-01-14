using Appulu.Web.Areas.Admin.Models.Common;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Affiliates
{
    /// <summary>
    /// Represents an affiliate model
    /// </summary>
    public partial class AffiliateModel : BaseAppEntityModel
    {
        #region Ctor

        public AffiliateModel()
        {
            Address = new AddressModel();
            AffiliatedOrderSearchModel= new AffiliatedOrderSearchModel();
            AffiliatedUserSearchModel = new AffiliatedUserSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Affiliates.Fields.URL")]
        public string Url { get; set; }
        
        [AppResourceDisplayName("Admin.Affiliates.Fields.AdminComment")]
        public string AdminComment { get; set; }

        [AppResourceDisplayName("Admin.Affiliates.Fields.FriendlyUrlName")]
        public string FriendlyUrlName { get; set; }
        
        [AppResourceDisplayName("Admin.Affiliates.Fields.Active")]
        public bool Active { get; set; }

        public AddressModel Address { get; set; }

        public AffiliatedOrderSearchModel AffiliatedOrderSearchModel { get; set; }

        public AffiliatedUserSearchModel AffiliatedUserSearchModel { get; set; }

        #endregion
    }
}