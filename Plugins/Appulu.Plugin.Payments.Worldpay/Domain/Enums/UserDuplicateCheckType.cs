
namespace Appulu.Plugin.Payments.Worldpay.Domain.Enums
{
    /// <summary>
    /// User duplicate check type enumeration. Indicates how should behave if the user ID already exists. 
    /// </summary>
    public enum UserDuplicateCheckType
    {
        /// <summary>
        /// If User ID exists then return an error.
        /// </summary>
        Error = 0,

        /// <summary>
        ///  If user ID exists then do not add account but continue with transaction.
        /// </summary>
        Ignore = 1
    }
}