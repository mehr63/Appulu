namespace Appulu.Services.Forums
{
    /// <summary>
    /// Represents default values related to forums services
    /// </summary>
    public static partial class AppForumDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string ForumGroupAllCacheKey => "App.forumgroup.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : forum group ID
        /// </remarks>
        public static string ForumAllByForumGroupIdCacheKey => "App.forum.allbyforumgroupid-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ForumGroupPatternCacheKey => "App.forumgroup.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ForumPatternCacheKey => "App.forum.";
    }
}