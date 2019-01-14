using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Appulu.Core;
using Appulu.Core.Caching;
using Appulu.Core.Data;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Security;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Events;
using Appulu.Services.Localization;
using Appulu.Services.Messages;
using Appulu.Services.Orders;
using Appulu.Services.Security;
using Appulu.Services.Stores;
using Appulu.Tests;
using NUnit.Framework;

namespace Appulu.Services.Tests.Users
{
    [TestFixture]
    public class UserRegistrationServiceTests : ServiceTest
    {
        private UserSettings _userSettings;
        private SecuritySettings _securitySettings;
        private RewardPointsSettings _rewardPointsSettings;
        private EncryptionService _encryptionService;
        private Mock<IRepository<User>> _userRepo;
        private Mock<IRepository<UserPassword>> _userPasswordRepo;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IStoreService> _storeService;
        private Mock<IRepository<UserRole>> _userRoleRepo;
        private Mock<IRepository<GenericAttribute>> _genericAttributeRepo;
        private Mock<IGenericAttributeService> _genericAttributeService;
        private Mock<INewsLetterSubscriptionService> _newsLetterSubscriptionService;
        private Mock<IRewardPointService> _rewardPointService;
        private Mock<ILocalizationService> _localizationService;
        private Mock<IWorkContext> _workContext;
        private Mock<IWorkflowMessageService> _workflowMessageService;
        private UserService _userService;
        private UserRegistrationService _userRegistrationService;
        private Mock<IRepository<UserUserRoleMapping>> _userUserRoleMappingRepo;

        [SetUp]
        public new void SetUp()
        {
            _userSettings = new UserSettings
            {
                UnduplicatedPasswordsNumber = 1,
                HashedPasswordFormat = "SHA512"
            };
            _securitySettings = new SecuritySettings
            {
                EncryptionKey = "273ece6f97dd844d"
            };
            _rewardPointsSettings = new RewardPointsSettings
            {
                Enabled = false
            };

            _encryptionService = new EncryptionService(_securitySettings);
            _userRepo = new Mock<IRepository<User>>();

            var user1 = new User
            {
                Id = 1,
                Username = "a@b.com",
                Email = "a@b.com",
                Active = true
            };
            AddUserToRegisteredRole(user1);

            var user2 = new User
            {
                Id = 2,
                Username = "test@test.com",
                Email = "test@test.com",
                Active = true
            };
            AddUserToRegisteredRole(user2);

            var user3 = new User
            {
                Id = 3,
                Username = "user@test.com",
                Email = "user@test.com",
                Active = true
            };
            AddUserToRegisteredRole(user3);

            var user4 = new User
            {
                Id = 4,
                Username = "registered@test.com",
                Email = "registered@test.com",
                Active = true
            };
            AddUserToRegisteredRole(user4);

            var user5 = new User
            {
                Id = 5,
                Username = "notregistered@test.com",
                Email = "notregistered@test.com",
                Active = true
            };
            _userRepo.Setup(x => x.Table).Returns(new List<User> { user1, user2, user3, user4, user5 }.AsQueryable());

            _userPasswordRepo = new Mock<IRepository<UserPassword>>();
            var saltKey = _encryptionService.CreateSaltKey(5);
            var password = _encryptionService.CreatePasswordHash("password", saltKey, "SHA512");
            var password1 = new UserPassword
            {
                UserId = user1.Id,
                PasswordFormat = PasswordFormat.Hashed,
                PasswordSalt = saltKey,
                Password = password,
                CreatedOnUtc = DateTime.UtcNow
            };
            var password2 = new UserPassword
            {
                UserId = user2.Id,
                PasswordFormat = PasswordFormat.Clear,
                Password = "password",
                CreatedOnUtc = DateTime.UtcNow
            };
            var password3 = new UserPassword
            {
                UserId = user3.Id,
                PasswordFormat = PasswordFormat.Encrypted,
                Password = _encryptionService.EncryptText("password"),
                CreatedOnUtc = DateTime.UtcNow
            };
            var password4 = new UserPassword
            {
                UserId = user4.Id,
                PasswordFormat = PasswordFormat.Clear,
                Password = "password",
                CreatedOnUtc = DateTime.UtcNow
            };
            var password5 = new UserPassword
            {
                UserId = user5.Id,
                PasswordFormat = PasswordFormat.Clear,
                Password = "password",
                CreatedOnUtc = DateTime.UtcNow
            };
            _userPasswordRepo.Setup(x => x.Table).Returns(new[] { password1, password2, password3, password4, password5 }.AsQueryable());

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher.Setup(x => x.Publish(It.IsAny<object>()));

            _storeService = new Mock<IStoreService>();
            _userRoleRepo = new Mock<IRepository<UserRole>>();
            _genericAttributeRepo = new Mock<IRepository<GenericAttribute>>();
            _genericAttributeService = new Mock<IGenericAttributeService>();
            _newsLetterSubscriptionService = new Mock<INewsLetterSubscriptionService>();
            _rewardPointService = new Mock<IRewardPointService>();

            _localizationService = new Mock<ILocalizationService>();
            _workContext = new Mock<IWorkContext>();
            _workflowMessageService = new Mock<IWorkflowMessageService>();
            _userUserRoleMappingRepo = new Mock<IRepository<UserUserRoleMapping>>();
            
             _userService = new UserService(new UserSettings(), 
                new AppNullCache(), 
                null,
                null,
                _eventPublisher.Object,
                _genericAttributeService.Object,
                _userRepo.Object,
                _userUserRoleMappingRepo.Object,
                _userPasswordRepo.Object,
                _userRoleRepo.Object,
                _genericAttributeRepo.Object,
                null);

            _userRegistrationService = new UserRegistrationService(_userSettings,
                _userService,
                _encryptionService,
                _eventPublisher.Object,
                _genericAttributeService.Object,
                _localizationService.Object,
                _newsLetterSubscriptionService.Object,
                _rewardPointService.Object,
                _storeService.Object,
                _workContext.Object,
                _workflowMessageService.Object,
                _rewardPointsSettings);
        }

