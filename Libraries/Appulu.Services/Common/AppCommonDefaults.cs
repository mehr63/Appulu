﻿namespace Appulu.Services.Common
{
    /// <summary>
    /// Represents default values related to common services
    /// </summary>
    public static partial class AppCommonDefaults
    {
        #region Address attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string AddressAttributesAllCacheKey => "App.addressattribute.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address attribute ID
        /// </remarks>
        public static string AddressAttributesByIdCacheKey => "App.addressattribute.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address attribute ID
        /// </remarks>
        public static string AddressAttributeValuesAllCacheKey => "App.addressattributevalue.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address attribute value ID
        /// </remarks>
        public static string AddressAttributeValuesByIdCacheKey => "App.addressattributevalue.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressAttributesPatternCacheKey => "App.addressattribute.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressAttributeValuesPatternCacheKey => "App.addressattributevalue.";

        /// <summary>
        /// Gets a name of the custom address attribute control
        /// </summary>
        /// <remarks>
        /// {0} : address attribute id
        /// </remarks>
        public static string AddressAttributeControlName => "address_attribute_{0}";

        #endregion

        #region Addresses

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address ID
        /// </remarks>
        public static string AddressesByIdCacheKey => "App.address.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressesPatternCacheKey => "App.address.";

        #endregion

        #region Generic attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : key group
        /// </remarks>
        public static string GenericAttributeCacheKey => "App.genericattribute.{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string GenericAttributePatternCacheKey => "App.genericattribute.";

        #endregion

        #region Maintenance

        /// <summary>
        /// Gets a path to the database backup files
        /// </summary>
        public static string DbBackupsPath => "db_backups\\";

        /// <summary>
        /// Gets a database backup file extension
        /// </summary>
        public static string DbBackupFileExtension => "bak";

        #endregion
    }
}