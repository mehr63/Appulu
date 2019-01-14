﻿using Appulu.Core.Infrastructure;
using Appulu.Services.Payments;

namespace Appulu.Plugin.Payments.Worldpay
{
    /// <summary>
    /// Represents constants of the Worldpay payment plugin
    /// </summary>
    public class WorldpayPaymentDefaults
    {
        /// <summary>
        /// Worldpay payment method system name
        /// </summary>
        public static string SystemName => "Payments.WorldpayUS";

        /// <summary>
        /// Name of the view component to display plugin in public store
        /// </summary>
        public const string ViewComponentName = "PaymentWorldpay";

        /// <summary>
        /// User agent used for requesting Worldpay services
        /// </summary>
        public static string UserAgent => "Appulu-plugin-3.0";

        /// <summary>
        /// Path to the Worldpay payment js script
        /// </summary>
        public static string PaymentScriptPath => "https://gwapi.demo.securenet.com/v1/PayOS.js";

        /// <summary>
        /// Key of the attribute to store Worldpay Vault user identifier
        /// </summary>
        public static string UserIdAttribute => "WorldpayUserId";

        /// <summary>
        /// Certified Appulu developer application ID
        /// </summary>
        public static string DeveloperId => "10000786";

        /// <summary>
        /// Certified Appulu developer application version
        /// </summary>
        public static string DeveloperVersion => EngineContext.Current.Resolve<IPaymentService>()?
            .LoadPaymentMethodBySystemName(SystemName)?.PluginDescriptor?.Version ?? "3.10";

        /// <summary>
        /// Sandbox application ID
        /// </summary>
        public static string SandboxDeveloperId => "12345678";

        /// <summary>
        /// Sandbox application version
        /// </summary>
        public static string SandboxDeveloperVersion => "1.2";
    }
}