﻿using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Templates;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Templates
{
    public partial class TopicTemplateValidator : BaseAppValidator<TopicTemplateModel>
    {
        public TopicTemplateValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.Templates.Topic.Name.Required"));
            RuleFor(x => x.ViewPath).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.Templates.Topic.ViewPath.Required"));
        }
    }
}