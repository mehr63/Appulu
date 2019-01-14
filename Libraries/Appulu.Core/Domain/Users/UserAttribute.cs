using System.Collections.Generic;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Localization;

namespace Appulu.Core.Domain.Users
{
    /// <summary>
    /// Represents a user attribute
    /// </summary>
    public partial class UserAttribute : BaseEntity, ILocalizedEntity
    {
        private ICollection<UserAttributeValue> _userAttributeValues;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attribute is required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the attribute control type identifier
        /// </summary>
        public int AttributeControlTypeId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets the attribute control type
        /// </summary>
        public AttributeControlType AttributeControlType
        {
            get => (AttributeControlType)AttributeControlTypeId;
            set => AttributeControlTypeId = (int)value;
        }

        /// <summary>
        /// Gets the user attribute values
        /// </summary>
        public virtual ICollection<UserAttributeValue> UserAttributeValues
        {
            get => _userAttributeValues ?? (_userAttributeValues = new List<UserAttributeValue>());
            protected set => _userAttributeValues = value;
        }
    }
}
