using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class ShowIssueViewModel : PageViewModelBase
    {
        private Issue _issue;

        private readonly BoardService _boardService;

        private ShowIssueView _showIssueView;

        private readonly IssuesViewModel _parent;

        public ShowIssueViewModel(IssuesViewModel parent, BoardService boardService)
            : base(parent.Frame)
        {
            _parent = parent;
            _boardService = boardService;
        }

        public string Summary
        {
            get { return _issue.Summary; }
        }

        public string Description
        {
            get { return _issue.Description; }
        }

        public int Priority
        {
            get { return _issue.Priority; }
        }

        public IssueType IssueType
        {
            get { return _issue.Type; }
        }

        public IssueStatus Status
        {
            get { return _issue.Status; }
        }

        public IssueStatus NextStatus
        {
            get { return _issue.NextStatus; }
        }

        public override Page Page
        {
            get { return _showIssueView ?? (_showIssueView = new ShowIssueView(this)); }
        }

        public void Show(Issue issue)
        {
            _issue = issue;
            UpdateSource();
            Navigate();
        }

        #region Commands

        public ICommand BackNavigateCommand
        {
            get { return _parent.NavigateCommand; }
        }

        #endregion

        private void UpdateSource()
        {
            OnPropertyChanged("Summary");
            OnPropertyChanged("Description");
            OnPropertyChanged("Priority");
            OnPropertyChanged("IssueType");
            OnPropertyChanged("Status");
            OnPropertyChanged("NextStatus");
        }

        private DelegateCommand _nextStateCommand;

        public ICommand NextStateCommand
        {
            get { return _nextStateCommand ?? (_nextStateCommand = new DelegateCommand(NextState)); }
        }

        private void NextState()
        {
            _issue.NextState();
            OnPropertyChanged("Status");
            OnPropertyChanged("NextStatus");
        }
    }
}
