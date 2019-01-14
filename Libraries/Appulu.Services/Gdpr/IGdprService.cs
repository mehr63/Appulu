using System.Collections.Generic;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Gdpr;

namespace Appulu.Services.Gdpr
{
    /// <summary>
    /// Represents the GDPR service interface
    /// </summary>
    public partial interface IGdprService
    {
        #region GDPR consent

        /// <summary>
        /// Get a GDPR consent
        /// </summary>
        /// <param name="gdprConsentId">The GDPR consent identifier</param>
        /// <returns>GDPR consent</returns>
        GdprConsent GetConsentById(int gdprConsentId);

        /// <summary>
        /// Get all GDPR consents
        /// </summary>
        /// <returns>GDPR consent</returns>
        IList<GdprConsent> GetAllConsents();

        /// <summary>
        /// Insert a GDPR consent
        /// </summary>
        /// <param name="gdprConsent">GDPR consent</param>
        void InsertConsent(GdprConsent gdprConsent);

        /// <summary>
        /// Update the GDPR consent
        /// </summary>
        /// <param name="gdprConsent">GDPR consent</param>
        void UpdateConsent(GdprConsent gdprConsent);

        /// <summary>
        /// Delete a GDPR consent
        /// </summary>
        /// <param name="gdprConsent">GDPR consent</param>
        void DeleteConsent(GdprConsent gdprConsent);
        
        /// <summary>
        /// Gets the latest selected value (a consent is accepted or not by a user)
        /// </summary>
        /// <param name="consentId">Consent identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result; null if previous a user hasn't been asked</returns>
        bool? IsConsentAccepted(int consentId, int userId);

        #endregion

        #region GDPR log

        /// <summary>
        /// Get a GDPR log
        /// </summary>
        /// <param name="gdprLogId">The GDPR log identifier</param>
        /// <returns>GDPR log</returns>
        GdprLog GetLogById(int gdprLogId);

        /// <summary>
        /// Get all GDPR log records
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="consentId">Consent identifier</param>
        /// <param name="userInfo">User info (Exact match)</param>
        /// <param name="requestType">GDPR request type</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>GDPR log records</returns>
        IPagedList<GdprLog> GetAllLog(int userId = 0, int consentId = 0,
            string userInfo = "", GdprRequestType? requestType = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Insert a GDPR log
        /// </summary>
        /// <param name="gdprLog">GDPR log</param>
        void InsertLog(GdprLog gdprLog);

        /// <summary>
        /// Insert a GDPR log
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="consentId">Consent identifier</param>
        /// <param name="requestType">Request type</param>
        /// <param name="requestDetails">Request details</param>
        void InsertLog(User user, int consentId, GdprRequestType requestType, string requestDetails);

        /// <summary>
        /// Update the GDPR log
        /// </summary>
        /// <param name="gdprLog">GDPR log</param>
        void UpdateLog(GdprLog gdprLog);

        /// <summary>
        /// Delete a GDPR log
        /// </summary>
        /// <param name="gdprLog">GDPR log</param>
        void DeleteLog(GdprLog gdprLog);

        #endregion

        #region User

        /// <summary>
        /// Permanent delete of user
        /// </summary>
        /// <param name="user">User</param>
        void PermanentDeleteUser(User user);

        #endregion
    }
}
