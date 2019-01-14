using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Appulu.Web.Framework.Localization;
using Appulu.Web.Framework.Mvc.Routing;

namespace Appulu.Web.Infrastructure
{
    /// <summary>
    /// Represents provider that provided basic routes
    /// </summary>
    public partial class RouteProvider : IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //reorder routes so the most used ones are on top. It can improve performance

            //areas
            routeBuilder.MapRoute(name: "areaRoute", template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            //home page
            routeBuilder.MapLocalizedRoute("HomePage", "",
				new { controller = "Home", action = "Index" });

            //login
            routeBuilder.MapLocalizedRoute("Login", "login/",
				new { controller = "User", action = "Login" });

            //register
            routeBuilder.MapLocalizedRoute("Register", "register/",
				new { controller = "User", action = "Register" });

            //logout
            routeBuilder.MapLocalizedRoute("Logout", "logout/",
				new { controller = "User", action = "Logout" });

            //shopping cart
            routeBuilder.MapLocalizedRoute("ShoppingCart", "cart/",
				new { controller = "ShoppingCart", action = "Cart" });            

            //estimate shipping
            routeBuilder.MapLocalizedRoute("EstimateShipping", "cart/estimateshipping",
				new {controller = "ShoppingCart", action = "GetEstimateShipping"});

            //wishlist
            routeBuilder.MapLocalizedRoute("Wishlist", "wishlist/{userGuid?}",
				new { controller = "ShoppingCart", action = "Wishlist"});

            //user account links
            routeBuilder.MapLocalizedRoute("UserInfo", "user/info",
				new { controller = "User", action = "Info" });

            routeBuilder.MapLocalizedRoute("UserAddresses", "user/addresses",
				new { controller = "User", action = "Addresses" });

            routeBuilder.MapLocalizedRoute("UserOrders", "order/history",
				new { controller = "Order", action = "UserOrders" });

            //contact us
            routeBuilder.MapLocalizedRoute("ContactUs", "contactus",
				new { controller = "Common", action = "ContactUs" });

            //sitemap
            routeBuilder.MapLocalizedRoute("Sitemap", "sitemap",
				new { controller = "Common", action = "Sitemap" });

            //product search
            routeBuilder.MapLocalizedRoute("ProductSearch", "search/",
				new { controller = "Catalog", action = "Search" });                     

            routeBuilder.MapLocalizedRoute("ProductSearchAutoComplete", "catalog/searchtermautocomplete",
				new { controller = "Catalog", action = "SearchTermAutoComplete" });

            //change currency (AJAX link)
            routeBuilder.MapLocalizedRoute("ChangeCurrency", "changecurrency/{usercurrency:min(0)}",
				new { controller = "Common", action = "SetCurrency" });

            //change language (AJAX link)
            routeBuilder.MapLocalizedRoute("ChangeLanguage", "changelanguage/{langid:min(0)}",
				new { controller = "Common", action = "SetLanguage" });

            //change tax (AJAX link)
            routeBuilder.MapLocalizedRoute("ChangeTaxType", "changetaxtype/{usertaxtype:min(0)}",
				new { controller = "Common", action = "SetTaxType" });

            //recently viewed products
            routeBuilder.MapLocalizedRoute("RecentlyViewedProducts", "recentlyviewedproducts/",
				new { controller = "Product", action = "RecentlyViewedProducts" });

            //new products
            routeBuilder.MapLocalizedRoute("NewProducts", "newproducts/",
				new { controller = "Product", action = "NewProducts" });

            //blog
            routeBuilder.MapLocalizedRoute("Blog", "blog",
				new { controller = "Blog", action = "List" });

            //news
            routeBuilder.MapLocalizedRoute("NewsArchive", "news",
				new { controller = "News", action = "List" });

            //forum
            routeBuilder.MapLocalizedRoute("Boards", "boards",
				new { controller = "Boards", action = "Index" });

            //compare products
            routeBuilder.MapLocalizedRoute("CompareProducts", "compareproducts/",
				new { controller = "Product", action = "CompareProducts" });

            //product tags
            routeBuilder.MapLocalizedRoute("ProductTagsAll", "producttag/all/",
				new { controller = "Catalog", action = "ProductTagsAll" });

            //manufacturers
            routeBuilder.MapLocalizedRoute("ManufacturerList", "manufacturer/all/",
				new { controller = "Catalog", action = "ManufacturerAll" });

            //vendors
            routeBuilder.MapLocalizedRoute("VendorList", "vendor/all/",
				new { controller = "Catalog", action = "VendorAll" });

            //add product to cart (without any attributes and options). used on catalog pages.
            routeBuilder.MapLocalizedRoute("AddProductToCart-Catalog", "addproducttocart/catalog/{productId:min(0)}/{shoppingCartTypeId:min(0)}/{quantity:min(0)}",
				new { controller = "ShoppingCart", action = "AddProductToCart_Catalog" });

            //add product to cart (with attributes and options). used on the product details pages.
            routeBuilder.MapLocalizedRoute("AddProductToCart-Details", "addproducttocart/details/{productId:min(0)}/{shoppingCartTypeId:min(0)}",
				new { controller = "ShoppingCart", action = "AddProductToCart_Details" });

            //comparing products
            routeBuilder.MapLocalizedRoute("AddProductToCompare", "compareproducts/add/{productId:min(0)}",
				new { controller = "Product", action = "AddProductToCompareList" });

            //product email a friend
            routeBuilder.MapLocalizedRoute("ProductEmailAFriend", "productemailafriend/{productId:min(0)}",
				new { controller = "Product", action = "ProductEmailAFriend" });

            //reviews
            routeBuilder.MapLocalizedRoute("ProductReviews", "productreviews/{productId}",
				new { controller = "Product", action = "ProductReviews" });

            routeBuilder.MapLocalizedRoute("UserProductReviews", "user/productreviews",
				new { controller = "Product", action = "UserProductReviews" });

            routeBuilder.MapLocalizedRoute("UserProductReviewsPaged", "user/productreviews/page/{pageNumber:min(0)}",
				new { controller = "Product", action = "UserProductReviews" });

            //back in stock notifications
            routeBuilder.MapLocalizedRoute("BackInStockSubscribePopup", "backinstocksubscribe/{productId:min(0)}",
				new { controller = "BackInStockSubscription", action = "SubscribePopup" });

            routeBuilder.MapLocalizedRoute("BackInStockSubscribeSend", "backinstocksubscribesend/{productId:min(0)}",
				new { controller = "BackInStockSubscription", action = "SubscribePopupPOST" });

            //downloads
            routeBuilder.MapRoute("GetSampleDownload", "download/sample/{productid:min(0)}",
				new { controller = "Download", action = "Sample" });

            //checkout pages
            routeBuilder.MapLocalizedRoute("Checkout", "checkout/",
				new { controller = "Checkout", action = "Index" });

            routeBuilder.MapLocalizedRoute("CheckoutOnePage", "onepagecheckout/",
				new { controller = "Checkout", action = "OnePageCheckout" });

            routeBuilder.MapLocalizedRoute("CheckoutShippingAddress", "checkout/shippingaddress",
				new { controller = "Checkout", action = "ShippingAddress" });

            routeBuilder.MapLocalizedRoute("CheckoutSelectShippingAddress", "checkout/selectshippingaddress",
				new { controller = "Checkout", action = "SelectShippingAddress" });

            routeBuilder.MapLocalizedRoute("CheckoutBillingAddress", "checkout/billingaddress",
				new { controller = "Checkout", action = "BillingAddress" });

            routeBuilder.MapLocalizedRoute("CheckoutSelectBillingAddress", "checkout/selectbillingaddress",
				new { controller = "Checkout", action = "SelectBillingAddress" });

            routeBuilder.MapLocalizedRoute("CheckoutShippingMethod", "checkout/shippingmethod",
				new { controller = "Checkout", action = "ShippingMethod" });

            routeBuilder.MapLocalizedRoute("CheckoutPaymentMethod", "checkout/paymentmethod",
				new { controller = "Checkout", action = "PaymentMethod" });

            routeBuilder.MapLocalizedRoute("CheckoutPaymentInfo", "checkout/paymentinfo",
				new { controller = "Checkout", action = "PaymentInfo" });

            routeBuilder.MapLocalizedRoute("CheckoutConfirm", "checkout/confirm",
				new { controller = "Checkout", action = "Confirm" });

            routeBuilder.MapLocalizedRoute("CheckoutCompleted", "checkout/completed/{orderId:regex(\\d*)}",
				new { controller = "Checkout", action = "Completed" });

            //subscribe newsletters
            routeBuilder.MapLocalizedRoute("SubscribeNewsletter", "subscribenewsletter",
				new { controller = "Newsletter", action = "SubscribeNewsletter" });

            //email wishlist
            routeBuilder.MapLocalizedRoute("EmailWishlist", "emailwishlist",
				new { controller = "ShoppingCart", action = "EmailWishlist" });

            //login page for checkout as guest
            routeBuilder.MapLocalizedRoute("LoginCheckoutAsGuest", "login/checkoutasguest",
				new { controller = "User", action = "Login", checkoutAsGuest = true });

            //register result page
            routeBuilder.MapLocalizedRoute("RegisterResult", "registerresult/{resultId:min(0)}",
				new { controller = "User", action = "RegisterResult" });

            //check username availability
            routeBuilder.MapLocalizedRoute("CheckUsernameAvailability", "user/checkusernameavailability",
				new { controller = "User", action = "CheckUsernameAvailability" });

            //passwordrecovery
            routeBuilder.MapLocalizedRoute("PasswordRecovery", "passwordrecovery",
				new { controller = "User", action = "PasswordRecovery" });

            //password recovery confirmation
            routeBuilder.MapLocalizedRoute("PasswordRecoveryConfirm", "passwordrecovery/confirm",
				new { controller = "User", action = "PasswordRecoveryConfirm" });

            //topics
            routeBuilder.MapLocalizedRoute("TopicPopup", "t-popup/{SystemName}",
				new { controller = "Topic", action = "TopicDetailsPopup" });
            
            //blog
            routeBuilder.MapLocalizedRoute("BlogByTag", "blog/tag/{tag}",
				new { controller = "Blog", action = "BlogByTag" });

            routeBuilder.MapLocalizedRoute("BlogByMonth", "blog/month/{month}",
				new { controller = "Blog", action = "BlogByMonth" });

            //blog RSS
            routeBuilder.MapLocalizedRoute("BlogRSS", "blog/rss/{languageId:min(0)}",
				new { controller = "Blog", action = "ListRss" });

            //news RSS
            routeBuilder.MapLocalizedRoute("NewsRSS", "news/rss/{languageId:min(0)}",
				new { controller = "News", action = "ListRss" });

            //set review helpfulness (AJAX link)
            routeBuilder.MapRoute("SetProductReviewHelpfulness", "setproductreviewhelpfulness",
				new { controller = "Product", action = "SetProductReviewHelpfulness" });

            //user account links
            routeBuilder.MapLocalizedRoute("UserReturnRequests", "returnrequest/history",
				new { controller = "ReturnRequest", action = "UserReturnRequests" });

            routeBuilder.MapLocalizedRoute("UserDownloadableProducts", "user/downloadableproducts",
				new { controller = "User", action = "DownloadableProducts" });

            routeBuilder.MapLocalizedRoute("UserBackInStockSubscriptions", "backinstocksubscriptions/manage",
				new { controller = "BackInStockSubscription", action = "UserSubscriptions" });

            routeBuilder.MapLocalizedRoute("UserBackInStockSubscriptionsPaged", "backinstocksubscriptions/manage/{pageNumber:regex(\\d*)}",
				new { controller = "BackInStockSubscription", action = "UserSubscriptions" });

            routeBuilder.MapLocalizedRoute("UserRewardPoints", "rewardpoints/history",
				new { controller = "Order", action = "UserRewardPoints" });

            routeBuilder.MapLocalizedRoute("UserRewardPointsPaged", "rewardpoints/history/page/{pageNumber:min(0)}",
				new { controller = "Order", action = "UserRewardPoints" });

            routeBuilder.MapLocalizedRoute("UserChangePassword", "user/changepassword",
				new { controller = "User", action = "ChangePassword" });

            routeBuilder.MapLocalizedRoute("UserAvatar", "user/avatar",
				new { controller = "User", action = "Avatar" });

            routeBuilder.MapLocalizedRoute("AccountActivation", "user/activation",
				new { controller = "User", action = "AccountActivation" });

            routeBuilder.MapLocalizedRoute("EmailRevalidation", "user/revalidateemail",
				new { controller = "User", action = "EmailRevalidation" });

            routeBuilder.MapLocalizedRoute("UserForumSubscriptions", "boards/forumsubscriptions",
				new { controller = "Boards", action = "UserForumSubscriptions" });

            routeBuilder.MapLocalizedRoute("UserForumSubscriptionsPaged", "boards/forumsubscriptions/{pageNumber:regex(\\d*)}",
				new { controller = "Boards", action = "UserForumSubscriptions" });

            routeBuilder.MapLocalizedRoute("UserAddressEdit", "user/addressedit/{addressId:min(0)}",
				new { controller = "User", action = "AddressEdit" });

            routeBuilder.MapLocalizedRoute("UserAddressAdd", "user/addressadd",
				new { controller = "User", action = "AddressAdd" });

            //user profile page
            routeBuilder.MapLocalizedRoute("UserProfile", "profile/{id:min(0)}",
				new { controller = "Profile", action = "Index" });

            routeBuilder.MapLocalizedRoute("UserProfilePaged", "profile/{id}/page/{pageNumber:min(0)}",
				new { controller = "Profile", action = "Index" });

            //orders
            routeBuilder.MapLocalizedRoute("OrderDetails", "orderdetails/{orderId:min(0)}",
				new { controller = "Order", action = "Details" });

            routeBuilder.MapLocalizedRoute("ShipmentDetails", "orderdetails/shipment/{shipmentId}",
				new { controller = "Order", action = "ShipmentDetails" });

            routeBuilder.MapLocalizedRoute("ReturnRequest", "returnrequest/{orderId:min(0)}",
				new { controller = "ReturnRequest", action = "ReturnRequest" });

            routeBuilder.MapLocalizedRoute("ReOrder", "reorder/{orderId:min(0)}",
				new { controller = "Order", action = "ReOrder" });

            routeBuilder.MapLocalizedRoute("GetOrderPdfInvoice", "orderdetails/pdf/{orderId}",
				new { controller = "Order", action = "GetPdfInvoice" });

            routeBuilder.MapLocalizedRoute("PrintOrderDetails", "orderdetails/print/{orderId}",
				new { controller = "Order", action = "PrintOrderDetails" });

            //order downloads
            routeBuilder.MapRoute("GetDownload", "download/getdownload/{orderItemId:guid}/{agree?}",
				new { controller = "Download", action = "GetDownload" });

            routeBuilder.MapRoute("GetLicense", "download/getlicense/{orderItemId:guid}/",
				new { controller = "Download", action = "GetLicense" });

            routeBuilder.MapLocalizedRoute("DownloadUserAgreement", "user/useragreement/{orderItemId:guid}",
				new { controller = "User", action = "UserAgreement" });

            routeBuilder.MapRoute("GetOrderNoteFile", "download/ordernotefile/{ordernoteid:min(0)}",
				new { controller = "Download", action = "GetOrderNoteFile" });

            //contact vendor
            routeBuilder.MapLocalizedRoute("ContactVendor", "contactvendor/{vendorId}",
				new { controller = "Common", action = "ContactVendor" });

            //apply for vendor account
            routeBuilder.MapLocalizedRoute("ApplyVendorAccount", "vendor/apply",
				new { controller = "Vendor", action = "ApplyVendor" });

            //vendor info
            routeBuilder.MapLocalizedRoute("UserVendorInfo", "user/vendorinfo",
				new { controller = "Vendor", action = "Info" });

            //user GDPR
            routeBuilder.MapLocalizedRoute("GdprTools", "user/gdpr",
                new { controller = "User", action = "GdprTools" });

            //user check gift card balance 
            routeBuilder.MapLocalizedRoute("CheckGiftCardBalance", "user/CheckGiftCardBalance",
                new { controller = "User", action = "CheckGiftCardBalance" });

            //poll vote AJAX link
            routeBuilder.MapLocalizedRoute("PollVote", "poll/vote",
				new { controller = "Poll", action = "Vote" });

            //comparing products
            routeBuilder.MapLocalizedRoute("RemoveProductFromCompareList", "compareproducts/remove/{productId}",
				new { controller = "Product", action = "RemoveProductFromCompareList" });

            routeBuilder.MapLocalizedRoute("ClearCompareList", "clearcomparelist/",
				new { controller = "Product", action = "ClearCompareList" });

            //new RSS
            routeBuilder.MapLocalizedRoute("NewProductsRSS", "newproducts/rss",
				new { controller = "Product", action = "NewProductsRss" });
            
            //get state list by country ID  (AJAX link)
            routeBuilder.MapRoute("GetStatesByCountryId", "country/getstatesbycountryid/",
				new { controller = "Country", action = "GetStatesByCountryId" });

            //EU Cookie law accept button handler (AJAX link)
            routeBuilder.MapRoute("EuCookieLawAccept", "eucookielawaccept",
				new { controller = "Common", action = "EuCookieLawAccept" });

            //authenticate topic AJAX link
            routeBuilder.MapLocalizedRoute("TopicAuthenticate", "topic/authenticate",
				new { controller = "Topic", action = "Authenticate" });

            //product attributes with "upload file" type
            routeBuilder.MapLocalizedRoute("UploadFileProductAttribute", "uploadfileproductattribute/{attributeId:min(0)}",
				new { controller = "ShoppingCart", action = "UploadFileProductAttribute" });

            //checkout attributes with "upload file" type
            routeBuilder.MapLocalizedRoute("UploadFileCheckoutAttribute", "uploadfilecheckoutattribute/{attributeId:min(0)}",
				new { controller = "ShoppingCart", action = "UploadFileCheckoutAttribute" });

            //return request with "upload file" support
            routeBuilder.MapLocalizedRoute("UploadFileReturnRequest", "uploadfilereturnrequest",
				new { controller = "ReturnRequest", action = "UploadFileReturnRequest" });

            //forums
            routeBuilder.MapLocalizedRoute("ActiveDiscussions", "boards/activediscussions",
				new { controller = "Boards", action = "ActiveDiscussions" });

            routeBuilder.MapLocalizedRoute("ActiveDiscussionsPaged", "boards/activediscussions/page/{pageNumber:regex(\\d*)}",
				new { controller = "Boards", action = "ActiveDiscussions" });

            routeBuilder.MapLocalizedRoute("ActiveDiscussionsRSS", "boards/activediscussionsrss",
				new { controller = "Boards", action = "ActiveDiscussionsRSS" });

            routeBuilder.MapLocalizedRoute("PostEdit", "boards/postedit/{id:min(0)}",
				new { controller = "Boards", action = "PostEdit" });

            routeBuilder.MapLocalizedRoute("PostDelete", "boards/postdelete/{id:min(0)}",
				new { controller = "Boards", action = "PostDelete" });

            routeBuilder.MapLocalizedRoute("PostCreate", "boards/postcreate/{id:min(0)}",
				new { controller = "Boards", action = "PostCreate" });

            routeBuilder.MapLocalizedRoute("PostCreateQuote", "boards/postcreate/{id:min(0)}/{quote:min(0)}",
				new { controller = "Boards", action = "PostCreate" });

            routeBuilder.MapLocalizedRoute("TopicEdit", "boards/topicedit/{id:min(0)}",
				new { controller = "Boards", action = "TopicEdit" });

            routeBuilder.MapLocalizedRoute("TopicDelete", "boards/topicdelete/{id:min(0)}",
				new { controller = "Boards", action = "TopicDelete" });

            routeBuilder.MapLocalizedRoute("TopicCreate", "boards/topiccreate/{id:min(0)}",
				new { controller = "Boards", action = "TopicCreate" });

            routeBuilder.MapLocalizedRoute("TopicMove", "boards/topicmove/{id:min(0)}",
				new { controller = "Boards", action = "TopicMove" });

            routeBuilder.MapLocalizedRoute("TopicWatch", "boards/topicwatch/{id:min(0)}",
				new { controller = "Boards", action = "TopicWatch" });

            routeBuilder.MapLocalizedRoute("TopicSlug", "boards/topic/{id:min(0)}/{slug?}",
				new { controller = "Boards", action = "Topic" });

            routeBuilder.MapLocalizedRoute("TopicSlugPaged", "boards/topic/{id:min(0)}/{slug?}/page/{pageNumber:regex(\\d*)}",
				new { controller = "Boards", action = "Topic" });

            routeBuilder.MapLocalizedRoute("ForumWatch", "boards/forumwatch/{id:min(0)}",
				new { controller = "Boards", action = "ForumWatch" });

            routeBuilder.MapLocalizedRoute("ForumRSS", "boards/forumrss/{id:min(0)}",
				new { controller = "Boards", action = "ForumRSS" });

            routeBuilder.MapLocalizedRoute("ForumSlug", "boards/forum/{id:min(0)}/{slug?}",
				new { controller = "Boards", action = "Forum" });

            routeBuilder.MapLocalizedRoute("ForumSlugPaged", "boards/forum/{id:min(0)}/{slug?}/page/{pageNumber:regex(\\d*)}",
				new { controller = "Boards", action = "Forum" });

            routeBuilder.MapLocalizedRoute("ForumGroupSlug", "boards/forumgroup/{id:min(0)}/{slug?}",
				new { controller = "Boards", action = "ForumGroup"});

            routeBuilder.MapLocalizedRoute("Search", "boards/search",
				new { controller = "Boards", action = "Search" });

            //private messages
            routeBuilder.MapLocalizedRoute("PrivateMessages", "privatemessages/{tab?}",
				new { controller = "PrivateMessages", action = "Index" });

            routeBuilder.MapLocalizedRoute("PrivateMessagesPaged", "privatemessages/{tab?}/page/{pageNumber:min(0)}",
				new { controller = "PrivateMessages", action = "Index" });

            routeBuilder.MapLocalizedRoute("PrivateMessagesInbox", "inboxupdate",
				new { controller = "PrivateMessages", action = "InboxUpdate" });

            routeBuilder.MapLocalizedRoute("PrivateMessagesSent", "sentupdate",
				new { controller = "PrivateMessages", action = "SentUpdate" });

            routeBuilder.MapLocalizedRoute("SendPM", "sendpm/{toUserId:min(0)}",
				new { controller = "PrivateMessages", action = "SendPM" });

            routeBuilder.MapLocalizedRoute("SendPMReply", "sendpm/{toUserId:min(0)}/{replyToMessageId:min(0)}",
				new { controller = "PrivateMessages", action = "SendPM" });

            routeBuilder.MapLocalizedRoute("ViewPM", "viewpm/{privateMessageId:min(0)}",
				new { controller = "PrivateMessages", action = "ViewPM" });

            routeBuilder.MapLocalizedRoute("DeletePM", "deletepm/{privateMessageId:min(0)}",
				new { controller = "PrivateMessages", action = "DeletePM" });

            //activate newsletters
            routeBuilder.MapLocalizedRoute("NewsletterActivation", "newsletter/subscriptionactivation/{token:guid}/{active}",
				new { controller = "Newsletter", action = "SubscriptionActivation" });

            //robots.txt
            routeBuilder.MapRoute("robots.txt", "robots.txt",
				new { controller = "Common", action = "RobotsTextFile" });

            //sitemap (XML)
            routeBuilder.MapLocalizedRoute("sitemap.xml", "sitemap.xml",
				new { controller = "Common", action = "SitemapXml" });

            routeBuilder.MapLocalizedRoute("sitemap-indexed.xml", "sitemap-{Id:min(0)}.xml",
				new { controller = "Common", action = "SitemapXml" });

            //store closed
            routeBuilder.MapLocalizedRoute("StoreClosed", "storeclosed",
				new { controller = "Common", action = "StoreClosed" });

            //install
            routeBuilder.MapRoute("Installation", "install",
				new { controller = "Install", action = "Index" });

            //error page
            routeBuilder.MapLocalizedRoute("Error", "error",
                new { controller = "Common", action = "Error" });

            //page not found
            routeBuilder.MapLocalizedRoute("PageNotFound", "page-not-found", 
                new { controller = "Common", action = "PageNotFound" });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return 0; }
        }

        #endregion
    }
}
