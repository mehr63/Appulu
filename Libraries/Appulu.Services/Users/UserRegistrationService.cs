using System;
using System.Linq;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Services.Common;
using Appulu.Services.Events;
using Appulu.Services.Localization;
using Appulu.Services.Messages;
using Appulu.Services.Orders;
using Appulu.Services.Security;
using Appulu.Services.Stores;

namespace Appulu.Services.Users
{
    /// <summary>
    /// User registration service
    /// </summary>
    public partial class UserRegistrationService : IUserRegistrationService
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IRewardPointService _rewardPointService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly RewardPointsSettings _rewardPointsSettings;

        #endregion

        #region Ctor

        public UserRegistrationService(UserSettings userSettings,
            IUserService userService,
            IEncryptionService encryptionService,
            IEventPublisher eventPublisher,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IRewardPointService rewardPointService,
            IStoreService storeService,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            RewardPointsSettings rewardPointsSettings)
        {
            this._userSettings = userSettings;
            this._userService = userService;
            this._encryptionService = encryptionService;
            this._eventPublisher = eventPublisher;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._rewardPointService = rewardPointService;
            this._storeService = storeService;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
            this._rewardPointsSettings = rewardPointsSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether the entered password matches with a saved one
        /// </summary>
        /// <param name="userPassword">User password</param>
        /// <param name="enteredPassword">The entered password</param>
        /// <returns>True if passwords match; otherwise false</returns>
        protected bool PasswordsMatch(UserPassword userPassword, string enteredPassword)
        {
            if (userPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (userPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, userPassword.PasswordSalt, _userSettings.HashedPasswordFormat);
                    break;
            }

            if (userPassword.Password == null)
                return false;

            return userPassword.Password.Equals(savedPassword);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual UserLoginResults ValidateUser(string usernameOrEmail, string password)
        {
            var user = _userSettings.UsernamesEnabled ?
                _userService.GetUserByUsername(usernameOrEmail) :
                _userService.GetUserByEmail(usernameOrEmail);

            if (user == null)
                return UserLoginResults.UserNotExist;
            if (user.Deleted)
                return UserLoginResults.Deleted;
            if (!user.Active)
                return UserLoginResults.NotActive;
            //only registered can login
            if (!user.IsRegistered())
                return UserLoginResults.NotRegistered;
            //check whether a user is locked out
            if (user.CannotLoginUntilDateUtc.HasValue && user.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return UserLoginResults.LockedOut;

            if (!PasswordsMatch(_userService.GetCurrentPassword(user.Id), password))
            {
                //wrong password
                user.FailedLoginAttempts++;
                if (_userSettings.FailedPasswordAllowedAttempts > 0 &&
                    user.FailedLoginAttempts >= _userSettings.FailedPasswordAllowedAttempts)
                {
                    //lock out
                    user.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_userSettings.FailedPasswordLockoutMinutes);
                    //reset the counter
                    user.FailedLoginAttempts = 0;
                }

                _userService.UpdateUser(user);

                return UserLoginResults.WrongPassword;
            }

            //update login details
            user.FailedLoginAttempts = 0;
            user.CannotLoginUntilDateUtc = null;
            user.RequireReLogin = false;
            user.LastLoginDateUtc = DateTime.UtcNow;
            _userService.UpdateUser(user);

            return UserLoginResults.Successful;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual UserRegistrationResult RegisterUser(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.User == null)
                throw new ArgumentException("Can't load current user");

            var result = new UserRegistrationResult();
            if (request.User.IsSearchEngineAccount())
            {
                result.AddError("Search engine can't be registered");
                return result;
            }

            if (request.User.IsBackgroundTaskAccount())
            {
                result.AddError("Background task account can't be registered");
                return result;
            }

            if (request.User.IsRegistered())
            {
                result.AddError("Current user is already registered");
                return result;
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.EmailIsNotProvided"));
                return result;
            }

            if (!CommonHelper.IsValidEmail(request.Email))
            {
                result.AddError(_localizationService.GetResource("Common.WrongEmail"));
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.PasswordIsNotProvided"));
                return result;
            }

            if (_userSettings.UsernamesEnabled && string.IsNullOrEmpty(request.Username))
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.UsernameIsNotProvided"));
                return result;
            }

            //validate unique user
            if (_userService.GetUserByEmail(request.Email) != null)
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.EmailAlreadyExists"));
                return result;
            }

            if (_userSettings.UsernamesEnabled && _userService.GetUserByUsername(request.Username) != null)
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.UsernameAlreadyExists"));
                return result;
            }

            //at this point request is valid
            request.User.Username = request.Username;
            request.User.Email = request.Email;

