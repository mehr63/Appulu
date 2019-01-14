using System;
using System.Collections.Generic;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a shipment model
    /// </summary>
    public partial class ShipmentModel : BaseAppEntityModel
    {
        #region Ctor

        public ShipmentModel()
        {
            this.ShipmentStatusEvents = new List<ShipmentStatusEventModel>();
            this.Items = new List<ShipmentItemModel>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Orders.Shipments.ID")]
        public override int Id { get; set; }

        public int OrderId { get; set; }

        [AppResourceDisplayName("Admin.Orders.Shipments.CustomOrderNumber")]
        public string CustomOrderNumber { get; set; }

        [AppResourceDisplayName("Admin.Orders.Shipments.TotalWeight")]
        public string TotalWeight { get; set; }

        [AppResourceDisplayName("Admin.Orders.Shipments.TrackingNumber")]
        public string TrackingNumber { get; set; }

        public string TrackingNumberUrl { get; set; }

        [AppResourceDisplayName("Admin.Orders.Shipments.ShippedDate")]
        public string ShippedDate { get; set; }

        public bool CanShip { get; set; }

        public DateTime? ShippedDateUtc { get; set; }

        [AppResourceDisplayName("Admin.Orders.Shipments.DeliveryDate")]
        public string DeliveryDate { get; set; }

        public bool CanDeliver { get; set; }

        public DateTime? DeliveryDateUtc { get; set; }

        [AppResourceDisplayName("Admin.Orders.Shipments.AdminComment")]
        public string AdminComment { get; set; }

        public List<ShipmentItemModel> Items { get; set; }

        public IList<ShipmentStatusEventModel> ShipmentStatusEvents { get; set; }

        #endregion
    }
}