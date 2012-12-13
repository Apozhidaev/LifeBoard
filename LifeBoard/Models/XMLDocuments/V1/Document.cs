namespace LifeBoard.Models.XMLDocuments.V1
{
    /// <summary>
    /// Class Document
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document" /> class.
        /// </summary>
        public Document()
        {
            Version = new Version {Major = 1, Minor = 0};
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets the issues.
        /// </summary>
        /// <value>The issues.</value>
        public Issue[] Issues { get; set; }

        /// <summary>
        /// Gets or sets the issues links.
        /// </summary>
        /// <value>The issues links.</value>
        public IssueLinks[] IssuesLinks { get; set; }
    }
}