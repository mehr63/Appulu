using System.Collections.Generic;
using Appulu.Web.Framework.Models;
using Appulu.Web.Models.Media;

namespace Appulu.Web.Models.ShoppingCart
{
    public partial class MiniShoppingCartModel : BaseAppModel
    {
        public MiniShoppingCartModel()
        {
            Items = new List<ShoppingCartItemModel>();
        }

        public IList<ShoppingCartItemModel> Items { get; set; }
        public int TotalProducts { get; set; }
        public string SubTotal { get; set; }
        public bool DisplayShoppingCartButton { get; set; }
        public bool DisplayCheckoutButton { get; set; }
        public bool CurrentUserIsGuest { get; set; }
        public bool AnonymousCheckoutAllowed { get; set; }
        public bool ShowProductImages { get; set; }

        #region Nested Classes

        public partial class ShoppingCartItemModel : BaseAppEntityModel
        {
            public ShoppingCartItemModel()
            {
                Picture = new PictureModel();
            }

            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public string ProductSeName { get; set; }

            public int Quantity { get; set; }

            public string UnitPrice { get; set; }

            public string AttributeInfo { get; set; }

            public PictureModel Picture { get; set; }
        }

        #endregion
    }
}