            var userPassword = new UserPassword
            {
                User = request.User,
                PasswordFormat = request.PasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.Password;
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.Password);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(AppUserServiceDefaults.PasswordSaltKeySize);
                    userPassword.PasswordSalt = saltKey;
                    userPassword.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _userSettings.HashedPasswordFormat);
                    break;
            }

            _userService.InsertUserPassword(userPassword);

            request.User.Active = request.IsApproved;

            //add to 'Registered' role
            var registeredRole = _userService.GetUserRoleBySystemName(AppUserDefaults.RegisteredRoleName);
            if (registeredRole == null)
                throw new AppException("'Registered' role could not be loaded");
            //request.User.UserRoles.Add(registeredRole);
            request.User.UserUserRoleMappings.Add(new UserUserRoleMapping { UserRole = registeredRole });
            //remove from 'Guests' role
            var guestRole = request.User.UserRoles.FirstOrDefault(cr => cr.SystemName == AppUserDefaults.GuestsRoleName);
            if (guestRole != null)
            {
                //request.User.UserRoles.Remove(guestRole);
                request.User.UserUserRoleMappings
                    .Remove(request.User.UserUserRoleMappings.FirstOrDefault(mapping => mapping.UserRoleId == guestRole.Id));
            }

            //add reward points for user registration (if enabled)
            if (_rewardPointsSettings.Enabled && _rewardPointsSettings.PointsForRegistration > 0)
            {
                var endDate = _rewardPointsSettings.RegistrationPointsValidity > 0
                    ? (DateTime?)DateTime.UtcNow.AddDays(_rewardPointsSettings.RegistrationPointsValidity.Value) : null;
                _rewardPointService.AddRewardPointsHistoryEntry(request.User, _rewardPointsSettings.PointsForRegistration,
                    request.StoreId, _localizationService.GetResource("RewardPoints.Message.EarnedForRegistration"), endDate: endDate);
            }

            _userService.UpdateUser(request.User);

            return result;
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new ChangePasswordResult();
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.EmailIsNotProvided"));
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.PasswordIsNotProvided"));
                return result;
            }

            var user = _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.EmailNotFound"));
                return result;
            }
          
            //request isn't valid
            if (request.ValidateRequest && !PasswordsMatch(_userService.GetCurrentPassword(user.Id), request.OldPassword))
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.OldPasswordDoesntMatch"));
                return result;
            }

            //check for duplicates
            if (_userSettings.UnduplicatedPasswordsNumber > 0)
            {
                //get some of previous passwords
                var previousPasswords = _userService.GetUserPasswords(user.Id, passwordsToReturn: _userSettings.UnduplicatedPasswordsNumber);

                var newPasswordMatchesWithPrevious = previousPasswords.Any(password => PasswordsMatch(password, request.NewPassword));
                if (newPasswordMatchesWithPrevious)
                {
                    result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.PasswordMatchesWithPrevious"));
                    return result;
                }
            }

            //at this point request is valid
            var userPassword = new UserPassword
            {
                User = user,
                PasswordFormat = request.NewPasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.NewPasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.NewPassword;
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.NewPassword);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(AppUserServiceDefaults.PasswordSaltKeySize);
                    userPassword.PasswordSalt = saltKey;
                    userPassword.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey, _userSettings.HashedPasswordFormat);
                    break;
            }

            _userService.InsertUserPassword(userPassword);

            //publish event
            _eventPublisher.Publish(new UserPasswordChangedEvent(userPassword));

            return result;
        }

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newEmail">New email</param>
        /// <param name="requireValidation">Require validation of new email address</param>
        public virtual void SetEmail(User user, string newEmail, bool requireValidation)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (newEmail == null)
                throw new AppException("Email cannot be null");

            newEmail = newEmail.Trim();
            var oldEmail = user.Email;

            if (!CommonHelper.IsValidEmail(newEmail))
                throw new AppException(_localizationService.GetResource("Account.EmailUsernameErrors.NewEmailIsNotValid"));

            if (newEmail.Length > 100)
                throw new AppException(_localizationService.GetResource("Account.EmailUsernameErrors.EmailTooLong"));

            var user2 = _userService.GetUserByEmail(newEmail);
            if (user2 != null && user.Id != user2.Id)
                throw new AppException(_localizationService.GetResource("Account.EmailUsernameErrors.EmailAlreadyExists"));

            if (requireValidation)
            {
                //re-validate email
                user.EmailToRevalidate = newEmail;
                _userService.UpdateUser(user);

                //email re-validation message
                _genericAttributeService.SaveAttribute(user, AppUserDefaults.EmailRevalidationTokenAttribute, Guid.NewGuid().ToString());
                _workflowMessageService.SendUserEmailRevalidationMessage(user, _workContext.WorkingLanguage.Id);
            }
            else
            {
                user.Email = newEmail;
                _userService.UpdateUser(user);
                
                if (string.IsNullOrEmpty(oldEmail) || oldEmail.Equals(newEmail, StringComparison.InvariantCultureIgnoreCase)) 
                    return;

                //update newsletter subscription (if required)
                foreach (var store in _storeService.GetAllStores())
                {
                    var subscriptionOld = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(oldEmail, store.Id);
                    
                    if (subscriptionOld == null) 
                        continue;

                    subscriptionOld.Email = newEmail;
                    _newsLetterSubscriptionService.UpdateNewsLetterSubscription(subscriptionOld);
                }
            }
        }

        /// <summary>
        /// Sets a user username
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newUsername">New Username</param>
        public virtual void SetUsername(User user, string newUsername)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!_userSettings.UsernamesEnabled)
                throw new AppException("Usernames are disabled");

            newUsername = newUsername.Trim();

            if (newUsername.Length > AppUserServiceDefaults.UserUsernameLength)
                throw new AppException(_localizationService.GetResource("Account.EmailUsernameErrors.UsernameTooLong"));

            var user2 = _userService.GetUserByUsername(newUsername);
            if (user2 != null && user.Id != user2.Id)
                throw new AppException(_localizationService.GetResource("Account.EmailUsernameErrors.UsernameAlreadyExists"));

            user.Username = newUsername;
            _userService.UpdateUser(user);
        }

        #endregion
    }
}