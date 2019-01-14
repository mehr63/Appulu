using System.Collections.Generic;
using System.Linq;
using Moq;
using Appulu.Core.Caching;
using Appulu.Core.Data;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Events;
using Appulu.Tests;
using NUnit.Framework;

namespace Appulu.Core.Tests.Domain.Users
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Can_check_IsInUserRole()
        {
            var user = new User();

            user.UserRoles.Add(new UserRole
            {
                Active = true,
                Name = "Test name 1",
                SystemName = "Test system name 1"
            });
            user.UserRoles.Add(new UserRole
            {
                Active = false,
                Name = "Test name 2",
                SystemName = "Test system name 2"
            });
            user.IsInUserRole("Test system name 1", false).ShouldBeTrue();
            user.IsInUserRole("Test system name 1").ShouldBeTrue();

            user.IsInUserRole("Test system name 2", false).ShouldBeTrue();
            user.IsInUserRole("Test system name 2").ShouldBeFalse();

            user.IsInUserRole("Test system name 3", false).ShouldBeFalse();
            user.IsInUserRole("Test system name 3").ShouldBeFalse();
        }
        [Test]
        public void Can_check_whether_user_is_admin()
        {
            var user = new User();

            user.UserRoles.Add(new UserRole
            {
                Active = true,
                Name = "Registered",
                SystemName = AppUserDefaults.RegisteredRoleName
            });
            user.UserRoles.Add(new UserRole
            {
                Active = true,
                Name = "Guests",
                SystemName = AppUserDefaults.GuestsRoleName
            });

            user.IsAdmin().ShouldBeFalse();

            user.UserRoles.Add(
                new UserRole
                {
                    Active = true,
                    Name = "Administrators",
                    SystemName = AppUserDefaults.AdministratorsRoleName
                });
            user.IsAdmin().ShouldBeTrue();
        }
        [Test]
        public void Can_check_whether_user_is_forum_moderator()
        {
            var user = new TestUser();

            user.UserRoles.Add(new UserRole
            {
                Active = true,
                Name = "Registered",
                SystemName = AppUserDefaults.RegisteredRoleName
            });
            user.UserRoles.Add(new UserRole
            {
                Active = true,
                Name = "Guests",
                SystemName = AppUserDefaults.GuestsRoleName
            });

            user.IsForumModerator().ShouldBeFalse();

            user.UserRoles.Add(
                new UserRole
                {
                    Active = true,
                    Name = "ForumModerators",
                    SystemName = AppUserDefaults.ForumModeratorsRoleName
                });
            user.IsForumModerator().ShouldBeTrue();
        }
        [Test]
        public void Can_check_whether_user_is_guest()
        {
            var user = new User();

            user.UserRoles.Add(new UserRole
            {
                Active = true,
                Name = "Registered",
                SystemName = AppUserDefaults.RegisteredRoleName
            });

            user.UserRoles.Add(new UserRole
            {
                Active = true,
                Name = "Administrators",
                SystemName = AppUserDefaults.AdministratorsRoleName
            });

            user.IsGuest().ShouldBeFalse();

            user.UserRoles.Add(
                new UserRole
                {
                    Active = true,
                    Name = "Guests",
                    SystemName = AppUserDefaults.GuestsRoleName

                }
                );
            user.IsGuest().ShouldBeTrue();
        }
        [Test]
        public void Can_check_whether_user_is_registered()
        {
            var user = new User();
            user.UserRoles.Add(new UserRole
            {
                Active = true,
                Name = "Administrators",
                SystemName = AppUserDefaults.AdministratorsRoleName
            });

            user.UserRoles.Add(new UserRole
            {
                Active = true,
                Name = "Guests",
                SystemName = AppUserDefaults.GuestsRoleName
            });

            user.IsRegistered().ShouldBeFalse();

            user.UserRoles.Add(
                new UserRole
                {
                    Active = true,
                    Name = "Registered",
                    SystemName = AppUserDefaults.RegisteredRoleName
                });
            user.IsRegistered().ShouldBeTrue();
        }
       
        [Test]
        public void Can_remove_address_assigned_as_billing_address()
        {
            var _userRepo = new Mock<IRepository<User>>();
            var _userUserRoleMappingRepo = new Mock<IRepository<UserUserRoleMapping>>();
            var _userPasswordRepo = new Mock<IRepository<UserPassword>>();
            var _genericAttributeRepo = new Mock<IRepository<GenericAttribute>>();
            var _genericAttributeService = new Mock<IGenericAttributeService>();
            var _eventPublisher = new Mock<IEventPublisher>();
            var _userRoleRepo = new Mock<IRepository<UserRole>>();

            var _userService = new UserService(new UserSettings(), 
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

            var user = new TestUser();
            var address = new Address { Id = 1 };

            user.AddAddresses(address);
            user.BillingAddress  = address;

            user.BillingAddress.ShouldBeTheSameAs(user.Addresses.First());

            _userService.RemoveUserAddress(user, address);
            user.Addresses.Count.ShouldEqual(0);
            user.BillingAddress.ShouldBeNull();
        }

        [Test]
        public void Can_add_rewardPointsHistoryEntry()
        {
            //TODO temporary disabled until we can inject (not resolve using DI) "RewardPointsSettings" into "LimitPerStore" method of UserExtensions

            //var user = new User();
            //user.AddRewardPointsHistoryEntry(1, 0, "Points for registration");

            //user.RewardPointsHistory.Count.ShouldEqual(1);
            //user.RewardPointsHistory.First().Points.ShouldEqual(1);
        }

        [Test]
        public void Can_get_rewardPointsHistoryBalance()
        {
            //TODO temporary disabled until we can inject (not resolve using DI) "RewardPointsSettings" into "LimitPerStore" method of UserExtensions

            //var user = new User();
            //user.AddRewardPointsHistoryEntry(1, 0, "Points for registration");

            //user.GetRewardPointsBalance(0).ShouldEqual(1);
        }

        class TestUser : User
        {
            public TestUser()
            {
                _userAddressMappings = new List<UserAddressMapping>();
            }

            public void AddAddresses(Address address)
            {
                _userAddressMappings.Add(new UserAddressMapping{Address = address, AddressId = address.Id});
            }
        }
    }
}
