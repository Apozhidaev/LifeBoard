using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class TypeViewModel
    /// </summary>
    public class TypeViewModel : ParentViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="issueType">Type of the issue.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        public TypeViewModel(object parent, IssueType issueType, bool isChecked)
            : base(parent)
        {
            IssueType = issueType;
            IsChecked = isChecked;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return IssueType.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets the type of the issue.
        /// </summary>
        /// <value>The type of the issue.</value>
        public IssueType IssueType { get; private set; }
    }
}