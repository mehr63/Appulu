using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Web.Areas.Admin.Models.Users;

namespace Appulu.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the user model factory
    /// </summary>
    public partial interface IUserModelFactory
    {
        /// <summary>
        /// Prepare user search model
        /// </summary>
        /// <param name="searchModel">User search model</param>
        /// <returns>User search model</returns>
        UserSearchModel PrepareUserSearchModel(UserSearchModel searchModel);

        /// <summary>
        /// Prepare paged user list model
        /// </summary>
        /// <param name="searchModel">User search model</param>
        /// <returns>User list model</returns>
        UserListModel PrepareUserListModel(UserSearchModel searchModel);

        /// <summary>
        /// Prepare user model
        /// </summary>
        /// <param name="model">User model</param>
        /// <param name="user">User</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>User model</returns>
        UserModel PrepareUserModel(UserModel model, User user, bool excludeProperties = false);

        /// <summary>
        /// Prepare paged reward points list model
        /// </summary>
        /// <param name="searchModel">Reward points search model</param>
        /// <param name="user">User</param>
        /// <returns>Reward points list model</returns>
        UserRewardPointsListModel PrepareRewardPointsListModel(UserRewardPointsSearchModel searchModel, User user);

        /// <summary>
        /// Prepare paged user address list model
        /// </summary>
        /// <param name="searchModel">User address search model</param>
        /// <param name="user">User</param>
        /// <returns>User address list model</returns>
        UserAddressListModel PrepareUserAddressListModel(UserAddressSearchModel searchModel, User user);

        /// <summary>
        /// Prepare user address model
        /// </summary>
        /// <param name="model">User address model</param>
        /// <param name="user">User</param>
        /// <param name="address">Address</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>User address model</returns>
        UserAddressModel PrepareUserAddressModel(UserAddressModel model,
            User user, Address address, bool excludeProperties = false);

        /// <summary>
        /// Prepare paged user order list model
        /// </summary>
        /// <param name="searchModel">User order search model</param>
        /// <param name="user">User</param>
        /// <returns>User order list model</returns>
        UserOrderListModel PrepareUserOrderListModel(UserOrderSearchModel searchModel, User user);        
        
        /// <summary>
        /// Prepare paged user shopping cart list model
        /// </summary>
        /// <param name="searchModel">User shopping cart search model</param>
        /// <param name="user">User</param>
        /// <returns>User shopping cart list model</returns>
        UserShoppingCartListModel PrepareUserShoppingCartListModel(UserShoppingCartSearchModel searchModel,
            User user);

        /// <summary>
        /// Prepare paged user activity log list model
        /// </summary>
        /// <param name="searchModel">User activity log search model</param>
        /// <param name="user">User</param>
        /// <returns>User activity log list model</returns>
        UserActivityLogListModel PrepareUserActivityLogListModel(UserActivityLogSearchModel searchModel, User user);
        
        /// <summary>
        /// Prepare paged user back in stock subscriptions list model
        /// </summary>
        /// <param name="searchModel">User back in stock subscriptions search model</param>
        /// <param name="user">User</param>
        /// <returns>User back in stock subscriptions list model</returns>
        UserBackInStockSubscriptionListModel PrepareUserBackInStockSubscriptionListModel(
            UserBackInStockSubscriptionSearchModel searchModel, User user);

        /// <summary>
        /// Prepare online user search model
        /// </summary>
        /// <param name="searchModel">Online user search model</param>
        /// <returns>Online user search model</returns>
        OnlineUserSearchModel PrepareOnlineUserSearchModel(OnlineUserSearchModel searchModel);

        /// <summary>
        /// Prepare paged online user list model
        /// </summary>
        /// <param name="searchModel">Online user search model</param>
        /// <returns>Online user list model</returns>
        OnlineUserListModel PrepareOnlineUserListModel(OnlineUserSearchModel searchModel);

        /// <summary>
        /// Prepare GDPR request (log) search model
        /// </summary>
        /// <param name="searchModel">GDPR request search model</param>
        /// <returns>GDPR request search model</returns>
        GdprLogSearchModel PrepareGdprLogSearchModel(GdprLogSearchModel searchModel);

        /// <summary>
        /// Prepare paged GDPR request list model
        /// </summary>
        /// <param name="searchModel">GDPR request search model</param>
        /// <returns>GDPR request list model</returns>
        GdprLogListModel PrepareGdprLogListModel(GdprLogSearchModel searchModel);

    }
}