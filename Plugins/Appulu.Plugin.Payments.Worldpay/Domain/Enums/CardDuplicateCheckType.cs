
namespace Appulu.Plugin.Payments.Worldpay.Domain.Enums
{
    /// <summary>
    /// Credit card duplicate check type enumeration. Indicates how (and whether) should check for duplicate cards. 
    /// </summary>
    public enum CardDuplicateCheckType
    {
        /// <summary>
        /// Does not check for duplicate card number for specified user ID
        /// </summary>
        NoCheck = 0,

        /// <summary>
        /// Checks for duplicate card number for specified user ID
        /// </summary>
        CheckWithinUser = 1,

        /// <summary>
        /// Checks for duplicate card number for all user IDs for specified SecureNet ID 
        /// </summary>
        CheckWithinAllUsers = 2,

        /// <summary>
        /// Checks for duplicate card number for all user IDs for specified Group ID
        /// </summary>
        CheckWithinUsersInGroup = 3
    }
}