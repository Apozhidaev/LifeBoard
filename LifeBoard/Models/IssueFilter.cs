using System;
using System.Collections.Generic;

namespace LifeBoard.Models
{
    /// <summary>
    /// Class IssueFilter
    /// </summary>
    public class IssueFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IssueFilter" /> class.
        /// </summary>
        public IssueFilter()
        {
            Query = String.Empty;
        }

        public bool HasDeadline { get; set; }

        public bool IsActualDeadline { get; set; }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        public String Query { get; set; }

        /// <summary>
        /// Gets or sets the priorities.
        /// </summary>
        /// <value>The priorities.</value>
        public HashSet<int> Priorities { get; set; }

        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>The types.</value>
        public HashSet<IssueType> Types { get; set; }

        /// <summary>
        /// Gets or sets the statuses.
        /// </summary>
        /// <value>The statuses.</value>
        public HashSet<IssueStatus> Statuses { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has query.
        /// </summary>
        /// <value><c>true</c> if this instance has query; otherwise, <c>false</c>.</value>
        public bool HasQuery
        {
            get { return !String.IsNullOrEmpty(Query); }
        }
    }
}