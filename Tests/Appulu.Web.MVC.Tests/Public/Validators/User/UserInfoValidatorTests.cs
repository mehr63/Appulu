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
    public class UserInfoValidatorTests : BaseValidatorTests
    {
        private IStateProvinceService _stateProvinceService;

        [Test]
        public void Should_have_error_when_email_is_null_or_empty()
        {
            _stateProvinceService = new Mock<IStateProvinceService>().Object;

            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, new UserSettings());

            var model = new UserInfoModel
            {
                Email = null
            };
            validator.ShouldHaveValidationErrorFor(x => x.Email, model);
            model.Email = "";
            validator.ShouldHaveValidationErrorFor(x => x.Email, model);
        }
        [Test]
        public void Should_have_error_when_email_is_wrong_format()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, 
                new UserSettings());

            var model = new UserInfoModel
            {
                Email = "adminexample.com"
            };
            validator.ShouldHaveValidationErrorFor(x => x.Email, model);
        }
        [Test]
        public void Should_not_have_error_when_email_is_correct_format()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings());

            var model = new UserInfoModel
            {
                Email = "admin@example.com"
            };
            validator.ShouldNotHaveValidationErrorFor(x => x.Email, model);
        }

        [Test]
        public void Should_have_error_when_firstName_is_null_or_empty()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, 
                new UserSettings());

            var model = new UserInfoModel
            {
                FirstName = null
            };
            validator.ShouldHaveValidationErrorFor(x => x.FirstName, model);
            model.FirstName = "";
            validator.ShouldHaveValidationErrorFor(x => x.FirstName, model);
        }
        [Test]
        public void Should_not_have_error_when_firstName_is_specified()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, 
                new UserSettings());

            var model = new UserInfoModel
            {
                FirstName = "John"
            };
            validator.ShouldNotHaveValidationErrorFor(x => x.FirstName, model);
        }

        [Test]
        public void Should_have_error_when_lastName_is_null_or_empty()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, 
                new UserSettings());

            var model = new UserInfoModel
            {
                LastName = null
            };
            validator.ShouldHaveValidationErrorFor(x => x.LastName, model);
            model.LastName = "";
            validator.ShouldHaveValidationErrorFor(x => x.LastName, model);
        }
        [Test]
        public void Should_not_have_error_when_lastName_is_specified()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, 
                new UserSettings());

            var model = new UserInfoModel
            {
                LastName = "Smith"
            };
            validator.ShouldNotHaveValidationErrorFor(x => x.LastName, model);
        }

        [Test]
        public void Should_have_error_when_company_is_null_or_empty_based_on_required_setting()
        {
            var model = new UserInfoModel();

            //required
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    CompanyEnabled = true,
                    CompanyRequired = true
                });
            model.Company = null;
            validator.ShouldHaveValidationErrorFor(x => x.Company, model);
            model.Company = "";
            validator.ShouldHaveValidationErrorFor(x => x.Company, model);


            //not required
            validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    CompanyEnabled = true,
                    CompanyRequired = false
                });
            model.Company = null;
            validator.ShouldNotHaveValidationErrorFor(x => x.Company, model);
            model.Company = "";
            validator.ShouldNotHaveValidationErrorFor(x => x.Company, model);
        }
        [Test]
        public void Should_not_have_error_when_company_is_specified()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, 
                new UserSettings
                {
                    CompanyEnabled = true
                });

            var model = new UserInfoModel
            {
                Company = "Company"
            };
            validator.ShouldNotHaveValidationErrorFor(x => x.Company, model);
        }

        [Test]
        public void Should_have_error_when_streetaddress_is_null_or_empty_based_on_required_setting()
        {
            var model = new UserInfoModel();

            //required
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    StreetAddressEnabled = true,
                    StreetAddressRequired = true
                });
            model.StreetAddress = null;
            validator.ShouldHaveValidationErrorFor(x => x.StreetAddress, model);
            model.StreetAddress = "";
            validator.ShouldHaveValidationErrorFor(x => x.StreetAddress, model);

            //not required
            validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    StreetAddressEnabled = true,
                    StreetAddressRequired = false
                });
            model.StreetAddress = null;
            validator.ShouldNotHaveValidationErrorFor(x => x.StreetAddress, model);
            model.StreetAddress = "";
            validator.ShouldNotHaveValidationErrorFor(x => x.StreetAddress, model);
        }
        [Test]
        public void Should_not_have_error_when_streetaddress_is_specified()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    StreetAddressEnabled = true
                });

            var model = new UserInfoModel
            {
                StreetAddress = "Street address"
            };
            validator.ShouldNotHaveValidationErrorFor(x => x.StreetAddress, model);
        }

        [Test]
        public void Should_have_error_when_streetaddress2_is_null_or_empty_based_on_required_setting()
        {
            var model = new UserInfoModel();

            //required
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    StreetAddress2Enabled = true,
                    StreetAddress2Required = true
                });
            model.StreetAddress2 = null;
            validator.ShouldHaveValidationErrorFor(x => x.StreetAddress2, model);
            model.StreetAddress2 = "";
            validator.ShouldHaveValidationErrorFor(x => x.StreetAddress2, model);

            //not required
            validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    StreetAddress2Enabled = true,
                    StreetAddress2Required = false
                });
            model.StreetAddress2 = null;
            validator.ShouldNotHaveValidationErrorFor(x => x.StreetAddress2, model);
            model.StreetAddress2 = "";
            validator.ShouldNotHaveValidationErrorFor(x => x.StreetAddress2, model);
        }
        [Test]
        public void Should_not_have_error_when_streetaddress2_is_specified()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, 
                new UserSettings
                {
                    StreetAddress2Enabled = true
                });

            var model = new UserInfoModel
            {
                StreetAddress2 = "Street address 2"
            };
            validator.ShouldNotHaveValidationErrorFor(x => x.StreetAddress2, model);
        }

        [Test]
        public void Should_have_error_when_zippostalcode_is_null_or_empty_based_on_required_setting()
        {
            var model = new UserInfoModel();

            //required
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    ZipPostalCodeEnabled = true,
                    ZipPostalCodeRequired = true
                });
            model.ZipPostalCode = null;
            validator.ShouldHaveValidationErrorFor(x => x.ZipPostalCode, model);
            model.ZipPostalCode = "";
            validator.ShouldHaveValidationErrorFor(x => x.ZipPostalCode, model);


            //not required
            validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    ZipPostalCodeEnabled = true,
                    ZipPostalCodeRequired = false
                });
            model.ZipPostalCode = null;
            validator.ShouldNotHaveValidationErrorFor(x => x.ZipPostalCode, model);
            model.ZipPostalCode = "";
            validator.ShouldNotHaveValidationErrorFor(x => x.ZipPostalCode, model);
        }
        [Test]
        public void Should_not_have_error_when_zippostalcode_is_specified()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, 
                new UserSettings
                {
                    StreetAddress2Enabled = true
                });

            var model = new UserInfoModel
            {
                ZipPostalCode = "zip"
            };
            validator.ShouldNotHaveValidationErrorFor(x => x.ZipPostalCode, model);
        }

        [Test]
        public void Should_have_error_when_city_is_null_or_empty_based_on_required_setting()
        {
            var model = new UserInfoModel();

            //required
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    CityEnabled = true,
                    CityRequired = true
                });
            model.City = null;
            validator.ShouldHaveValidationErrorFor(x => x.City, model);
            model.City = "";
            validator.ShouldHaveValidationErrorFor(x => x.City, model);


            //not required
            validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    CityEnabled = true,
                    CityRequired = false
                });
            model.City = null;
            validator.ShouldNotHaveValidationErrorFor(x => x.City, model);
            model.City = "";
            validator.ShouldNotHaveValidationErrorFor(x => x.City, model);
        }
        [Test]
        public void Should_not_have_error_when_city_is_specified()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, 
                new UserSettings
                {
                    CityEnabled = true
                });

            var model = new UserInfoModel
            {
                City = "City"
            };
            validator.ShouldNotHaveValidationErrorFor(x => x.City, model);
        }

        [Test]
        public void Should_have_error_when_phone_is_null_or_empty_based_on_required_setting()
        {
            var model = new UserInfoModel();

            //required
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    PhoneEnabled = true,
                    PhoneRequired = true
                });
            model.Phone = null;
            validator.ShouldHaveValidationErrorFor(x => x.Phone, model);
            model.Phone = "";
            validator.ShouldHaveValidationErrorFor(x => x.Phone, model);

            //not required
            validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    PhoneEnabled = true,
                    PhoneRequired = false
                });
            model.Phone = null;
            validator.ShouldNotHaveValidationErrorFor(x => x.Phone, model);
            model.Phone = "";
            validator.ShouldNotHaveValidationErrorFor(x => x.Phone, model);
        }
        [Test]
        public void Should_not_have_error_when_phone_is_specified()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService, 
                new UserSettings
                {
                    PhoneEnabled = true
                });

            var model = new UserInfoModel
            {
                Phone = "Phone"
            };
            validator.ShouldNotHaveValidationErrorFor(x => x.Phone, model);
        }

        [Test]
        public void Should_have_error_when_fax_is_null_or_empty_based_on_required_setting()
        {
            var model = new UserInfoModel();

            //required
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    FaxEnabled = true,
                    FaxRequired = true
                });
            model.Fax = null;
            validator.ShouldHaveValidationErrorFor(x => x.Fax, model);
            model.Fax = "";
            validator.ShouldHaveValidationErrorFor(x => x.Fax, model);


            //not required
            validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    FaxEnabled = true,
                    FaxRequired = false
                });
            model.Fax = null;
            validator.ShouldNotHaveValidationErrorFor(x => x.Fax, model);
            model.Fax = "";
            validator.ShouldNotHaveValidationErrorFor(x => x.Fax, model);
        }
        [Test]
        public void Should_not_have_error_when_fax_is_specified()
        {
            var validator = new UserInfoValidator(_localizationService, _stateProvinceService,
                new UserSettings
                {
                    FaxEnabled = true
                });

            var model = new UserInfoModel
            {
                Fax = "Fax"
            };
            validator.ShouldNotHaveValidationErrorFor(x => x.Fax, model);
        }
    }
}
