using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels
{
    public class ShowIssueViewModel : PageViewModelBase
    {
        private Issue _issue;

        private IssueViewModel _issueModel;

        private readonly BoardService _boardService;

        private ShowIssueView _showIssueView;

        public ShowIssueViewModel(INavigatePage parent, BoardService boardService)
            : base(parent)
        {
            _boardService = boardService;
            Children = new ObservableCollection<IssueViewModel>();
        }

        public IssueViewModel IssueModel
        {
            get { return _issueModel; }
        }

        public ObservableCollection<IssueViewModel> Children { get; private set; }

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

        public Visibility ChildrenVisibility
        {
            get { return Children.Count == 0 ? Visibility.Collapsed : Visibility.Visible; }
        }

        public void SetIssue(IssueViewModel issue)
        {
            _issueModel = issue;
            _issue = issue.Model;
            Children.Clear();
            foreach (var child in _boardService.GetChildren(_issue.Id))
            {
                Children.Add(new IssueViewModel(this, child));
            }
            UpdateSource();
        }

        private void UpdateSource()
        {
            OnPropertyChanged("IssueModel");
            OnPropertyChanged("Summary");
            OnPropertyChanged("Description");
            OnPropertyChanged("Priority");
            OnPropertyChanged("IssueType");
            OnPropertyChanged("Status");
            OnPropertyChanged("NextStatus");
            OnPropertyChanged("ChildrenVisibility");
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
