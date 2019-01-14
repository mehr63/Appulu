using System;
using System.Linq;

namespace Appulu.Core.Domain.Users
{
    /// <summary>
    /// User extensions
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Gets a value indicating whether user is in a certain user role
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userRoleSystemName">User role system name</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsInUserRole(this User user,
            string userRoleSystemName, bool onlyActiveUserRoles = true)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(userRoleSystemName))
                throw new ArgumentNullException(nameof(userRoleSystemName));

            var result = user.UserRoles
                .FirstOrDefault(cr => (!onlyActiveUserRoles || cr.Active) && cr.SystemName == userRoleSystemName) != null;
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether user a search engine
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public static bool IsSearchEngineAccount(this User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!user.IsSystemAccount || string.IsNullOrEmpty(user.SystemName))
                return false;

            var result = user.SystemName.Equals(AppUserDefaults.SearchEngineUserName, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the user is a built-in record for background tasks
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public static bool IsBackgroundTaskAccount(this User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!user.IsSystemAccount || string.IsNullOrEmpty(user.SystemName))
                return false;

            var result = user.SystemName.Equals(AppUserDefaults.BackgroundTaskUserName, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether user is administrator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsAdmin(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, AppUserDefaults.AdministratorsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is a forum moderator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsForumModerator(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, AppUserDefaults.ForumModeratorsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is registered
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, AppUserDefaults.RegisteredRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is guest
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsGuest(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, AppUserDefaults.GuestsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is vendor
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsVendor(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, AppUserDefaults.VendorsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Get user role identifiers
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>User role identifiers</returns>
        public static int[] GetUserRoleIds(this User user, bool showHidden = false)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userRolesIds = user.UserRoles
               .Where(cr => showHidden || cr.Active)
               .Select(cr => cr.Id)
               .ToArray();

            return userRolesIds;
        }
    }
}