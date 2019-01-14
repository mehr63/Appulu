using System;
using Appulu.Core.Domain.Users;

namespace Appulu.Core.Domain.Forums
{
    /// <summary>
    /// Represents a forum post
    /// </summary>
    public partial class ForumPost : BaseEntity
    {
        /// <summary>
        /// Gets or sets the forum topic identifier
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the count of votes
        /// </summary>
        public int VoteCount { get; set; }

        /// <summary>
        /// Gets the topic
        /// </summary>
        public virtual ForumTopic ForumTopic { get; set; }

        /// <summary>
        /// Gets the user
        /// </summary>
        public virtual User User { get; set; }
    }
}
