using System;
using System.Collections.Generic;

namespace LifeBoard.Models
{
    /// <summary>
    /// Class Issue
    /// </summary>
    public class Issue
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public IssueType Type { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public IssueStatus Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is custom root.
        /// </summary>
        /// <value><c>true</c> if this instance is custom root; otherwise, <c>false</c>.</value>
        public bool IsCustomRoot { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the web site.
        /// </summary>
        /// <value>The web site.</value>
        public List<string> Links { get; set; }

        /// <summary>
        /// Gets the next status.
        /// </summary>
        /// <value>The next status.</value>
        public IssueStatus NextStatus
        {
            get
            {
                if (Status == IssueStatus.Closed)
                {
                    return IssueStatus.Open;
                }
                var status = (int) Status;
                ++status;
                return (IssueStatus) status;
            }
        }

        /// <summary>
        /// Nexts the state.
        /// </summary>
        public void NextState()
        {
            Status = NextStatus;
        }
    }
}