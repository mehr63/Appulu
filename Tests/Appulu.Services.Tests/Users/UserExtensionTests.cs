using NUnit.Framework;

namespace Appulu.Services.Tests.Users
{
    [TestFixture]
    public class UserExtensionTests : ServiceTest
    {
        //[Test]
        //public void Can_get_add_remove_giftCardCouponCodes()
        //{
        //    var user = new User();
        //    user.ApplyGiftCardCouponCode("code1");
        //    user.ApplyGiftCardCouponCode("code2");
        //    user.RemoveGiftCardCouponCode("code2");
        //    user.ApplyGiftCardCouponCode("code3");

        //    var codes = user.ParseAppliedGiftCardCouponCodes();
        //    codes.Length.ShouldEqual(2);
        //    codes[0].ShouldEqual("code1");
        //    codes[1].ShouldEqual("code3");
        //}
        //[Test]
        //public void Can_not_add_duplicate_giftCardCouponCodes()
        //{
        //    var user = new User();
        //    user.ApplyGiftCardCouponCode("code1");
        //    user.ApplyGiftCardCouponCode("code2");
        //    user.ApplyGiftCardCouponCode("code1");

        //    var codes = user.ParseAppliedGiftCardCouponCodes();
        //    codes.Length.ShouldEqual(2);
        //    codes[0].ShouldEqual("code1");
        //    codes[1].ShouldEqual("code2");
        //}
    }
}
