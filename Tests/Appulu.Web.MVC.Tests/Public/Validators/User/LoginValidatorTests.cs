using FluentValidation.TestHelper;
using Appulu.Core.Domain.Users;
using Appulu.Web.Models.User;
using Appulu.Web.Validators.User;
using NUnit.Framework;

namespace Appulu.Web.MVC.Tests.Public.Validators.User
{
    [TestFixture]
    public class LoginValidatorTests : BaseValidatorTests
    {
        private LoginValidator _validator;
        private UserSettings _userSettings;
        
        [SetUp]
        public new void Setup()
        {
            _userSettings = new UserSettings();
            _validator = new LoginValidator(_localizationService, _userSettings);
        }
        
        [Test]
        public void Should_have_error_when_email_is_null_or_empty()
        {
            var model = new LoginModel
            {
                Email = null
            };
            _validator.ShouldHaveValidationErrorFor(x => x.Email, model);
            model.Email = "";
            _validator.ShouldHaveValidationErrorFor(x => x.Email, model);
        }

        [Test]
        public void Should_have_error_when_email_is_wrong_format()
        {
            var model = new LoginModel
            {
                Email = "adminexample.com"
            };
            _validator.ShouldHaveValidationErrorFor(x => x.Email, model);
        }

        [Test]
        public void Should_not_have_error_when_email_is_correct_format()
        {
            var model = new LoginModel
            {
                Email = "admin@example.com"
            };
            _validator.ShouldNotHaveValidationErrorFor(x => x.Email, model);
        }

        [Test]
        public void Should_not_have_error_when_email_is_null_but_usernames_are_enabled()
        {
            _userSettings = new UserSettings
            {
                UsernamesEnabled = true
            };
            _validator = new LoginValidator(_localizationService, _userSettings);

            var model = new LoginModel
            {
                Email = null
            };
            _validator.ShouldNotHaveValidationErrorFor(x => x.Email, model);
        }
    }
}
