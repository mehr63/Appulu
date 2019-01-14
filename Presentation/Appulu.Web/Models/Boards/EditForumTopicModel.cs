using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Core.Domain.Forums;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.Boards;

namespace Appulu.Web.Models.Boards
{
    [Validator(typeof(EditForumTopicValidator))]
    public partial class EditForumTopicModel : BaseAppModel
    {
        public EditForumTopicModel()
        {
            TopicPriorities = new List<SelectListItem>();
        }

        public bool IsEdit { get; set; }

        public int Id { get; set; }

        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public string ForumSeName { get; set; }

        public int TopicTypeId { get; set; }
        public EditorType ForumEditor { get; set; }
        
        public string Subject { get; set; }
        	
        public string Text { get; set; }
        
        public bool IsUserAllowedToSetTopicPriority { get; set; }
        public IEnumerable<SelectListItem> TopicPriorities { get; set; }

        public bool IsUserAllowedToSubscribe { get; set; }
        public bool Subscribed { get; set; }

    }
}