        //[Test]
        //public void Can_register_a_user() 
        //{
        //    var registrationRequest = CreateUserRegistrationRequest();
        //    var result = _userService.RegisterUser(registrationRequest);

        //    result.Success.ShouldBeTrue();
        //}

        //[Test]
        //public void Can_not_have_duplicate_usernames_or_emails() 
        //{
        //    var registrationRequest = CreateUserRegistrationRequest();
        //    registrationRequest.Username = "a@b.com";
        //    registrationRequest.Email = "a@b.com";

        //    var userService = new UserService(_encryptionService, _userRepo, _userSettings);
        //    var result = userService.RegisterUser(registrationRequest);

        //    result.Success.ShouldBeFalse();
        //    result.Errors.Count.ShouldEqual(1);
        //}

        [Test]
        public void Ensure_only_registered_users_can_login()
        {
            var result = _userRegistrationService.ValidateUser("registered@test.com", "password");
            result.ShouldEqual(UserLoginResults.Successful);

            result = _userRegistrationService.ValidateUser("notregistered@test.com", "password");
            result.ShouldEqual(UserLoginResults.NotRegistered);
        }

        [Test]
        public void Can_validate_a_hashed_password()
        {
            var result = _userRegistrationService.ValidateUser("a@b.com", "password");
            result.ShouldEqual(UserLoginResults.Successful);
        }

        [Test]
        public void Can_validate_a_clear_password()
        {
            var result = _userRegistrationService.ValidateUser("test@test.com", "password");
            result.ShouldEqual(UserLoginResults.Successful);
        }

        [Test]
        public void Can_validate_an_encrypted_password()
        {
            var result = _userRegistrationService.ValidateUser("user@test.com", "password");
            result.ShouldEqual(UserLoginResults.Successful);
        }

        private void AddUserToRegisteredRole(User user)
        {
            user.UserRoles.Add(new UserRole
            {
                Active = true,
                IsSystemRole = true,
                SystemName = AppUserDefaults.RegisteredRoleName
            });
        }

        [Test]
        public void Can_change_password()
        {
            var request = new ChangePasswordRequest("registered@test.com", true, PasswordFormat.Clear, "password", "password");
            var result = _userRegistrationService.ChangePassword(request);
            result.Success.ShouldEqual(false);

            request = new ChangePasswordRequest("registered@test.com", true, PasswordFormat.Hashed, "newpassword", "password");
            result = _userRegistrationService.ChangePassword(request);
            result.Success.ShouldEqual(true);

            //request = new ChangePasswordRequest("registered@test.com", true, PasswordFormat.Encrypted, "password", "newpassword");
            //result = _userRegistrationService.ChangePassword(request);
            //result.Success.ShouldEqual(true);
        }
    }
}
