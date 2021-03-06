﻿using System;
using Appulu.Core.Domain.News;
using Appulu.Services.News;
using Appulu.Tests;
using NUnit.Framework;

namespace Appulu.Services.Tests.News
{
    [TestFixture]
    public class NewsServiceTests : ServiceTest
    {
        private INewsService _newsService;

        [SetUp]
        public new void SetUp()
        {
            _newsService = new NewsService(null, null, null, null, null);
        }

        [Test]
        public void Should_be_available_when_startdate_is_not_set()
        {
            var newsItem = new NewsItem
            {
                StartDateUtc = null
            };

            _newsService.IsNewsAvailable(newsItem, new DateTime(2010, 01, 03)).ShouldEqual(true);
        }

        [Test]
        public void Should_be_available_when_startdate_is_less_than_somedate()
        {
            var newsItem = new NewsItem
            {
                StartDateUtc = new DateTime(2010, 01, 02)
            };

            _newsService.IsNewsAvailable(newsItem, new DateTime(2010, 01, 03)).ShouldEqual(true);
        }

        [Test]
        public void Should_not_be_available_when_startdate_is_greater_than_somedate()
        {
            var newsItem = new NewsItem
            {
                StartDateUtc = new DateTime(2010, 01, 02)
            };

            _newsService.IsNewsAvailable(newsItem, new DateTime(2010, 01, 01)).ShouldEqual(false);
        }

        [Test]
        public void Should_be_available_when_enddate_is_not_set()
        {
            var newsItem = new NewsItem
            {
                EndDateUtc = null
            };

            _newsService.IsNewsAvailable(newsItem, new DateTime(2010, 01, 03)).ShouldEqual(true);
        }

        [Test]
        public void Should_be_available_when_enddate_is_greater_than_somedate()
        {
            var newsItem = new NewsItem
            {
                EndDateUtc = new DateTime(2010, 01, 02)
            };

            _newsService.IsNewsAvailable(newsItem, new DateTime(2010, 01, 01)).ShouldEqual(true);
        }

        [Test]
        public void Should_not_be_available_when_enddate_is_less_than_somedate()
        {
            var newsItem = new NewsItem
            {
                EndDateUtc = new DateTime(2010, 01, 02)
            };

            _newsService.IsNewsAvailable(newsItem, new DateTime(2010, 01, 03)).ShouldEqual(false);
        }
    }
}
