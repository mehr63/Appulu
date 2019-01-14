using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Appulu.Core;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Stores;
using Appulu.Services.Common;
using Appulu.Services.Configuration;
using Appulu.Services.Helpers;
using Appulu.Tests;
using NUnit.Framework;

namespace Appulu.Services.Tests.Helpers
{
    [TestFixture]
    public class DateTimeHelperTests : ServiceTest
    {
        private Mock<IWorkContext> _workContext;
        private Mock<IStoreContext> _storeContext;
        private Mock<IGenericAttributeService> _genericAttributeService;
        private Mock<ISettingService> _settingService;
        private DateTimeSettings _dateTimeSettings;
        private IDateTimeHelper _dateTimeHelper;
        private Store _store;

        [SetUp]
        public new void SetUp()
        {
            _genericAttributeService = new Mock<IGenericAttributeService>();
            _settingService = new Mock<ISettingService>();

            _workContext = new Mock<IWorkContext>();

            _store = new Store { Id = 1 };
            _storeContext = new Mock<IStoreContext>();
            _storeContext.Setup(x => x.CurrentStore).Returns(_store);

            _dateTimeSettings = new DateTimeSettings
            {
                AllowUsersToSetTimeZone = false,
                DefaultStoreTimeZoneId = ""
            };

            _dateTimeHelper = new DateTimeHelper(_dateTimeSettings, _genericAttributeService.Object, 
                _settingService.Object, _workContext.Object);
        }

        [Test]
        public void Can_find_systemTimeZone_by_id()
        {
            var timeZones = _dateTimeHelper.FindTimeZoneById("E. Europe Standard Time");
            timeZones.ShouldNotBeNull();
            timeZones.Id.ShouldEqual("E. Europe Standard Time");
        }

        [Test]
        public void Can_get_all_systemTimeZones()
        {
            var systemTimeZones = _dateTimeHelper.GetSystemTimeZones();
            systemTimeZones.ShouldNotBeNull();
            systemTimeZones.Any().ShouldBeTrue();
        }

        [Test]
        public void Can_get_user_timeZone_with_customTimeZones_enabled()
        {
            _dateTimeSettings.AllowUsersToSetTimeZone = true;
            _dateTimeSettings.DefaultStoreTimeZoneId = "E. Europe Standard Time"; //(GMT+02:00) Minsk;

            var user = new User
            {
                Id = 10
            };

            _genericAttributeService.Setup(x => x.GetAttribute<string>(user, AppUserDefaults.TimeZoneIdAttribute, 0))
                .Returns("Russian Standard Time" /*(GMT+03:00) Moscow, St. Petersburg, Volgograd*/);

            var timeZone = _dateTimeHelper.GetUserTimeZone(user);
            timeZone.ShouldNotBeNull();
            timeZone.Id.ShouldEqual("Russian Standard Time");
        }

        [Test]
        public void Can_get_user_timeZone_with_customTimeZones_disabled()
        {
            _dateTimeSettings.AllowUsersToSetTimeZone = false;
            _dateTimeSettings.DefaultStoreTimeZoneId = "E. Europe Standard Time"; //(GMT+02:00) Minsk;

            var user = new User
            {
                Id = 10
            };

            _genericAttributeService.Setup(x => x.GetAttributesForEntity(user.Id, "User"))
                .Returns(new List<GenericAttribute>
                            {
                                new GenericAttribute
                                    {
                                        StoreId = 0,
                                        EntityId = user.Id,
                                        Key = AppUserDefaults.TimeZoneIdAttribute,
                                        KeyGroup = "User",
                                        Value = "Russian Standard Time" //(GMT+03:00) Moscow, St. Petersburg, Volgograd
                                    }
                            });

            var timeZone = _dateTimeHelper.GetUserTimeZone(user);
            timeZone.ShouldNotBeNull();
            timeZone.Id.ShouldEqual("E. Europe Standard Time");
        }

        [Test]
        public void Can_convert_dateTime_to_userTime()
        {
            var sourceDateTime = TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"); //(GMT+02:00) Minsk;
            sourceDateTime.ShouldNotBeNull();

            var destinationDateTime = TimeZoneInfo.FindSystemTimeZoneById("North Asia Standard Time"); //(GMT+07:00) Krasnoyarsk;
            destinationDateTime.ShouldNotBeNull();

            //summer time
            _dateTimeHelper.ConvertToUserTime(new DateTime(2010, 06, 01, 0, 0, 0), sourceDateTime, destinationDateTime)
                .ShouldEqual(new DateTime(2010, 06, 01, 5, 0, 0));

            //winter time
            _dateTimeHelper.ConvertToUserTime(new DateTime(2010, 01, 01, 0, 0, 0), sourceDateTime, destinationDateTime)
                .ShouldEqual(new DateTime(2010, 01, 01, 5, 0, 0));
        }

        [Test]
        public void Can_convert_dateTime_to_utc_dateTime()
        {
            var sourceDateTime = TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"); //(GMT+02:00) Minsk;
            sourceDateTime.ShouldNotBeNull();

            //summer time
            var dateTime1 = new DateTime(2010, 06, 01, 0, 0, 0);
            var convertedDateTime1 = _dateTimeHelper.ConvertToUtcTime(dateTime1, sourceDateTime);
            convertedDateTime1.ShouldEqual(new DateTime(2010, 05, 31, 21, 0, 0));

            //winter time
            var dateTime2 = new DateTime(2010, 01, 01, 0, 0, 0);
            var convertedDateTime2 = _dateTimeHelper.ConvertToUtcTime(dateTime2, sourceDateTime);
            convertedDateTime2.ShouldEqual(new DateTime(2009, 12, 31, 22, 0, 0));
        }
    }
}
