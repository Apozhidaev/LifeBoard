using System.Windows;
using LifeBoard.Models.Configs;

namespace LifeBoard.ViewModels.Issues
{
    public class ConfigIssueViewModel : ViewModelBase
    {
        private readonly Issue _issue;

        public ConfigIssueViewModel(Issue issue)
        {
            _issue = issue;
        }

        public Visibility IsIssueType
        {
            get { return _issue.IsIssueType ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility IsPriority
        {
            get { return _issue.IsPriority ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility IsIssueStatus
        {
            get { return _issue.IsIssueStatus ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility IsCreationDate
        {
            get { return _issue.IsCreationDate ? Visibility.Visible : Visibility.Collapsed; }
        }
    }
}
