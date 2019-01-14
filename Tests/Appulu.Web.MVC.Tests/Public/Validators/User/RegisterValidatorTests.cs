using FluentValidation.TestHelper;
using Moq;
using Appulu.Core.Domain.Users;
using Appulu.Services.Directory;
using Appulu.Web.Models.User;
using Appulu.Web.Validators.User;
using NUnit.Framework;

namespace Appulu.Web.MVC.Tests.Public.Validators.User
{
    [TestFixture]
    public class RegisterValidatorTests : BaseValidatorTests
    {
        private RegisterValidator _validator;
        private Mock<IStateProvinceService> _stateProvinceService;
        private UserSettings _userSettings;
        
        [SetUp]
        public new void Setup()
        {
            _userSettings = new UserSettings();
            _stateProvinceService = new Mock<IStateProvinceService>();
            _validator = new RegisterValidator(_localizationService, _stateProvinceService.Object, _userSettings);
        }
        
        [Test]
        public void Should_have_error_when_email_is_null_or_empty()
        {
            var model = new RegisterModel
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
            var model = new RegisterModel
            {
                Email = "adminexample.com"
            };
            _validator.ShouldHaveValidationErrorFor(x => x.Email, model);
        }

        [Test]
        public void Should_not_have_error_when_email_is_correct_format()
        {
            var model = new RegisterModel
            {
                Email = "admin@example.com"
            };
            _validator.ShouldNotHaveValidationErrorFor(x => x.Email, model);
        }

        [Test]
        public void Should_have_error_when_firstName_is_null_or_empty()
        {
            var model = new RegisterModel
            {
                FirstName = null
            };
            _validator.ShouldHaveValidationErrorFor(x => x.FirstName, model);
            model.FirstName = "";
            _validator.ShouldHaveValidationErrorFor(x => x.FirstName, model);
        }

        [Test]
        public void Should_not_have_error_when_firstName_is_specified()
        {
            var model = new RegisterModel
            {
                FirstName = "John"
            };
            _validator.ShouldNotHaveValidationErrorFor(x => x.FirstName, model);
        }

        [Test]
        public void Should_have_error_when_lastName_is_null_or_empty()
        {
            var model = new RegisterModel
            {
                LastName = null
            };
            _validator.ShouldHaveValidationErrorFor(x => x.LastName, model);
            model.LastName = "";
            _validator.ShouldHaveValidationErrorFor(x => x.LastName, model);
        }

        [Test]
        public void Should_not_have_error_when_lastName_is_specified()
        {
            var model = new RegisterModel
            {
                LastName = "Smith"
            };
            _validator.ShouldNotHaveValidationErrorFor(x => x.LastName, model);
        }

        [Test]
        public void Should_have_error_when_password_is_null_or_empty()
        {
            var model = new RegisterModel
            {
                Password = null
            };
            //we know that password should equal confirmation password
            model.ConfirmPassword = model.Password;
            _validator.ShouldHaveValidationErrorFor(x => x.Password, model);
            model.Password = "";
            //we know that password should equal confirmation password
            model.ConfirmPassword = model.Password;
            _validator.ShouldHaveValidationErrorFor(x => x.Password, model);
        }

        [Test]
        public void Should_not_have_error_when_password_is_specified()
        {
            var model = new RegisterModel
            {
                Password = "password"
            };
            //we know that password should equal confirmation password
            model.ConfirmPassword = model.Password;
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, model);
        }

        [Test]
        public void Should_have_error_when_confirmPassword_is_null_or_empty()
        {
            var model = new RegisterModel
            {
                ConfirmPassword = null
            };
            _validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, model);
            model.ConfirmPassword = "";
            _validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, model);
        }

        [Test]
        public void Should_not_have_error_when_confirmPassword_is_specified()
        {
            var model = new RegisterModel
            {
                ConfirmPassword = "some password"
            };
            //we know that new password should equal confirmation password
            model.Password = model.ConfirmPassword;
            _validator.ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword, model);
        }

        [Test]
        public void Should_have_error_when_password_doesnot_equal_confirmationPassword()
        {
            var model = new RegisterModel
            {
                Password = "some password",
                ConfirmPassword = "another password"
            };
            _validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, model);
        }

        [Test]
        public void Should_not_have_error_when_password_equals_confirmationPassword()
        {
            var model = new RegisterModel
            {
                Password = "some password",
                ConfirmPassword = "some password"
            };
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, model);
        }

        [Test]
        public void Should_validate_password_is_length()
        {
            _userSettings.PasswordMinLength = 5;
            _validator = new RegisterValidator(_localizationService, _stateProvinceService.Object, _userSettings);

            var model = new RegisterModel
            {
                Password = "1234"
            };
            //we know that password should equal confirmation password
            model.ConfirmPassword = model.Password;
            _validator.ShouldHaveValidationErrorFor(x => x.Password, model);
            model.Password = "12345";
            //we know that password should equal confirmation password
            model.ConfirmPassword = model.Password;
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, model);
        }
    }
}
