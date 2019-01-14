using System.Collections.Generic;
using Appulu.Web.Areas.Admin.Models.Users;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Security
{
    /// <summary>
    /// Represents a permission mapping model
    /// </summary>
    public partial class PermissionMappingModel : BaseAppModel
    {
        #region Ctor

        public PermissionMappingModel()
        {
            AvailablePermissions = new List<PermissionRecordModel>();
            AvailableUserRoles = new List<UserRoleModel>();
            Allowed = new Dictionary<string, IDictionary<int, bool>>();
        }

        #endregion

        #region Properties

        public IList<PermissionRecordModel> AvailablePermissions { get; set; }

        public IList<UserRoleModel> AvailableUserRoles { get; set; }

        //[permission system name] / [user role id] / [allowed]
        public IDictionary<string, IDictionary<int, bool>> Allowed { get; set; }

        #endregion
    }
}