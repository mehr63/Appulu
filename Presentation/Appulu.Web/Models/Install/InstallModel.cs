using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Core.Data;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Validators.Install;

namespace Appulu.Web.Models.Install
{
    [Validator(typeof(InstallValidator))]
    public partial class InstallModel : BaseAppModel
    {
        public InstallModel()
        {
            this.AvailableLanguages = new List<SelectListItem>();
        }

        public string AdminEmail { get; set; }
        [NoTrim]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }
        [NoTrim]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string DatabaseConnectionString { get; set; }
        public DataProviderType DataProvider { get; set; }
        //SQL Server properties
        public string SqlConnectionInfo { get; set; }

        public string SqlServerName { get; set; }
        public string SqlDatabaseName { get; set; }
        public string SqlServerUsername { get; set; }
        [DataType(DataType.Password)]
        public string SqlServerPassword { get; set; }
        public string SqlAuthenticationType { get; set; }
        public bool SqlServerCreateDatabase { get; set; }

        public bool UseCustomCollation { get; set; }
        public string Collation { get; set; }

        public bool DisableSampleDataOption { get; set; }
        public bool InstallSampleData { get; set; }
        public List<SelectListItem> AvailableLanguages { get; set; }
    }
}