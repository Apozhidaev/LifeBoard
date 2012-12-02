using System.Windows;
using LifeBoard.Models.Configs;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class ConfigIssueViewModel
    /// </summary>
    public class ConfigIssueViewModel : ViewModelBase
    {
        /// <summary>
        /// The _issue
        /// </summary>
        private readonly Issue _issue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigIssueViewModel" /> class.
        /// </summary>
        /// <param name="issue">The issue.</param>
        public ConfigIssueViewModel(Issue issue)
        {
            _issue = issue;
        }

        /// <summary>
        /// Gets the type of the is issue.
        /// </summary>
        /// <value>The type of the is issue.</value>
        public Visibility IsIssueType
        {
            get { return _issue.IsIssueType ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Gets the is priority.
        /// </summary>
        /// <value>The is priority.</value>
        public Visibility IsPriority
        {
            get { return _issue.IsPriority ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Gets the is issue status.
        /// </summary>
        /// <value>The is issue status.</value>
        public Visibility IsIssueStatus
        {
            get { return _issue.IsIssueStatus ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Gets the is creation date.
        /// </summary>
        /// <value>The is creation date.</value>
        public Visibility IsCreationDate
        {
            get { return _issue.IsCreationDate ? Visibility.Visible : Visibility.Collapsed; }
        }
    }
}