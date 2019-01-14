﻿using Appulu.Core.Configuration;

namespace Appulu.Core.Domain.Gdpr
{
    /// <summary>
    /// GDPR settings
    /// </summary>
    public class GdprSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether GDPR is enabled
        /// </summary>
        public bool GdprEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should log "accept privacy policy" consent
        /// </summary>
        public bool LogPrivacyPolicyConsent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should log "newsletter" consent
        /// </summary>
        public bool LogNewsletterConsent { get; set; }
    }
}