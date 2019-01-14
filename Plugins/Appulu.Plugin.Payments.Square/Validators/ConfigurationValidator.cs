using System;
using FluentValidation;
using Appulu.Plugin.Payments.Square.Models;
using Appulu.Web.Framework.Validators;

namespace Appulu.Plugin.Payments.Square.Validators
{
    /// <summary>
    /// Represents validator of the Square payment plugin configuration model
    /// </summary>
    public partial class ConfigurationModelValidator : BaseAppValidator<ConfigurationModel>
    {
        public ConfigurationModelValidator()
        {
            //rules for sandbox credentials
            RuleFor(model => model.AccessToken).Must((model, context) =>
            {
                //do not validate for production credentials
                if (!model.UseSandbox)
                    return true;

                return !string.IsNullOrEmpty(model.AccessToken) && 
                    model.AccessToken.StartsWith(SquarePaymentDefaults.SandboxCredentialsPrefix, StringComparison.InvariantCultureIgnoreCase);
            }).WithMessage($"Sandbox access token should start with '{SquarePaymentDefaults.SandboxCredentialsPrefix}'");

            RuleFor(model => model.ApplicationId).Must((model, context) =>
            {
                //do not validate for production credentials
                if (!model.UseSandbox)
                    return true;

                return !string.IsNullOrEmpty(model.ApplicationId) && 
                    model.ApplicationId.StartsWith(SquarePaymentDefaults.SandboxCredentialsPrefix, StringComparison.InvariantCultureIgnoreCase);
            }).WithMessage($"Sandbox application ID should start with '{SquarePaymentDefaults.SandboxCredentialsPrefix}'");
        }
    }
}