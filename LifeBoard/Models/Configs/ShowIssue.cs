namespace LifeBoard.Models.Configs
{
    /// <summary>
    /// Class ShowIssue
    /// </summary>
    public class ShowIssue
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is history as parents.
        /// </summary>
        /// <value><c>true</c> if this instance is history as parents; otherwise, <c>false</c>.</value>
        public bool IsHistoryAsParents { get; set; }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>The table.</value>
        public Issue Table { get; set; }

        /// <summary>
        /// Gets or sets the sitebar.
        /// </summary>
        /// <value>The sitebar.</value>
        public Issue Sitebar { get; set; }
    }
}