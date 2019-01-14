
namespace Appulu.Plugin.DiscountRules.UserRoles
{
    /// <summary>
    /// Represents constants for the discount requirement rule
    /// </summary>
    public static class DiscountRequirementDefaults
    {
        /// <summary>
        /// The system name of the discount requirement rule
        /// </summary>
        public const string SystemName = "DiscountRequirement.MustBeAssignedToUserRole";

        /// <summary>
        /// The key of the settings to save restricted user roles
        /// </summary>
        public const string SettingsKey = "DiscountRequirement.MustBeAssignedToUserRole-{0}";

        /// <summary>
        /// The HTML field prefix for discount requirements
        /// </summary>
        public const string HtmlFieldPrefix = "DiscountRulesUserRoles{0}";
    }
}
