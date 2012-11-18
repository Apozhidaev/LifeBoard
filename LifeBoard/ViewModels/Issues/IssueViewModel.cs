using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class IssueViewModel : ViewModelBase
    {
        private readonly Issue _issue;

        private readonly object _parent;

        public IssueViewModel(object parent, Issue issue)
        {
            _parent = parent;
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

        public IssueStatus Status
        {
            get { return _issue.Status; }
            set { _issue.Status = value; }
        }

        public Issue Model
        {
            get { return _issue; }
        }

        public object Parent
        {
            get { return _parent; }
        }
    }
}
