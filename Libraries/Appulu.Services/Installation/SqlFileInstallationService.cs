using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Appulu.Core;
using Appulu.Core.Data;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Localization;
using Appulu.Core.Domain.Stores;
using Appulu.Core.Infrastructure;
using Appulu.Data;
using Appulu.Services.Users;
using Appulu.Services.Localization;

namespace Appulu.Services.Installation
{
    /// <summary>
    /// Installation service using SQL files (fast installation)
    /// </summary>
    public partial class SqlFileInstallationService : IInstallationService
    {
        #region Fields

        private readonly IDbContext _dbContext;
        private readonly IAppFileProvider _fileProvider;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public SqlFileInstallationService(IDbContext dbContext,
            IAppFileProvider fileProvider,
            IRepository<User> userRepository,
            IRepository<Language> languageRepository,
            IRepository<Store> storeRepository,
            IWebHelper webHelper)
        {
            this._dbContext = dbContext;
            this._fileProvider = fileProvider;
            this._userRepository = userRepository;
            this._languageRepository = languageRepository;
            this._storeRepository = storeRepository;
            this._webHelper = webHelper;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Install locales
        /// </summary>
        protected virtual void InstallLocaleResources()
        {
            //'English' language
            var language = _languageRepository.Table.Single(l => l.Name == "English");

            //save resources
            var directoryPath = _fileProvider.MapPath(AppInstallationDefaults.LocalizationResourcesPath);
            var pattern = $"*.{AppInstallationDefaults.LocalizationResourcesFileExtension}";
            foreach (var filePath in _fileProvider.EnumerateFiles(directoryPath, pattern))
            {
                var localesXml = _fileProvider.ReadAllText(filePath, Encoding.UTF8);
                var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
                localizationService.ImportResourcesFromXml(language, localesXml);
            }
        }

        /// <summary>
        /// Update default user
        /// </summary>
        /// <param name="defaultUserEmail">Email</param>
        /// <param name="defaultUserPassword">Password</param>
        protected virtual void UpdateDefaultUser(string defaultUserEmail, string defaultUserPassword)
        {
            var adminUser = _userRepository.Table.Single(x => x.Email == "admin@yourStore.com");
            if (adminUser == null)
                throw new Exception("Admin user cannot be loaded");

            adminUser.UserGuid = Guid.NewGuid();
            adminUser.Email = defaultUserEmail;
            adminUser.Username = defaultUserEmail;
            _userRepository.Update(adminUser);

            var userRegistrationService = EngineContext.Current.Resolve<IUserRegistrationService>();
            userRegistrationService.ChangePassword(new ChangePasswordRequest(defaultUserEmail, false,
                 PasswordFormat.Hashed, defaultUserPassword));
        }

        /// <summary>
        /// Update default store URL
        /// </summary>
        protected virtual void UpdateDefaultStoreUrl()
        {
            var store = _storeRepository.Table.FirstOrDefault();
            if (store == null)
                throw new Exception("Default store cannot be loaded");

            store.Url = _webHelper.GetStoreLocation(false);
            _storeRepository.Update(store);
        }

        /// <summary>
        /// Execute SQL file
        /// </summary>
        /// <param name="path">File path</param>
        protected virtual void ExecuteSqlFile(string path)
        {
            var statements = new List<string>();

            using (var reader = new StreamReader(path))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                    statements.Add(statement);
            }

            foreach (var stmt in statements)
                _dbContext.ExecuteSqlCommand(stmt);
        }

        /// <summary>
        /// Read next statement from stream
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <returns>Result</returns>
        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();

                    return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Install data
        /// </summary>
        /// <param name="defaultUserEmail">Default user email</param>
        /// <param name="defaultUserPassword">Default user password</param>
        /// <param name="installSampleData">A value indicating whether to install sample data</param>
        public virtual void InstallData(string defaultUserEmail,
            string defaultUserPassword, bool installSampleData = true)
        {
            ExecuteSqlFile(_fileProvider.MapPath(AppInstallationDefaults.RequiredDataPath));
            InstallLocaleResources();
            UpdateDefaultUser(defaultUserEmail, defaultUserPassword);
            UpdateDefaultStoreUrl();

            if (installSampleData)
            {
                ExecuteSqlFile(_fileProvider.MapPath(AppInstallationDefaults.SampleDataPath));
            }
        }

        #endregion
    }
}