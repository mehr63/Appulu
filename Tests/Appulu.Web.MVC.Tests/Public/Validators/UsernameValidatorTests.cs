using Appulu.Core.Domain.Users;
using Appulu.Tests;
using Appulu.Web.Framework.Validators;
using NUnit.Framework;

namespace Appulu.Web.MVC.Tests.Public.Validators
{
    [TestFixture]
    public class UsernameValidatorTests
    {
        private TestValidator _validator;
        private UserSettings _userSettings;
        
        [SetUp]
        public void Setup()
        {
            _userSettings = new UserSettings
            {
                UsernameValidationRule = "^a.*1$",
                UsernameValidationEnabled = true,
                UsernameValidationUseRegex = false
            };

            _validator = new TestValidator { v => v.RuleFor(x => x.Username).IsUsername(_userSettings) };
        }

        [Test]
        public void IsValidTests()
        {
            //optional value is not valid
            _validator.Validate(new Person { Username = null }).IsValid.ShouldBeFalse();
            _validator.Validate(new Person { Username = string.Empty }).IsValid.ShouldBeFalse();

            //validation without regex
            _validator.Validate(new Person { Username = "test_user" }).IsValid.ShouldBeFalse();
            _validator.Validate(new Person { Username = "a*1^" }).IsValid.ShouldBeTrue();

            //validation with regex
            _userSettings.UsernameValidationUseRegex = true;
            _validator.Validate(new Person { Username = "test_user" }).IsValid.ShouldBeFalse();
            _validator.Validate(new Person { Username = "a*1^" }).IsValid.ShouldBeFalse();
            _validator.Validate(new Person { Username = "a1" }).IsValid.ShouldBeTrue();
            _validator.Validate(new Person { Username = "a_test_user_name_1" }).IsValid.ShouldBeTrue();
            _validator.Validate(new Person { Username = "b_test_user_name_1" }).IsValid.ShouldBeFalse();
        }
    }
}
