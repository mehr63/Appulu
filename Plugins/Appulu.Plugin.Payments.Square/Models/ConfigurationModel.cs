using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Plugin.Payments.Square.Validators;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Payments.Square.Models
{
    [Validator(typeof(ConfigurationModelValidator))]
    public class ConfigurationModel : BaseAppModel
    {
        #region Ctor

        public ConfigurationModel()
        {
            Locations = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.ApplicationId")]
        public string ApplicationId { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.SandboxApplicationId")]
        public string SandboxApplicationId { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.ApplicationSecret")]
        [DataType(DataType.Password)]
        [NoTrim]
        public string ApplicationSecret { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.AccessToken")]
        [DataType(DataType.Password)]
        [NoTrim]
        public string AccessToken { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.SandboxAccessToken")]
        public string SandboxAccessToken { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.UseSandbox")]
        public bool UseSandbox { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.TransactionMode")]
        public int TransactionModeId { get; set; }
        public SelectList TransactionModes { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.Location")]
        public string LocationId { get; set; }
        public IList<SelectListItem> Locations { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.AdditionalFee")]
        public decimal AdditionalFee { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Square.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }

        #endregion
    }
}