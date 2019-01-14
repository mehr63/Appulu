namespace Appulu.Services.Users
{
    /// <summary>
    /// Represents default values related to user services
    /// </summary>
    public static partial class AppUserServiceDefaults
    {
        #region User attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string UserAttributesAllCacheKey => "App.userattribute.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user attribute ID
        /// </remarks>
        public static string UserAttributesByIdCacheKey => "App.userattribute.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user attribute ID
        /// </remarks>
        public static string UserAttributeValuesAllCacheKey => "App.userattributevalue.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user attribute value ID
        /// </remarks>
        public static string UserAttributeValuesByIdCacheKey => "App.userattributevalue.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserAttributesPatternCacheKey => "App.userattribute.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserAttributeValuesPatternCacheKey => "App.userattributevalue.";

        #endregion

        #region User roles

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static string UserRolesAllCacheKey => "App.userrole.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        public static string UserRolesBySystemNameCacheKey => "App.userrole.systemname-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserRolesPatternCacheKey => "App.userrole.";

        #endregion

        /// <summary>
        /// Gets a key for caching current user password lifetime
        /// </summary>
        /// <remarks>
        /// {0} : user identifier
        /// </remarks>
        public static string UserPasswordLifetimeCacheKey => "App.users.passwordlifetime-{0}";

        /// <summary>
        /// Gets a password salt key size
        /// </summary>
        public static int PasswordSaltKeySize => 5;
        
        /// <summary>
        /// Gets a max username length
        /// </summary>
        public static int UserUsernameLength => 100;
    }
}