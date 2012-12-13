using LifeBoard.Models.Configs;

namespace LifeBoard.ViewModels.Configuration
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
        /// Gets or sets a value indicating whether this instance is issue type.
        /// </summary>
        /// <value><c>true</c> if this instance is issue type; otherwise, <c>false</c>.</value>
        public bool IsIssueType
        {
            get { return _issue.IsIssueType; }
            set
            {
                if (_issue.IsIssueType != value)
                {
                    _issue.IsIssueType = value;
                    ConfigRepository.Save();
                    OnPropertyChanged("IsIssueType");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is priority.
        /// </summary>
        /// <value><c>true</c> if this instance is priority; otherwise, <c>false</c>.</value>
        public bool IsPriority
        {
            get { return _issue.IsPriority; }
            set
            {
                if (_issue.IsPriority != value)
                {
                    _issue.IsPriority = value;
                    ConfigRepository.Save();
                    OnPropertyChanged("IsPriority");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is issue status.
        /// </summary>
        /// <value><c>true</c> if this instance is issue status; otherwise, <c>false</c>.</value>
        public bool IsIssueStatus
        {
            get { return _issue.IsIssueStatus; }
            set
            {
                if (_issue.IsIssueStatus != value)
                {
                    _issue.IsIssueStatus = value;
                    ConfigRepository.Save();
                    OnPropertyChanged("IsIssueStatus");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is creation date.
        /// </summary>
        /// <value><c>true</c> if this instance is creation date; otherwise, <c>false</c>.</value>
        public bool IsCreationDate
        {
            get { return _issue.IsCreationDate; }
            set
            {
                if (_issue.IsCreationDate != value)
                {
                    _issue.IsCreationDate = value;
                    ConfigRepository.Save();
                    OnPropertyChanged("IsCreationDate");
                }
            }
        }

        public bool IsDeadline
        {
            get { return _issue.IsDeadline; }
            set
            {
                if (_issue.IsDeadline != value)
                {
                    _issue.IsDeadline = value;
                    ConfigRepository.Save();
                    OnPropertyChanged("IsDeadline");
                }
            }
        }
    }
}