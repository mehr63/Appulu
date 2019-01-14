namespace Appulu.Services.Security
{
    /// <summary>
    /// Represents default values related to security services
    /// </summary>
    public static partial class AppSecurityDefaults
    {
        #region Access control list

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static string AclRecordByEntityIdNameCacheKey => "App.aclrecord.entityid-name-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AclRecordPatternCacheKey => "App.aclrecord.";

        #endregion

        #region Permissions

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user role ID
        /// {1} : permission system name
        /// </remarks>
        public static string PermissionsAllowedCacheKey => "App.permission.allowed-{0}-{1}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user role ID
        /// </remarks>
        public static string PermissionsAllByUserRoleIdCacheKey => "App.permission.allbyuserroleid-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string PermissionsPatternCacheKey => "App.permission.";

        #endregion
    }
}