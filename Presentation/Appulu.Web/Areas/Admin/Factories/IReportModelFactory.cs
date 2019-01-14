using Appulu.Web.Areas.Admin.Models.Reports;

namespace Appulu.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the report model factory
    /// </summary>
    public partial interface IReportModelFactory
    {
        #region LowStockProduct

        /// <summary>
        /// Prepare low stock product search model
        /// </summary>
        /// <param name="searchModel">Low stock product search model</param>
        /// <returns>Low stock product search model</returns>
        LowStockProductSearchModel PrepareLowStockProductSearchModel(LowStockProductSearchModel searchModel);

        /// <summary>
        /// Prepare paged low stock product list model
        /// </summary>
        /// <param name="searchModel">Low stock product search model</param>
        /// <returns>Low stock product list model</returns>
        LowStockProductListModel PrepareLowStockProductListModel(LowStockProductSearchModel searchModel);

        #endregion

        #region Bestseller

        /// <summary>
        /// Prepare bestseller search model
        /// </summary>
        /// <param name="searchModel">Bestseller search model</param>
        /// <returns>Bestseller search model</returns>
        BestsellerSearchModel PrepareBestsellerSearchModel(BestsellerSearchModel searchModel);

        /// <summary>
        /// Prepare paged bestseller list model
        /// </summary>
        /// <param name="searchModel">Bestseller search model</param>
        /// <returns>Bestseller list model</returns>
        BestsellerListModel PrepareBestsellerListModel(BestsellerSearchModel searchModel);

        #endregion

        #region NeverSold

        /// <summary>
        /// Prepare never sold report search model
        /// </summary>
        /// <param name="searchModel">Never sold report search model</param>
        /// <returns>Never sold report search model</returns>
        NeverSoldReportSearchModel PrepareNeverSoldSearchModel(NeverSoldReportSearchModel searchModel);

        /// <summary>
        /// Prepare paged never sold report list model
        /// </summary>
        /// <param name="searchModel">Never sold report search model</param>
        /// <returns>Never sold report list model</returns>
        NeverSoldReportListModel PrepareNeverSoldListModel(NeverSoldReportSearchModel searchModel);

        #endregion

        #region Country sales

        /// <summary>
        /// Prepare country report search model
        /// </summary>
        /// <param name="searchModel">Country report search model</param>
        /// <returns>Country report search model</returns>
        CountryReportSearchModel PrepareCountrySalesSearchModel(CountryReportSearchModel searchModel);

        /// <summary>
        /// Prepare paged country report list model
        /// </summary>
        /// <param name="searchModel">Country report search model</param>
        /// <returns>Country report list model</returns>
        CountryReportListModel PrepareCountrySalesListModel(CountryReportSearchModel searchModel);

        #endregion

        #region User reports

        /// <summary>
        /// Prepare user reports search model
        /// </summary>
        /// <param name="searchModel">User reports search model</param>
        /// <returns>User reports search model</returns>
        UserReportsSearchModel PrepareUserReportsSearchModel(UserReportsSearchModel searchModel);

        /// <summary>
        /// Prepare paged best users report list modelSearchModel searchModel
        /// </summary>
        /// <param name="searchModel">Best users report search model</param>
        /// <returns>Best users report list model</returns>
        BestUsersReportListModel PrepareBestUsersReportListModel(BestUsersReportSearchModel searchModel);

        /// <summary>
        /// Prepare paged registered users report list model
        /// </summary>
        /// <param name="searchModel">Registered users report search model</param>
        /// <returns>Registered users report list model</returns>
        RegisteredUsersReportListModel PrepareRegisteredUsersReportListModel(RegisteredUsersReportSearchModel searchModel);

        #endregion
    }
}
