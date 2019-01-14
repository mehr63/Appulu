
namespace Appulu.Web.Framework.Models
{
    /// <summary>
    /// Represents base Appulu entity model
    /// </summary>
    public partial class BaseAppEntityModel : BaseAppModel
    {
        /// <summary>
        /// Gets or sets model identifier
        /// </summary>
        public virtual int Id { get; set; }
    }
}