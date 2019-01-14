using Appulu.Web.Areas.Admin.Models.Home;

namespace Appulu.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the home models factory
    /// </summary>
    public partial interface IHomeModelFactory
    {
        /// <summary>
        /// Prepare dashboard model
        /// </summary>
        /// <param name="model">Dashboard model</param>
        /// <returns>Dashboard model</returns>
        DashboardModel PrepareDashboardModel(DashboardModel model);

        /// <summary>
        /// Prepare Appulu news model
        /// </summary>
        /// <returns>Appulu news model</returns>
        AppuluNewsModel PrepareAppuluNewsModel();
    }
}