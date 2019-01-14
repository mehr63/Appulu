using FluentValidation.TestHelper;
using Appulu.Core.Domain.Users;
using Appulu.Web.Models.User;
using Appulu.Web.Validators.User;
using NUnit.Framework;

namespace Appulu.Web.MVC.Tests.Public.Validators.User
{
    [TestFixture]
    public class PasswordRecoveryConfirmValidatorTests : BaseValidatorTests
    {
        private PasswordRecoveryConfirmValidator _validator;
        private UserSettings _userSettings;
        
        [SetUp]
        public new void Setup()
        {
            _userSettings = new UserSettings();
            _validator = new PasswordRecoveryConfirmValidator(_localizationService, _userSettings);
        }

        [Test]
        public void Should_have_error_when_newPassword_is_null_or_empty()
        {
            var model = new PasswordRecoveryConfirmModel
            {
                NewPassword = null
            };
            //we know that new password should equal confirmation password
            model.ConfirmNewPassword = model.NewPassword;
            _validator.ShouldHaveValidationErrorFor(x => x.NewPassword, model);
            model.NewPassword = "";
            //we know that new password should equal confirmation password
            model.ConfirmNewPassword = model.NewPassword;
            _validator.ShouldHaveValidationErrorFor(x => x.NewPassword, model);
        }

        [Test]
        public void Should_not_have_error_when_newPassword_is_specified()
        {
            var model = new PasswordRecoveryConfirmModel
            {
                NewPassword = "new password"
            };
            //we know that new password should equal confirmation password
            model.ConfirmNewPassword = model.NewPassword;
            _validator.ShouldNotHaveValidationErrorFor(x => x.NewPassword, model);
        }

        [Test]
        public void Should_have_error_when_confirmNewPassword_is_null_or_empty()
        {
            var model = new PasswordRecoveryConfirmModel
            {
                ConfirmNewPassword = null
            };
            _validator.ShouldHaveValidationErrorFor(x => x.ConfirmNewPassword, model);
            model.ConfirmNewPassword = "";
            _validator.ShouldHaveValidationErrorFor(x => x.ConfirmNewPassword, model);
        }

        [Test]
        public void Should_not_have_error_when_confirmNewPassword_is_specified()
        {
            var model = new PasswordRecoveryConfirmModel
            {
                ConfirmNewPassword = "some password"
            };
            //we know that new password should equal confirmation password
            model.NewPassword = model.ConfirmNewPassword;
            _validator.ShouldNotHaveValidationErrorFor(x => x.ConfirmNewPassword, model);
        }

        [Test]
        public void Should_have_error_when_newPassword_doesnot_equal_confirmationPassword()
        {
            var model = new PasswordRecoveryConfirmModel
            {
                NewPassword = "some password",
                ConfirmNewPassword = "another password"
            };
            _validator.ShouldHaveValidationErrorFor(x => x.ConfirmNewPassword, model);
        }

        [Test]
        public void Should_not_have_error_when_newPassword_equals_confirmationPassword()
        {
            var model = new PasswordRecoveryConfirmModel
            {
                NewPassword = "some password",
                ConfirmNewPassword = "some password"
            };
            _validator.ShouldNotHaveValidationErrorFor(x => x.NewPassword, model);
        }

        [Test]
        public void Should_validate_newPassword_is_length()
        {
            _userSettings.PasswordMinLength = 5;
            _validator = new PasswordRecoveryConfirmValidator(_localizationService, _userSettings);

            var model = new PasswordRecoveryConfirmModel
            {
                NewPassword = "1234"
            };
            //we know that new password should equal confirmation password
            model.ConfirmNewPassword = model.NewPassword;
            _validator.ShouldHaveValidationErrorFor(x => x.NewPassword, model);
            model.NewPassword = "12345";
            //we know that new password should equal confirmation password
            model.ConfirmNewPassword = model.NewPassword;
            _validator.ShouldNotHaveValidationErrorFor(x => x.NewPassword, model);
        }
    }
}
