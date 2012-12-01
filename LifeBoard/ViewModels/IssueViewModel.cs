using LifeBoard.Models;
using System;

namespace LifeBoard.ViewModels
{
    public class IssueViewModel : ViewModelBase
    {
        protected bool Equals(IssueViewModel other)
        {
            return Equals(_issue, other._issue);
        }

        public override int GetHashCode()
        {
            return (_issue != null ? _issue.GetHashCode() : 0);
        }

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
        }

        public string Summary
        {
            get { return _issue.Summary; }
        }

        public string Description
        {
            get { return _issue.Description; }
        }

        public IssueType IssueType
        {
            get { return _issue.Type; }
        }

        public IssueStatus Status
        {
            get { return _issue.Status; }
        }

        public string CreationDate
        {
            get { return _issue.CreationDate.ToShortDateString(); }
        }

        public Issue Model
        {
            get { return _issue; }
        }

        public object Parent
        {
            get { return _parent; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IssueViewModel) obj);
        }

        public void UpdateSource()
        {
            OnPropertyChanged("Summary");
            OnPropertyChanged("Description");
            OnPropertyChanged("Priority");
            OnPropertyChanged("IssueType");
            OnPropertyChanged("Status");
            OnPropertyChanged("CreationDate");
        }
    }
}
