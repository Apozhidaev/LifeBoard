namespace LifeBoard.Models
{
    /// <summary>
    /// Class IssueLink
    /// </summary>
    public class IssueLink
    {
        /// <summary>
        /// Gets or sets the child id.
        /// </summary>
        /// <value>The child id.</value>
        public int ChildId { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        /// <value>The parent id.</value>
        public int ParentId { get; set; }
    }
}