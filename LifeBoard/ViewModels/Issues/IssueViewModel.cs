using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace LifeBoard.ViewModels.Issues
{
    public class IssueViewModel
    {
        private readonly Issue _issue;

        private readonly IssuesViewModel _parent;

        public IssueViewModel(IssuesViewModel parent, Issue issue)
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

        private DelegateCommand _showCommand;

        public ICommand ShowCommand
        {
            get { return _showCommand ?? (_showCommand = new DelegateCommand(Show)); }
        }

        public void Show()
        {
            _parent.ShowIssue.Show(_issue);
        }

        private DelegateCommand _editCommand;

        public ICommand EditCommand
        {
            get { return _editCommand ?? (_editCommand = new DelegateCommand(Edit)); }
        }

        private void Edit()
        {
            _parent.EditIssue.Edit(_issue);
        }

        private DelegateCommand _deleteCommand;

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new DelegateCommand(Delete)); }
        }

        private void Delete()
        {
            _parent.Delete(_issue);
        }
    }
}
