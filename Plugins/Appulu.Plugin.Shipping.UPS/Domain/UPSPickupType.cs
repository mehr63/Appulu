
namespace Appulu.Plugin.Shipping.UPS.Domain
{
    /// <summary>
    /// UPS pickup type
    /// </summary>
    public enum UPSPickupType
    {
        /// <summary>
        /// Daily pickup
        /// </summary>
        DailyPickup,
        /// <summary>
        /// User counter
        /// </summary>
        UserCounter,
        /// <summary>
        /// One time pickup
        /// </summary>
        OneTimePickup,
        /// <summary>
        /// On call air
        /// </summary>
        OnCallAir,
        /// <summary>
        /// Suggested retail rates
        /// </summary>
        SuggestedRetailRates,
        /// <summary>
        /// Letter center
        /// </summary>
        LetterCenter,
        /// <summary>
        /// Air service center
        /// </summary>
        AirServiceCenter
    }
}
