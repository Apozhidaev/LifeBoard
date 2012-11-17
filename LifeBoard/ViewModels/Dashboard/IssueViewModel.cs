using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.ViewModels.Issues;

namespace LifeBoard.ViewModels.Dashboard
{
    public class IssueViewModel:ViewModelBase
    {
        private readonly Issue _issue;

        public IssueViewModel(Issue issue)
        {
            _issue = issue;
        }

        public int Priority
        {
            get { return _issue.Priority; }
            set { _issue.Priority = value; }
        }

        public string Summary
        {
            get { return _issue.Summary; }
            set { _issue.Summary = value; }
        }

        public string Description
        {
            get { return _issue.Description; }
            set { _issue.Description = value; }
        }

        public IssueType IssueType
        {
            get { return _issue.Type; }
            set { _issue.Type = value; }
        }
    }
}
