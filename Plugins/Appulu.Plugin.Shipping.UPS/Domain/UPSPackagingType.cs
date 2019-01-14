
namespace Appulu.Plugin.Shipping.UPS.Domain
{
    /// <summary>
    /// UPSP packaging type
    /// </summary>
    public enum UPSPackagingType 
    {
        /// <summary>
        /// User supplied package
        /// </summary>
        UserSuppliedPackage,
        /// <summary>
        /// Letter
        /// </summary>
        Letter,
        /// <summary>
        /// Tube
        /// </summary>
        Tube,
        /// <summary>
        /// PAK
        /// </summary>
        PAK,
        /// <summary>
        /// ExpressBox
        /// </summary>
        ExpressBox,
        /// <summary>
        /// _10 Kg Box
        /// </summary>
        _10KgBox,
        /// <summary>
        /// _25 Kg Box
        /// </summary>
        _25KgBox
    }
}
