using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Vendors;
using Appulu.Core.Domain.Vendors;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Services.Seo;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Vendors
{
    public partial class VendorValidator : BaseAppValidator<VendorModel>
    {
        public VendorValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vendors.Fields.Name.Required"));

            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vendors.Fields.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));
            RuleFor(x => x.PageSizeOptions).Must(ValidatorUtilities.PageSizeOptionsValidator).WithMessage(localizationService.GetResource("Admin.Vendors.Fields.PageSizeOptions.ShouldHaveUniqueItems"));
            RuleFor(x => x.PageSize).Must((x, context) =>
            {
                if (!x.AllowUsersToSelectPageSize && x.PageSize <= 0)
                    return false;

                return true;
            }).WithMessage(localizationService.GetResource("Admin.Vendors.Fields.PageSize.Positive"));
            RuleFor(x => x.SeName).Length(0, AppSeoDefaults.SearchEngineNameLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), AppSeoDefaults.SearchEngineNameLength));

            SetDatabaseValidationRules<Vendor>(dbContext);
        }
    }
}