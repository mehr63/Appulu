using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Moq;
using Appulu.Core;
using Appulu.Core.Configuration;
using Appulu.Core.Infrastructure;
using Appulu.Core.Plugins;
using Appulu.Services.Events;
using NUnit.Framework;

namespace Appulu.Web.MVC.Tests.Events
{
    [TestFixture]
    public class EventsTests
    {
        private IEventPublisher _eventPublisher;

        [OneTimeSetUp]
        public void SetUp()
        {
            var hostingEnvironment = new Mock<IHostingEnvironment>();
            hostingEnvironment.Setup(x => x.ContentRootPath).Returns(System.Reflection.Assembly.GetExecutingAssembly().Location);
            hostingEnvironment.Setup(x => x.WebRootPath).Returns(System.IO.Directory.GetCurrentDirectory());
            CommonHelper.DefaultFileProvider = new AppFileProvider(hostingEnvironment.Object);
            PluginManager.Initialize(new ApplicationPartManager(), new AppConfig());

            var subscriptionService = new Mock<ISubscriptionService>();
            var consumers = new List<IConsumer<DateTime>> {new DateTimeConsumer()};
            subscriptionService.Setup(c => c.GetSubscriptions<DateTime>()).Returns(consumers);
            _eventPublisher = new EventPublisher(subscriptionService.Object);
        }

        [Test]
        public void Can_publish_event()
        {
            var oldDateTime = DateTime.Now.Subtract(TimeSpan.FromDays(7));
            DateTimeConsumer.DateTime = oldDateTime;

            var newDateTime = DateTime.Now.Subtract(TimeSpan.FromDays(5));
            _eventPublisher.Publish(newDateTime);
            Assert.AreEqual(DateTimeConsumer.DateTime, newDateTime);
        }
    }
}