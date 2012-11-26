using LifeBoard.Models;
using System;

namespace LifeBoard.ViewModels
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
    }
}
