using FluentValidation;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;
using Appulu.Web.Models.Blogs;

namespace Appulu.Web.Validators.Blogs
{
    public partial class BlogPostValidator : BaseAppValidator<BlogPostModel>
    {
        public BlogPostValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.AddNewComment.CommentText).NotEmpty().WithMessage(localizationService.GetResource("Blog.Comments.CommentText.Required")).When(x => x.AddNewComment != null);
        }
    }
}