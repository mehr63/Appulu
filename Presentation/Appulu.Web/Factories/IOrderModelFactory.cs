using Appulu.Core.Domain.Orders;
using Appulu.Core.Domain.Shipping;
using Appulu.Web.Models.Order;

namespace Appulu.Web.Factories
{
    /// <summary>
    /// Represents the interface of the order model factory
    /// </summary>
    public partial interface IOrderModelFactory
    {
        /// <summary>
        /// Prepare the user order list model
        /// </summary>
        /// <returns>User order list model</returns>
        UserOrderListModel PrepareUserOrderListModel();

        /// <summary>
        /// Prepare the order details model
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Order details model</returns>
        OrderDetailsModel PrepareOrderDetailsModel(Order order);

        /// <summary>
        /// Prepare the shipment details model
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <returns>Shipment details model</returns>
        ShipmentDetailsModel PrepareShipmentDetailsModel(Shipment shipment);

        /// <summary>
        /// Prepare the user reward points model
        /// </summary>
        /// <param name="page">Number of items page; pass null to load the first page</param>
        /// <returns>User reward points model</returns>
        UserRewardPointsModel PrepareUserRewardPoints(int? page);
    }
}
