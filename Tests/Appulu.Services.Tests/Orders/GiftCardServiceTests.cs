﻿using Appulu.Core.Domain.Orders;
using Appulu.Services.Orders;
using Appulu.Tests;
using NUnit.Framework;

namespace Appulu.Services.Tests.Orders
{
    [TestFixture]
    public class GiftCardServiceTests : ServiceTest
    {
        private IGiftCardService _giftCardService;

        [SetUp]
        public new void SetUp()
        {
            _giftCardService = new GiftCardService(null, null, null, null);
        }

        [Test]
        public void Can_validate_giftCard()
        {

            var gc = new GiftCard
            {
                Amount = 100,
                IsGiftCardActivated = true
            };
            gc.GiftCardUsageHistory.Add
                (
                    new GiftCardUsageHistory
                    {
                        UsedValue = 30
                    }

                );
            gc.GiftCardUsageHistory.Add
                (
                    new GiftCardUsageHistory
                    {
                        UsedValue = 20
                    }

                );
            gc.GiftCardUsageHistory.Add
                (
                    new GiftCardUsageHistory
                    {
                        UsedValue = 5
                    }

                );

            //valid
            _giftCardService.IsGiftCardValid(gc).ShouldEqual(true);

            //mark as not active
            gc.IsGiftCardActivated = false;
            _giftCardService.IsGiftCardValid(gc).ShouldEqual(false);

            //again active
            gc.IsGiftCardActivated = true;
            _giftCardService.IsGiftCardValid(gc).ShouldEqual(true);

            //add usage history record
            gc.GiftCardUsageHistory.Add(new GiftCardUsageHistory
            {
                UsedValue = 1000
            });
            _giftCardService.IsGiftCardValid(gc).ShouldEqual(false);
        }

        [Test]
        public void Can_calculate_giftCard_remainingAmount()
        {
            var gc = new GiftCard
            {
                Amount = 100
            };
            gc.GiftCardUsageHistory.Add
                (
                    new GiftCardUsageHistory
                    {
                        UsedValue = 30
                    }

                );
            gc.GiftCardUsageHistory.Add
                (
                    new GiftCardUsageHistory
                    {
                        UsedValue = 20
                    }

                );
            gc.GiftCardUsageHistory.Add
                (
                    new GiftCardUsageHistory
                    {
                        UsedValue = 5
                    }

                );

            _giftCardService.GetGiftCardRemainingAmount(gc).ShouldEqual(45);
        }
    }
}
