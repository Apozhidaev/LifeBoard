namespace LifeBoard.Models.Configs
{
    /// <summary>
    /// Class Issue
    /// </summary>
    public class Issue
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is issue type.
        /// </summary>
        /// <value><c>true</c> if this instance is issue type; otherwise, <c>false</c>.</value>
        public bool IsIssueType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is priority.
        /// </summary>
        /// <value><c>true</c> if this instance is priority; otherwise, <c>false</c>.</value>
        public bool IsPriority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is issue status.
        /// </summary>
        /// <value><c>true</c> if this instance is issue status; otherwise, <c>false</c>.</value>
        public bool IsIssueStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is creation date.
        /// </summary>
        /// <value><c>true</c> if this instance is creation date; otherwise, <c>false</c>.</value>
        public bool IsCreationDate { get; set; }
    }
}