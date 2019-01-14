using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Topics;
using Appulu.Services.Users;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Security;
using Appulu.Services.Seo;
using Appulu.Services.Stores;
using Appulu.Services.Topics;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Appulu.Web.Areas.Admin.Models.Topics;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class TopicController : BaseAdminController
    {
        #region Fields

        private readonly IAclService _aclService;
        private readonly IUserActivityService _userActivityService;
        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStoreService _storeService;
        private readonly ITopicModelFactory _topicModelFactory;
        private readonly ITopicService _topicService;
        private readonly IUrlRecordService _urlRecordService;

        #endregion Fields

        #region Ctor

        public TopicController(IAclService aclService,
            IUserActivityService userActivityService,
            IUserService userService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            IStoreMappingService storeMappingService,
            IStoreService storeService,
            ITopicModelFactory topicModelFactory,
            ITopicService topicService,
            IUrlRecordService urlRecordService)
        {
            this._aclService = aclService;
            this._userActivityService = userActivityService;
            this._userService = userService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._storeMappingService = storeMappingService;
            this._storeService = storeService;
            this._topicModelFactory = topicModelFactory;
            this._topicService = topicService;
            this._urlRecordService = urlRecordService;
        }

        #endregion

        #region Utilities

        protected virtual void UpdateLocales(Topic topic, TopicModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(topic,
                    x => x.Title,
                    localized.Title,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(topic,
                    x => x.Body,
                    localized.Body,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(topic,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(topic,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(topic,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                //search engine name
                var seName = _urlRecordService.ValidateSeName(topic, localized.SeName, localized.Title, false);
                _urlRecordService.SaveSlug(topic, seName, localized.LanguageId);
            }
        }

        protected virtual void SaveTopicAcl(Topic topic, TopicModel model)
        {
            topic.SubjectToAcl = model.SelectedUserRoleIds.Any();

            var existingAclRecords = _aclService.GetAclRecords(topic);
            var allUserRoles = _userService.GetAllUserRoles(true);
            foreach (var userRole in allUserRoles)
            {
                if (model.SelectedUserRoleIds.Contains(userRole.Id))
                {
                    //new role
                    if (existingAclRecords.Count(acl => acl.UserRoleId == userRole.Id) == 0)
                        _aclService.InsertAclRecord(topic, userRole.Id);
                }
                else
                {
                    //remove role
                    var aclRecordToDelete = existingAclRecords.FirstOrDefault(acl => acl.UserRoleId == userRole.Id);
                    if (aclRecordToDelete != null)
                        _aclService.DeleteAclRecord(aclRecordToDelete);
                }
            }
        }

        protected virtual void SaveStoreMappings(Topic topic, TopicModel model)
        {
            topic.LimitedToStores = model.SelectedStoreIds.Any();

            var existingStoreMappings = _storeMappingService.GetStoreMappings(topic);
            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        _storeMappingService.InsertStoreMapping(topic, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        _storeMappingService.DeleteStoreMapping(storeMappingToDelete);
                }
            }
        }

        #endregion

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            //prepare model
            var model = _topicModelFactory.PrepareTopicSearchModel(new TopicSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(TopicSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _topicModelFactory.PrepareTopicListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            //prepare model
            var model = _topicModelFactory.PrepareTopicModel(new TopicModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(TopicModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                if (!model.IsPasswordProtected)
                    model.Password = null;

                var topic = model.ToEntity<Topic>();
                _topicService.InsertTopic(topic);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(topic, model.SeName, topic.Title ?? topic.SystemName, true);
                _urlRecordService.SaveSlug(topic, model.SeName, 0);

                //ACL (user roles)
                SaveTopicAcl(topic, model);

                //stores
                SaveStoreMappings(topic, model);

                //locales
                UpdateLocales(topic, model);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Topics.Added"));

                //activity log
                _userActivityService.InsertActivity("AddNewTopic",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewTopic"), topic.Title ?? topic.SystemName), topic);

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = topic.Id });
            }

            //prepare model
            model = _topicModelFactory.PrepareTopicModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            //try to get a topic with the specified id
            var topic = _topicService.GetTopicById(id);
            if (topic == null)
                return RedirectToAction("List");

            //prepare model
            var model = _topicModelFactory.PrepareTopicModel(null, topic);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(TopicModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            //try to get a topic with the specified id
            var topic = _topicService.GetTopicById(model.Id);
            if (topic == null)
                return RedirectToAction("List");

            if (!model.IsPasswordProtected)
                model.Password = null;

            if (ModelState.IsValid)
            {
                topic = model.ToEntity(topic);
                _topicService.UpdateTopic(topic);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(topic, model.SeName, topic.Title ?? topic.SystemName, true);
                _urlRecordService.SaveSlug(topic, model.SeName, 0);

                //ACL (user roles)
                SaveTopicAcl(topic, model);

                //stores
                SaveStoreMappings(topic, model);

                //locales
                UpdateLocales(topic, model);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Topics.Updated"));

                //activity log
                _userActivityService.InsertActivity("EditTopic",
                    string.Format(_localizationService.GetResource("ActivityLog.EditTopic"), topic.Title ?? topic.SystemName), topic);

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = topic.Id });
            }

            //prepare model
            model = _topicModelFactory.PrepareTopicModel(model, topic, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            //try to get a topic with the specified id
            var topic = _topicService.GetTopicById(id);
            if (topic == null)
                return RedirectToAction("List");

            _topicService.DeleteTopic(topic);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Topics.Deleted"));

            //activity log
            _userActivityService.InsertActivity("DeleteTopic",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteTopic"), topic.Title ?? topic.SystemName), topic);

            return RedirectToAction("List");
        }

        #endregion
    }
}