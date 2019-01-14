using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Appulu.Core;
using Appulu.Core.Domain.Blogs;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Forums;
using Appulu.Core.Domain.Messages;
using Appulu.Core.Domain.News;
using Appulu.Core.Domain.Orders;
using Appulu.Core.Domain.Shipping;
using Appulu.Core.Domain.Vendors;
using Appulu.Services.Affiliates;
using Appulu.Services.Users;
using Appulu.Services.Events;
using Appulu.Services.Localization;
using Appulu.Services.Stores;

namespace Appulu.Services.Messages
{
    /// <summary>
    /// Workflow message service
    /// </summary>
    public partial class WorkflowMessageService : IWorkflowMessageService
    {
        #region Fields

        private readonly CommonSettings _commonSettings;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IAffiliateService _affiliateService;
        private readonly IUserService _userService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ITokenizer _tokenizer;

        #endregion

        #region Ctor

        public WorkflowMessageService(CommonSettings commonSettings,
            EmailAccountSettings emailAccountSettings,
            IAffiliateService affiliateService,
            IUserService userService,
            IEmailAccountService emailAccountService,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IMessageTemplateService messageTemplateService,
            IMessageTokenProvider messageTokenProvider,
            IQueuedEmailService queuedEmailService,
            IStoreContext storeContext,
            IStoreService storeService,
            ITokenizer tokenizer)
        {
            this._commonSettings = commonSettings;
            this._emailAccountSettings = emailAccountSettings;
            this._affiliateService = affiliateService;
            this._userService = userService;
            this._emailAccountService = emailAccountService;
            this._eventPublisher = eventPublisher;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._messageTemplateService = messageTemplateService;
            this._messageTokenProvider = messageTokenProvider;
            this._queuedEmailService = queuedEmailService;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._tokenizer = tokenizer;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get active message templates by the name
        /// </summary>
        /// <param name="messageTemplateName">Message template name</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>List of message templates</returns>
        protected virtual IList<MessageTemplate> GetActiveMessageTemplates(string messageTemplateName, int storeId)
        {
            //get message templates by the name
            var messageTemplates = _messageTemplateService.GetMessageTemplatesByName(messageTemplateName, storeId);

            //no template found
            if (!messageTemplates?.Any() ?? true)
                return new List<MessageTemplate>();

            //filter active templates
            messageTemplates = messageTemplates.Where(messageTemplate => messageTemplate.IsActive).ToList();

            return messageTemplates;
        }

        /// <summary>
        /// Get EmailAccount to use with a message templates
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>EmailAccount</returns>
        protected virtual EmailAccount GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate, int languageId)
        {
            var emailAccountId = _localizationService.GetLocalized(messageTemplate, mt => mt.EmailAccountId, languageId);
            //some 0 validation (for localizable "Email account" dropdownlist which saves 0 if "Standard" value is chosen)
            if (emailAccountId == 0)
                emailAccountId = messageTemplate.EmailAccountId;

            var emailAccount = (_emailAccountService.GetEmailAccountById(emailAccountId) ?? _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId)) ??
                               _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
            return emailAccount;
        }

