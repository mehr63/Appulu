using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a templates model
    /// </summary>
    public partial class TemplatesModel : BaseAppModel
    {
        #region Ctor

        public TemplatesModel()
        {
            TemplatesCategory = new CategoryTemplateSearchModel();
            TemplatesManufacturer = new ManufacturerTemplateSearchModel();
            TemplatesProduct = new ProductTemplateSearchModel();
            TemplatesTopic = new TopicTemplateSearchModel();
        }

        #endregion

        #region Properties

        public CategoryTemplateSearchModel TemplatesCategory { get; set; }

        public ManufacturerTemplateSearchModel TemplatesManufacturer { get; set; }

        public ProductTemplateSearchModel TemplatesProduct { get; set; }

        public TopicTemplateSearchModel TemplatesTopic { get; set; }

        #endregion
    }
}
