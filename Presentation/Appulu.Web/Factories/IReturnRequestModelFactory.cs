using Appulu.Core.Domain.Orders;
using Appulu.Web.Models.Order;

namespace Appulu.Web.Factories
{
    /// <summary>
    /// Represents the interface of the return request model factory
    /// </summary>
    public partial interface IReturnRequestModelFactory
    {
        /// <summary>
        /// Prepare the order item model
        /// </summary>
        /// <param name="orderItem">Order item</param>
        /// <returns>Order item model</returns>
        SubmitReturnRequestModel.OrderItemModel PrepareSubmitReturnRequestOrderItemModel(OrderItem orderItem);

        /// <summary>
        /// Prepare the submit return request model
        /// </summary>
        /// <param name="model">Submit return request model</param>
        /// <param name="order">Order</param>
        /// <returns>Submit return request model</returns>
        SubmitReturnRequestModel PrepareSubmitReturnRequestModel(SubmitReturnRequestModel model, Order order);

        /// <summary>
        /// Prepare the user return requests model
        /// </summary>
        /// <returns>User return requests model</returns>
        UserReturnRequestsModel PrepareUserReturnRequestsModel();
    }
}
