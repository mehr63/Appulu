using System;
using System.Linq;
using Appulu.Core;
using Appulu.Core.Data;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Orders;
using Appulu.Core.Domain.Payments;
using Appulu.Core.Domain.Shipping;
using Appulu.Services.Helpers;

namespace Appulu.Services.Users
{
    /// <summary>
    /// User report service
    /// </summary>
    public partial class UserReportService : IUserReportService
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Order> _orderRepository;

        #endregion

        #region Ctor

        public UserReportService(IUserService userService,
            IDateTimeHelper dateTimeHelper,
            IRepository<User> userRepository,
            IRepository<Order> orderRepository)
        {
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._userRepository = userRepository;
            this._orderRepository = orderRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get best users
        /// </summary>
        /// <param name="createdFromUtc">Order created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Order created date to (UTC); null to load all records</param>
        /// <param name="os">Order status; null to load all records</param>
        /// <param name="ps">Order payment status; null to load all records</param>
        /// <param name="ss">Order shipment status; null to load all records</param>
        /// <param name="orderBy">1 - order by order total, 2 - order by number of orders</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Report</returns>
        public virtual IPagedList<BestUserReportLine> GetBestUsersReport(DateTime? createdFromUtc,
            DateTime? createdToUtc, OrderStatus? os, PaymentStatus? ps, ShippingStatus? ss, int orderBy,
            int pageIndex = 0, int pageSize = 214748364)
        {
            int? orderStatusId = null;
            if (os.HasValue)
                orderStatusId = (int)os.Value;

            int? paymentStatusId = null;
            if (ps.HasValue)
                paymentStatusId = (int)ps.Value;

            int? shippingStatusId = null;
            if (ss.HasValue)
                shippingStatusId = (int)ss.Value;
            var query1 = from c in _userRepository.Table
                         join o in _orderRepository.Table on c.Id equals o.UserId
                         where (!createdFromUtc.HasValue || createdFromUtc.Value <= o.CreatedOnUtc) &&
                         (!createdToUtc.HasValue || createdToUtc.Value >= o.CreatedOnUtc) &&
                         (!orderStatusId.HasValue || orderStatusId == o.OrderStatusId) &&
                         (!paymentStatusId.HasValue || paymentStatusId == o.PaymentStatusId) &&
                         (!shippingStatusId.HasValue || shippingStatusId == o.ShippingStatusId) &&
                         !o.Deleted &&
                         !c.Deleted
                         select new { c, o };

            var query2 = from co in query1
                         group co by co.c.Id into g
                         select new
                         {
                             UserId = g.Key,
                             OrderTotal = g.Sum(x => x.o.OrderTotal),
                             OrderCount = g.Count()
                         };
            switch (orderBy)
            {
                case 1:
                    query2 = query2.OrderByDescending(x => x.OrderTotal);
                    break;
                case 2:
                    query2 = query2.OrderByDescending(x => x.OrderCount);
                    break;
                default:
                    throw new ArgumentException("Wrong orderBy parameter", "orderBy");
            }

            var tmp = new PagedList<dynamic>(query2, pageIndex, pageSize);
            return new PagedList<BestUserReportLine>(tmp.Select(x => new BestUserReportLine
            {
                UserId = x.UserId,
                OrderTotal = x.OrderTotal,
                OrderCount = x.OrderCount
            }),
                tmp.PageIndex, tmp.PageSize, tmp.TotalCount);
        }

        /// <summary>
        /// Gets a report of users registered in the last days
        /// </summary>
        /// <param name="days">Users registered in the last days</param>
        /// <returns>Number of registered users</returns>
        public virtual int GetRegisteredUsersReport(int days)
        {
            var date = _dateTimeHelper.ConvertToUserTime(DateTime.Now).AddDays(-days);

            var registeredUserRole = _userService.GetUserRoleBySystemName(AppUserDefaults.RegisteredRoleName);
            if (registeredUserRole == null)
                return 0;

            var query = from c in _userRepository.Table
                        from mapping in c.UserUserRoleMappings
                        where !c.Deleted &&
                        mapping.UserRoleId == registeredUserRole.Id &&
                        c.CreatedOnUtc >= date
                        //&& c.CreatedOnUtc <= DateTime.UtcNow
                        select c;

            var count = query.Count();
            return count;
        }

        #endregion
    }
}