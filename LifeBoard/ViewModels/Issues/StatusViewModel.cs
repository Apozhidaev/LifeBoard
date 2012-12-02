using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class StatusViewModel
    /// </summary>
    public class StatusViewModel : ParentViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="status">The status.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        public StatusViewModel(object parent, IssueStatus status, bool isChecked)
            : base(parent)
        {
            IssueStatus = status;
            IsChecked = isChecked;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return IssueStatus.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets the issue status.
        /// </summary>
        /// <value>The issue status.</value>
        public IssueStatus IssueStatus { get; private set; }
    }
}