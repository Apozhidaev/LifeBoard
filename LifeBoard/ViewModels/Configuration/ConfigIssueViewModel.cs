using LifeBoard.Models.Configs;

namespace LifeBoard.ViewModels.Configuration
{
    public class ConfigIssueViewModel : ViewModelBase
    {
        private readonly Issue _issue;

        public ConfigIssueViewModel(Issue issue)
        {
            _issue = issue;
        }

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
    }
}
