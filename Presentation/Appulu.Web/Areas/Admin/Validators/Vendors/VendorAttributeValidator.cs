using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Vendors;
using Appulu.Core.Domain.Vendors;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Vendors
{
    public partial class VendorAttributeValidator : BaseAppValidator<VendorAttributeModel>
    {
        public VendorAttributeValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vendors.VendorAttributes.Fields.Name.Required"));

            SetDatabaseValidationRules<VendorAttribute>(dbContext);
        }
    }
}