        /// <summary>
        /// Ensure language is active
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Return a value language identifier</returns>
        protected virtual int EnsureLanguageIsActive(int languageId, int storeId)
        {
            //load language by specified ID
            var language = _languageService.GetLanguageById(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = _languageService.GetAllLanguages(storeId: storeId).FirstOrDefault();
            }

            if (language == null || !language.Published)
            {
                //load any language
                language = _languageService.GetAllLanguages().FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");

            return language.Id;
        }

        #endregion

        #region Methods

        #region User workflow

        /// <summary>
        /// Sends 'New user' notification message to a store owner
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendUserRegisteredNotificationMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserRegisteredNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a welcome message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendUserWelcomeMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserWelcomeMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = user.Email;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an email validation message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendUserEmailValidationMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserEmailValidationMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = user.Email;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an email re-validation message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendUserEmailRevalidationMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserEmailRevalidationMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                //email to re-validate
                var toEmail = user.EmailToRevalidate;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends password recovery message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendUserPasswordRecoveryMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserPasswordRecoveryMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = user.Email;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        #endregion

        #region Order workflow

        /// <summary>
        /// Sends an order placed notification to a vendor
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="vendor">Vendor instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderPlacedVendorNotification(Order order, Vendor vendor, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (vendor == null)
                throw new ArgumentNullException(nameof(vendor));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderPlacedVendorNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId, vendor.Id);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = vendor.Email;
                var toName = vendor.Name;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order placed notification to a store owner
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderPlacedStoreOwnerNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderPlacedStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order placed notification to an affiliate
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderPlacedAffiliateNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var affiliate = _affiliateService.GetAffiliateById(order.AffiliateId);

            if (affiliate == null)
                throw new ArgumentNullException(nameof(affiliate));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderPlacedAffiliateNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = affiliate.Address.Email;
                var toName = $"{affiliate.Address.FirstName} {affiliate.Address.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order paid notification to a store owner
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderPaidStoreOwnerNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderPaidStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order paid notification to an affiliate
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderPaidAffiliateNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var affiliate = _affiliateService.GetAffiliateById(order.AffiliateId);

            if (affiliate == null)
                throw new ArgumentNullException(nameof(affiliate));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderPaidAffiliateNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = affiliate.Address.Email;
                var toName = $"{affiliate.Address.FirstName} {affiliate.Address.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order paid notification to a user
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderPaidUserNotification(Order order, int languageId,
            string attachmentFilePath = null, string attachmentFileName = null)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderPaidUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = order.BillingAddress.Email;
                var toName = $"{order.BillingAddress.FirstName} {order.BillingAddress.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName,
                    attachmentFilePath, attachmentFileName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order paid notification to a vendor
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="vendor">Vendor instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderPaidVendorNotification(Order order, Vendor vendor, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (vendor == null)
                throw new ArgumentNullException(nameof(vendor));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderPaidVendorNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId, vendor.Id);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = vendor.Email;
                var toName = vendor.Name;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order placed notification to a user
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderPlacedUserNotification(Order order, int languageId,
            string attachmentFilePath = null, string attachmentFileName = null)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderPlacedUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = order.BillingAddress.Email;
                var toName = $"{order.BillingAddress.FirstName} {order.BillingAddress.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName,
                    attachmentFilePath, attachmentFileName);
            }).ToList();
        }

        /// <summary>
        /// Sends a shipment sent notification to a user
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendShipmentSentUserNotification(Shipment shipment, int languageId)
        {
            if (shipment == null)
                throw new ArgumentNullException(nameof(shipment));

            var order = shipment.Order;
            if (order == null)
                throw new Exception("Order cannot be loaded");

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ShipmentSentUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddShipmentTokens(commonTokens, shipment, languageId);
            _messageTokenProvider.AddOrderTokens(commonTokens, shipment.Order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, shipment.Order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = order.BillingAddress.Email;
                var toName = $"{order.BillingAddress.FirstName} {order.BillingAddress.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a shipment delivered notification to a user
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendShipmentDeliveredUserNotification(Shipment shipment, int languageId)
        {
            if (shipment == null)
                throw new ArgumentNullException(nameof(shipment));

            var order = shipment.Order;
            if (order == null)
                throw new Exception("Order cannot be loaded");

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ShipmentDeliveredUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddShipmentTokens(commonTokens, shipment, languageId);
            _messageTokenProvider.AddOrderTokens(commonTokens, shipment.Order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, shipment.Order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = order.BillingAddress.Email;
                var toName = $"{order.BillingAddress.FirstName} {order.BillingAddress.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order completed notification to a user
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderCompletedUserNotification(Order order, int languageId,
            string attachmentFilePath = null, string attachmentFileName = null)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderCompletedUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = order.BillingAddress.Email;
                var toName = $"{order.BillingAddress.FirstName} {order.BillingAddress.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName,
                    attachmentFilePath, attachmentFileName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order cancelled notification to a user
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderCancelledUserNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderCancelledUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = order.BillingAddress.Email;
                var toName = $"{order.BillingAddress.FirstName} {order.BillingAddress.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order refunded notification to a store owner
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="refundedAmount">Amount refunded</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderRefundedStoreOwnerNotification(Order order, decimal refundedAmount, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderRefundedStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId);
            _messageTokenProvider.AddOrderRefundedTokens(commonTokens, order, refundedAmount);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an order refunded notification to a user
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="refundedAmount">Amount refunded</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendOrderRefundedUserNotification(Order order, decimal refundedAmount, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderRefundedUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, order, languageId);
            _messageTokenProvider.AddOrderRefundedTokens(commonTokens, order, refundedAmount);
            _messageTokenProvider.AddUserTokens(commonTokens, order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = order.BillingAddress.Email;
                var toName = $"{order.BillingAddress.FirstName} {order.BillingAddress.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a new order note added notification to a user
        /// </summary>
        /// <param name="orderNote">Order note</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewOrderNoteAddedUserNotification(OrderNote orderNote, int languageId)
        {
            if (orderNote == null)
                throw new ArgumentNullException(nameof(orderNote));

            var order = orderNote.Order;

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewOrderNoteAddedUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderNoteTokens(commonTokens, orderNote);
            _messageTokenProvider.AddOrderTokens(commonTokens, orderNote.Order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, orderNote.Order.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = order.BillingAddress.Email;
                var toName = $"{order.BillingAddress.FirstName} {order.BillingAddress.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a "Recurring payment cancelled" notification to a store owner
        /// </summary>
        /// <param name="recurringPayment">Recurring payment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendRecurringPaymentCancelledStoreOwnerNotification(RecurringPayment recurringPayment, int languageId)
        {
            if (recurringPayment == null)
                throw new ArgumentNullException(nameof(recurringPayment));

            var store = _storeService.GetStoreById(recurringPayment.InitialOrder.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.RecurringPaymentCancelledStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, recurringPayment.InitialOrder, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, recurringPayment.InitialOrder.User);
            _messageTokenProvider.AddRecurringPaymentTokens(commonTokens, recurringPayment);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a "Recurring payment cancelled" notification to a user
        /// </summary>
        /// <param name="recurringPayment">Recurring payment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendRecurringPaymentCancelledUserNotification(RecurringPayment recurringPayment, int languageId)
        {
            if (recurringPayment == null)
                throw new ArgumentNullException(nameof(recurringPayment));

            var store = _storeService.GetStoreById(recurringPayment.InitialOrder.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.RecurringPaymentCancelledUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, recurringPayment.InitialOrder, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, recurringPayment.InitialOrder.User);
            _messageTokenProvider.AddRecurringPaymentTokens(commonTokens, recurringPayment);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = recurringPayment.InitialOrder.BillingAddress.Email;
                var toName = $"{recurringPayment.InitialOrder.BillingAddress.FirstName} {recurringPayment.InitialOrder.BillingAddress.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a "Recurring payment failed" notification to a user
        /// </summary>
        /// <param name="recurringPayment">Recurring payment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendRecurringPaymentFailedUserNotification(RecurringPayment recurringPayment, int languageId)
        {
            if (recurringPayment == null)
                throw new ArgumentNullException(nameof(recurringPayment));

            var store = _storeService.GetStoreById(recurringPayment.InitialOrder.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.RecurringPaymentFailedUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, recurringPayment.InitialOrder, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, recurringPayment.InitialOrder.User);
            _messageTokenProvider.AddRecurringPaymentTokens(commonTokens, recurringPayment);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = recurringPayment.InitialOrder.BillingAddress.Email;
                var toName = $"{recurringPayment.InitialOrder.BillingAddress.FirstName} {recurringPayment.InitialOrder.BillingAddress.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        #endregion

        #region Newsletter workflow

        /// <summary>
        /// Sends a newsletter subscription activation message
        /// </summary>
        /// <param name="subscription">Newsletter subscription</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewsLetterSubscriptionActivationMessage(NewsLetterSubscription subscription, int languageId)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewsletterSubscriptionActivationMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddNewsLetterSubscriptionTokens(commonTokens, subscription);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, subscription.Email, string.Empty);
            }).ToList();
        }

        /// <summary>
        /// Sends a newsletter subscription deactivation message
        /// </summary>
        /// <param name="subscription">Newsletter subscription</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewsLetterSubscriptionDeactivationMessage(NewsLetterSubscription subscription, int languageId)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewsletterSubscriptionDeactivationMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddNewsLetterSubscriptionTokens(commonTokens, subscription);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, subscription.Email, string.Empty);
            }).ToList();
        }

        #endregion

        #region Send a message to a friend

        /// <summary>
        /// Sends "email a friend" message
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="product">Product instance</param>
        /// <param name="userEmail">User's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendProductEmailAFriendMessage(User user, int languageId,
            Product product, string userEmail, string friendsEmail, string personalMessage)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.EmailAFriendMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);
            _messageTokenProvider.AddProductTokens(commonTokens, product, languageId);
            commonTokens.Add(new Token("EmailAFriend.PersonalMessage", personalMessage, true));
            commonTokens.Add(new Token("EmailAFriend.Email", userEmail));

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, friendsEmail, string.Empty);
            }).ToList();
        }

        /// <summary>
        /// Sends wishlist "email a friend" message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="userEmail">User's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendWishlistEmailAFriendMessage(User user, int languageId,
             string userEmail, string friendsEmail, string personalMessage)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.WishlistToFriendMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);
            commonTokens.Add(new Token("Wishlist.PersonalMessage", personalMessage, true));
            commonTokens.Add(new Token("Wishlist.Email", userEmail));

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, friendsEmail, string.Empty);
            }).ToList();
        }

        #endregion

        #region Return requests

        /// <summary>
        /// Sends 'New Return Request' message to a store owner
        /// </summary>
        /// <param name="returnRequest">Return request</param>
        /// <param name="orderItem">Order item</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewReturnRequestStoreOwnerNotification(ReturnRequest returnRequest, OrderItem orderItem, int languageId)
        {
            if (returnRequest == null)
                throw new ArgumentNullException(nameof(returnRequest));

            var store = _storeService.GetStoreById(orderItem.Order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewReturnRequestStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, orderItem.Order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, returnRequest.User);
            _messageTokenProvider.AddReturnRequestTokens(commonTokens, returnRequest, orderItem);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends 'New Return Request' message to a user
        /// </summary>
        /// <param name="returnRequest">Return request</param>
        /// <param name="orderItem">Order item</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewReturnRequestUserNotification(ReturnRequest returnRequest, OrderItem orderItem, int languageId)
        {
            if (returnRequest == null)
                throw new ArgumentNullException(nameof(returnRequest));

            var store = _storeService.GetStoreById(orderItem.Order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewReturnRequestUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, orderItem.Order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, returnRequest.User);
            _messageTokenProvider.AddReturnRequestTokens(commonTokens, returnRequest, orderItem);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = returnRequest.User.IsGuest() ?
                    orderItem.Order.BillingAddress.Email :
                    returnRequest.User.Email;
                var toName = returnRequest.User.IsGuest() ?
                    orderItem.Order.BillingAddress.FirstName :
                    _userService.GetUserFullName(returnRequest.User);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends 'Return Request status changed' message to a user
        /// </summary>
        /// <param name="returnRequest">Return request</param>
        /// <param name="orderItem">Order item</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendReturnRequestStatusChangedUserNotification(ReturnRequest returnRequest, OrderItem orderItem, int languageId)
        {
            if (returnRequest == null)
                throw new ArgumentNullException(nameof(returnRequest));

            var store = _storeService.GetStoreById(orderItem.Order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ReturnRequestStatusChangedUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddOrderTokens(commonTokens, orderItem.Order, languageId);
            _messageTokenProvider.AddUserTokens(commonTokens, returnRequest.User);
            _messageTokenProvider.AddReturnRequestTokens(commonTokens, returnRequest, orderItem);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = returnRequest.User.IsGuest() ?
                    orderItem.Order.BillingAddress.Email :
                    returnRequest.User.Email;
                var toName = returnRequest.User.IsGuest() ?
                    orderItem.Order.BillingAddress.FirstName :
                    _userService.GetUserFullName(returnRequest.User);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        #endregion

        #region Forum Notifications

        /// <summary>
        /// Sends a forum subscription message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="forumTopic">Forum Topic</param>
        /// <param name="forum">Forum</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewForumTopicMessage(User user, ForumTopic forumTopic, Forum forum, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = _storeContext.CurrentStore;

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewForumTopicMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddForumTopicTokens(commonTokens, forumTopic);
            _messageTokenProvider.AddForumTokens(commonTokens, forumTopic.Forum);
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = user.Email;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a forum subscription message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="forumPost">Forum post</param>
        /// <param name="forumTopic">Forum Topic</param>
        /// <param name="forum">Forum</param>
        /// <param name="friendlyForumTopicPageIndex">Friendly (starts with 1) forum topic page to use for URL generation</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewForumPostMessage(User user, ForumPost forumPost, ForumTopic forumTopic,
            Forum forum, int friendlyForumTopicPageIndex, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = _storeContext.CurrentStore;

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewForumPostMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddForumPostTokens(commonTokens, forumPost);
            _messageTokenProvider.AddForumTopicTokens(commonTokens, forumPost.ForumTopic, friendlyForumTopicPageIndex, forumPost.Id);
            _messageTokenProvider.AddForumTokens(commonTokens, forumPost.ForumTopic.Forum);
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = user.Email;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a private message notification
        /// </summary>
        /// <param name="privateMessage">Private message</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendPrivateMessageNotification(PrivateMessage privateMessage, int languageId)
        {
            if (privateMessage == null)
                throw new ArgumentNullException(nameof(privateMessage));

            var store = _storeService.GetStoreById(privateMessage.StoreId) ?? _storeContext.CurrentStore;

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.PrivateMessageNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddPrivateMessageTokens(commonTokens, privateMessage);
            _messageTokenProvider.AddUserTokens(commonTokens, privateMessage.ToUser);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = privateMessage.ToUser.Email;
                var toName = _userService.GetUserFullName(privateMessage.ToUser);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        #endregion

        #region Misc

        /// <summary>
        /// Sends 'New vendor account submitted' message to a store owner
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="vendor">Vendor</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewVendorAccountApplyStoreOwnerNotification(User user, Vendor vendor, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (vendor == null)
                throw new ArgumentNullException(nameof(vendor));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewVendorAccountApplyStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);
            _messageTokenProvider.AddVendorTokens(commonTokens, vendor);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends 'Vendor information changed' message to a store owner
        /// </summary>
        /// <param name="vendor">Vendor</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendVendorInformationChangeNotification(Vendor vendor, int languageId)
        {
            if (vendor == null)
                throw new ArgumentNullException(nameof(vendor));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.VendorInformationChangeNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddVendorTokens(commonTokens, vendor);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a gift card notification
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendGiftCardNotification(GiftCard giftCard, int languageId)
        {
            if (giftCard == null)
                throw new ArgumentNullException(nameof(giftCard));

            var order = giftCard.PurchasedWithOrderItem?.Order;
            var store = order != null ? _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore : _storeContext.CurrentStore;

            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.GiftCardNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddGiftCardTokens(commonTokens, giftCard);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = giftCard.RecipientEmail;
                var toName = giftCard.RecipientName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a product review notification message to a store owner
        /// </summary>
        /// <param name="productReview">Product review</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendProductReviewNotificationMessage(ProductReview productReview, int languageId)
        {
            if (productReview == null)
                throw new ArgumentNullException(nameof(productReview));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ProductReviewStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddProductReviewTokens(commonTokens, productReview);
            _messageTokenProvider.AddUserTokens(commonTokens, productReview.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a product review reply notification message to a user
        /// </summary>
        /// <param name="productReview">Product review</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendProductReviewReplyUserNotificationMessage(ProductReview productReview, int languageId)
        {
            if (productReview == null)
                throw new ArgumentNullException(nameof(productReview));

            var store = productReview.Store ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ProductReviewReplyUserNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddProductReviewTokens(commonTokens, productReview);
            _messageTokenProvider.AddUserTokens(commonTokens, productReview.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = productReview.User.Email;
                var toName = _userService.GetUserFullName(productReview.User);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a "quantity below" notification to a store owner
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendQuantityBelowStoreOwnerNotification(Product product, int languageId)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.QuantityBelowStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            var commonTokens = new List<Token>();
            _messageTokenProvider.AddProductTokens(commonTokens, product, languageId);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a "quantity below" notification to a store owner
        /// </summary>
        /// <param name="combination">Attribute combination</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendQuantityBelowStoreOwnerNotification(ProductAttributeCombination combination, int languageId)
        {
            if (combination == null)
                throw new ArgumentNullException(nameof(combination));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.QuantityBelowAttributeCombinationStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            var product = combination.Product;

            var commonTokens = new List<Token>();
            _messageTokenProvider.AddProductTokens(commonTokens, product, languageId);
            _messageTokenProvider.AddAttributeCombinationTokens(commonTokens, combination, languageId);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a "new VAT submitted" notification to a store owner
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="vatName">Received VAT name</param>
        /// <param name="vatAddress">Received VAT address</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewVatSubmittedStoreOwnerNotification(User user,
            string vatName, string vatAddress, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewVatSubmittedStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);
            commonTokens.Add(new Token("VatValidationResult.Name", vatName));
            commonTokens.Add(new Token("VatValidationResult.Address", vatAddress));

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a blog comment notification message to a store owner
        /// </summary>
        /// <param name="blogComment">Blog comment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>List of queued email identifiers</returns>
        public virtual IList<int> SendBlogCommentNotificationMessage(BlogComment blogComment, int languageId)
        {
            if (blogComment == null)
                throw new ArgumentNullException(nameof(blogComment));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.BlogCommentNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddBlogCommentTokens(commonTokens, blogComment);
            _messageTokenProvider.AddUserTokens(commonTokens, blogComment.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a news comment notification message to a store owner
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewsCommentNotificationMessage(NewsComment newsComment, int languageId)
        {
            if (newsComment == null)
                throw new ArgumentNullException(nameof(newsComment));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewsCommentNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddNewsCommentTokens(commonTokens, newsComment);
            _messageTokenProvider.AddUserTokens(commonTokens, newsComment.User);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a 'Back in stock' notification message to a user
        /// </summary>
        /// <param name="subscription">Subscription</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendBackInStockNotification(BackInStockSubscription subscription, int languageId)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            var store = _storeService.GetStoreById(subscription.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.BackInStockNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, subscription.User);
            _messageTokenProvider.AddBackInStockTokens(commonTokens, subscription);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var user = subscription.User;
                var toEmail = user.Email;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends "contact us" message
        /// </summary>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="subject">Email subject. Pass null if you want a message template subject to be used.</param>
        /// <param name="body">Email body</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendContactUsMessage(int languageId, string senderEmail,
            string senderName, string subject, string body)
        {
            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ContactUsMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>
            {
                new Token("ContactUs.SenderEmail", senderEmail),
                new Token("ContactUs.SenderName", senderName),
                new Token("ContactUs.Body", body, true)
            };

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                string fromEmail;
                string fromName;
                //required for some SMTP servers
                if (_commonSettings.UseSystemEmailForContactUsForm)
                {
                    fromEmail = emailAccount.Email;
                    fromName = emailAccount.DisplayName;
                    body = $"<strong>From</strong>: {WebUtility.HtmlEncode(senderName)} - {WebUtility.HtmlEncode(senderEmail)}<br /><br />{body}";
                }
                else
                {
                    fromEmail = senderEmail;
                    fromName = senderName;
                }

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName,
                    fromEmail: fromEmail,
                    fromName: fromName,
                    subject: subject,
                    replyToEmailAddress: senderEmail,
                    replyToName: senderName);
            }).ToList();
        }

        /// <summary>
        /// Sends "contact vendor" message
        /// </summary>
        /// <param name="vendor">Vendor</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="subject">Email subject. Pass null if you want a message template subject to be used.</param>
        /// <param name="body">Email body</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendContactVendorMessage(Vendor vendor, int languageId, string senderEmail,
            string senderName, string subject, string body)
        {
            if (vendor == null)
                throw new ArgumentNullException(nameof(vendor));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ContactVendorMessage, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>
            {
                new Token("ContactUs.SenderEmail", senderEmail),
                new Token("ContactUs.SenderName", senderName),
                new Token("ContactUs.Body", body, true)
            };

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                string fromEmail;
                string fromName;
                //required for some SMTP servers
                if (_commonSettings.UseSystemEmailForContactUsForm)
                {
                    fromEmail = emailAccount.Email;
                    fromName = emailAccount.DisplayName;
                    body = $"<strong>From</strong>: {WebUtility.HtmlEncode(senderName)} - {WebUtility.HtmlEncode(senderEmail)}<br /><br />{body}";
                }
                else
                {
                    fromEmail = senderEmail;
                    fromName = senderName;
                }

                var tokens = new List<Token>(commonTokens);
                _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = vendor.Email;
                var toName = vendor.Name;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName,
                    fromEmail: fromEmail,
                    fromName: fromName,
                    subject: subject,
                    replyToEmailAddress: senderEmail,
                    replyToName: senderName);
            }).ToList();
        }

        /// <summary>
        /// Sends a test email
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <param name="sendToEmail">Send to email</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendTestEmail(int messageTemplateId, string sendToEmail, List<Token> tokens, int languageId)
        {
            var messageTemplate = _messageTemplateService.GetMessageTemplateById(messageTemplateId);
            if (messageTemplate == null)
                throw new ArgumentException("Template cannot be loaded");

            //email account
            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

            return SendNotification(messageTemplate, emailAccount, languageId, tokens, sendToEmail, null);
        }

        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="toEmailAddress">Recipient email address</param>
        /// <param name="toName">Recipient name</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name</param>
        /// <param name="replyToEmailAddress">"Reply to" email</param>
        /// <param name="replyToName">"Reply to" name</param>
        /// <param name="fromEmail">Sender email. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="fromName">Sender name. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="subject">Subject. If specified, then it overrides subject of a message template</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendNotification(MessageTemplate messageTemplate,
            EmailAccount emailAccount, int languageId, IEnumerable<Token> tokens,
            string toEmailAddress, string toName,
            string attachmentFilePath = null, string attachmentFileName = null,
            string replyToEmailAddress = null, string replyToName = null,
            string fromEmail = null, string fromName = null, string subject = null)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            if (emailAccount == null)
                throw new ArgumentNullException(nameof(emailAccount));

            //retrieve localized message template data
            var bcc = _localizationService.GetLocalized(messageTemplate, mt => mt.BccEmailAddresses, languageId);
            if (string.IsNullOrEmpty(subject))
                subject = _localizationService.GetLocalized(messageTemplate, mt => mt.Subject, languageId);
            var body = _localizationService.GetLocalized(messageTemplate, mt => mt.Body, languageId);

            //Replace subject and body tokens 
            var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);

            //limit name length
            toName = CommonHelper.EnsureMaximumLength(toName, 300);

            var email = new QueuedEmail
            {
                Priority = QueuedEmailPriority.High,
                From = !string.IsNullOrEmpty(fromEmail) ? fromEmail : emailAccount.Email,
                FromName = !string.IsNullOrEmpty(fromName) ? fromName : emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                ReplyTo = replyToEmailAddress,
                ReplyToName = replyToName,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentFilePath = attachmentFilePath,
                AttachmentFileName = attachmentFileName,
                AttachedDownloadId = messageTemplate.AttachedDownloadId,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id,
                DontSendBeforeDateUtc = !messageTemplate.DelayBeforeSend.HasValue ? null
                    : (DateTime?)(DateTime.UtcNow + TimeSpan.FromHours(messageTemplate.DelayPeriod.ToHours(messageTemplate.DelayBeforeSend.Value)))
            };

            _queuedEmailService.InsertQueuedEmail(email);
            return email.Id;
        }

        #endregion

        #endregion
    }
}