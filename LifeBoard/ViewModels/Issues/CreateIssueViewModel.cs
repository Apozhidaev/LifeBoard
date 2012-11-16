using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class CreateIssueViewModel : PageViewModelBase
    {
        private int _priority;

        private IssueType _type;

        private string _summary;

        private string _description;

        private readonly BoardService _boardService;

        private CreateIssueView _createIssueView;

        private readonly IssuesViewModel _parent;

        public CreateIssueViewModel(IssuesViewModel parent, BoardService boardService)
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
            get { return _createIssueView ?? (_createIssueView = new CreateIssueView(this)); }
        }

        private void ClearForm()
        {
            Type = IssueType.Task;
            Priority = 3;
            Summary = String.Empty;
            Description = String.Empty;
        }

        public override void Navigate()
        {
            ClearForm();
            base.Navigate();
        }

        #region Commands

        private DelegateCommand _createCommand;

        public ICommand CreateCommand
        {
            get { return _createCommand ?? (_createCommand = new DelegateCommand(Create, CanCreate)); }
        }

        private void Create()
        {
            _boardService.CreateIssue(Type, Priority, Summary, Description);
            _parent.Navigate();
        }

        private bool CanCreate()
        {
            return !String.IsNullOrEmpty(Summary);
        }

        private DelegateCommand _backNavigateCommand;

        public ICommand BackNavigateCommand
        {
            get { return _backNavigateCommand ?? (_backNavigateCommand = new DelegateCommand(BackNavigate)); }
        }

        private void BackNavigate()
        {
            _parent.Navigate();
        }

        #endregion
    }
}
