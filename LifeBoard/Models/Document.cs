using System.Collections.Generic;

namespace LifeBoard.Models
{
    /// <summary>
    /// Class Document
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Gets or sets the issues.
        /// </summary>
        /// <value>The issues.</value>
        public Dictionary<int, Issue> Issues { get; set; }

        /// <summary>
        /// Gets or sets the issues links.
        /// </summary>
        /// <value>The issues links.</value>
        public List<IssueLink> IssuesLinks { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return Issues == null || Issues.Count == 0; }
        }
    }
}