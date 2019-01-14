using Moq;
using Appulu.Core.Data;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Messages;
using Appulu.Data;
using Appulu.Services.Users;
using Appulu.Services.Events;
using Appulu.Services.Messages;
using NUnit.Framework;

namespace Appulu.Services.Tests.Messages 
{
    [TestFixture]
    public class NewsLetterSubscriptionServiceTests : ServiceTest
    {
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IRepository<NewsLetterSubscription>> _newsLetterSubscriptionRepository;
        private Mock<IRepository<User>> _userRepository;
        private Mock<IUserService> _userService;
        private Mock<IDbContext> _dbContext;

        [SetUp]
        public new void SetUp()
        {
            _eventPublisher = new Mock<IEventPublisher>();
            _newsLetterSubscriptionRepository = new Mock<IRepository<NewsLetterSubscription>>();
            _userRepository = new Mock<IRepository<User>>();
            _userService = new Mock<IUserService>();
            _dbContext = new Mock<IDbContext>();
        }

        /// <summary>
        /// Verifies the active insert triggers subscribe event.
        /// </summary>
        [Test]
        public void VerifyActiveInsertTriggersSubscribeEvent()
        {
            var service = new NewsLetterSubscriptionService(_userService.Object, _dbContext.Object, _eventPublisher.Object,
                _userRepository.Object, _newsLetterSubscriptionRepository.Object);

            var subscription = new NewsLetterSubscription { Active = true, Email = "test@test.com" };
            service.InsertNewsLetterSubscription(subscription);

            _eventPublisher.Verify(x => x.Publish(new EmailSubscribedEvent(subscription)));
        }

        /// <summary>
        /// Verifies the delete triggers unsubscribe event.
        /// </summary>
        [Test]
        public void VerifyDeleteTriggersUnsubscribeEvent()
        {
            var service = new NewsLetterSubscriptionService(_userService.Object, _dbContext.Object, _eventPublisher.Object,
                _userRepository.Object, _newsLetterSubscriptionRepository.Object);

            var subscription = new NewsLetterSubscription { Active = true, Email = "test@test.com" };
            service.DeleteNewsLetterSubscription(subscription);

            _eventPublisher.Verify(x => x.Publish(new EmailUnsubscribedEvent(subscription)));
        }
        
        /// <summary>
        /// Verifies the insert event is fired.
        /// </summary>
        [Test]
        [Ignore("Moq can't verify an extension method")]
        public void VerifyInsertEventIsFired()
        {
            var service = new NewsLetterSubscriptionService(_userService.Object, _dbContext.Object, _eventPublisher.Object,
                _userRepository.Object, _newsLetterSubscriptionRepository.Object);

            var subscription = new NewsLetterSubscription {Email = "test@test.com"};

            service.InsertNewsLetterSubscription(subscription);

            _eventPublisher.Verify(x => x.EntityInserted(It.IsAny<NewsLetterSubscription>()));
        }
    }
}