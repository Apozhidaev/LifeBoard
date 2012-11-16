using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class EditIssueViewModel : PageViewModelBase
    {
        private Issue _issue;

        private int _priority;

        private IssueType _type;

        private string _summary;

        private string _description;

        private readonly BoardService _boardService;

        private EditIssueView _editIssueView;

        private readonly IssuesViewModel _parent;

        public EditIssueViewModel(IssuesViewModel parent, BoardService boardService)
            : base(parent.FramePage)
        {
            _parent = parent;
            _boardService = boardService;
        }

        public int Priority
        {
            get { return _priority; }
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged("Priority");
                }
            }
        }

        public IssueType Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public IEnumerable<int> Priorities
        {
            get { return _boardService.GetPriorities(); }
        }

        public IEnumerable<IssueType> Types
        {
            get { return _boardService.GetTypes(); }
        }

        public string Summary
        {
            get { return _summary; }
            set
            {
                if (_summary != value)
                {
                    _summary = value;
                    OnPropertyChanged("Summary");
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public override Page Page
        {
            get { return _editIssueView ?? (_editIssueView = new EditIssueView(this)); }
        }

        public void Edit(Issue issue)
        {
            _issue = issue;
            FromIssue();
            Navigate();
        }

        #region Commands

        private DelegateCommand _editCommand;

        public ICommand EditCommand
        {
            get { return _editCommand ?? (_editCommand = new DelegateCommand(Edit, CanEdit)); }
        }

        private void Edit()
        {
            ToIssue();
           _parent.Navigate();
        }

        private bool CanEdit()
        {
            return !String.IsNullOrEmpty(Summary);
        }

        public ICommand BackNavigateCommand
        {
            get { return _parent.NavigateCommand; }
        }

        #endregion

        private void FromIssue()
        {
            Type = _issue.Type;
            Priority = _issue.Priority;
            Summary = _issue.Summary;
            Description = _issue.Description;
        }

        private void ToIssue()
        {
            _issue.Type = Type;
            _issue.Priority = Priority;
            _issue.Summary = Summary;
            _issue.Description = Description;
        }
    }
}
