using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Models.Configs;
using LifeBoard.ViewModels.Configuration;
using LifeBoard.ViewModels.Dashboard;
using LifeBoard.ViewModels.Issues;
using Issue = LifeBoard.Models.Issue;

namespace LifeBoard.ViewModels
{
    public class MainViewModel : ViewModelBase, INavigatePage
    {
        private PageViewModelBase _current;

        private readonly Board _board;

        private IssueViewModel _actualIssue;

        private PageViewModelBase _backPage;

        private readonly Stack<IssueViewModel> _issueHistory = new Stack<IssueViewModel>();

        private readonly Stack<PageViewModelBase> _navigateHistory = new Stack<PageViewModelBase>();

        public MainViewModel(Board board)
        {
            _board = board;
            Dashboard = new DashboardViewModel(this, board);
            Issues = new IssuesViewModel(this, board);
            Configuration = new ConfigurationViewModel(this, board);
            CreateIssue = new CreateIssueViewModel(this, board);
            EditIssue = new EditIssueViewModel(this, board);
            ShowIssue = new ShowIssueViewModel(this, board);
            _current = Dashboard;
            _backPage = Dashboard;
            _current.IsNavigated = true;
        }

        public PageViewModelBase Current
        {
            get { return _current; }
            set
            {
                if (!Equals(_current, value))
                {
                    _current.IsNavigated = false;
                    _current = value;
                    _current.IsNavigated = true;
                    OnPropertyChanged("Current");
                    OnPropertyChanged("IsDashboardNavigated");
                    OnPropertyChanged("IsIssuesNavigated");
                    OnPropertyChanged("IsConfigurationNavigated");
                }
            }
        }

        public bool IsDashboardNavigated
        {
            get { return _backPage == Dashboard; }
        }

        public bool IsIssuesNavigated
        {
            get { return _backPage == Issues; }
        }

        public bool IsConfigurationNavigated
        {
            get { return _backPage == Configuration; }
        }

        public DashboardViewModel Dashboard { get; private set; }

        public ConfigurationViewModel Configuration { get; private set; }

        public IssuesViewModel Issues { get; private set; }

        public CreateIssueViewModel CreateIssue { get; private set; }

        public EditIssueViewModel EditIssue { get; private set; }

        public ShowIssueViewModel ShowIssue { get; private set; }

        private DelegateCommand<PageViewModelBase> _navigateCommand;

        public ICommand NavigateCommand
        {
            get { return _navigateCommand ?? (_navigateCommand = new DelegateCommand<PageViewModelBase>(Navigate)); }
        }

        private DelegateCommand<IssueViewModel> _showCommand;

        public ICommand ShowCommand
        {
            get { return _showCommand ?? (_showCommand = new DelegateCommand<IssueViewModel>(Show)); }
        }

        public void Show(IssueViewModel issue)
        {
            _actualIssue = issue;
            ShowIssue.SetIssue(issue);
            Navigate(ShowIssue);
        }

        private DelegateCommand<IssueViewModel> _createCommand;

        public ICommand CreateCommand
        {
            get { return _createCommand ?? (_createCommand = new DelegateCommand<IssueViewModel>(Create)); }
        }

        private void Create(IssueViewModel issue)
        {
            Navigate(CreateIssue);
            CreateIssue.AddParent(issue);
        }

        private DelegateCommand<IssueViewModel> _editCommand;

        public ICommand EditCommand
        {
            get { return _editCommand ?? (_editCommand = new DelegateCommand<IssueViewModel>(Edit)); }
        }

        private void Edit(IssueViewModel issue)
        {
            _actualIssue = issue;
            EditIssue.SetIssue(issue.Model);
            Navigate(EditIssue);
        }

        private DelegateCommand<IssueViewModel> _deleteCommand;

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new DelegateCommand<IssueViewModel>(Delete)); }
        }

        private void Delete(IssueViewModel issue)
        {
            Delete(issue, false);
        }

        private DelegateCommand<IssueViewModel> _deleteBackCommand;

        public ICommand DeleteBackCommand
        {
            get { return _deleteBackCommand ?? (_deleteBackCommand = new DelegateCommand<IssueViewModel>(DeleteBack)); }
        }

        private void DeleteBack(IssueViewModel issue)
        {
            Delete(issue, true);
        }

        private void Delete(IssueViewModel issue, bool isGoBack)
        {
            if (MessageBox.Show((string)Application.Current.FindResource("DeleteMessage"),
                (string)Application.Current.FindResource("DeleteHeader"),
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning,
                MessageBoxResult.Cancel) == MessageBoxResult.OK)
            {
                Delete(issue.Model);
                if (isGoBack)
                {
                    Back();
                }
            }
        }

        public void Delete(Issue issue)
        {
            _board.DeleteIssue(issue);
            _board.Submit();
            if (Current == Issues)
            {
                Issues.Search();
            }
            if (Current == ShowIssue)
            {
                ShowIssue.UpdateChildren();
            }
        }


        public void Navigate(PageViewModelBase pageViewModel)
        {
            if (IsRootPage(pageViewModel))
            {
                ClearHistory();
                _backPage = pageViewModel;
            }
            else
            {
                _navigateHistory.Push(pageViewModel);
                _issueHistory.Push(_actualIssue);
            }
            Current = pageViewModel;
        }

        public bool IsRootPage(PageViewModelBase page)
        {
            return page == Dashboard || page == Issues || page == Configuration;
        }

        private DelegateCommand _backCommand;

        public ICommand BackCommand
        {
            get { return _backCommand ?? (_backCommand = new DelegateCommand(Back)); }
        }

        private void Back()
        {
            if (_navigateHistory.Count > 1)
            {
                _navigateHistory.Pop();
                _issueHistory.Pop();
                var lastPage = _navigateHistory.Peek();
                var lastIssue = _issueHistory.Peek();
                if (lastPage is EditIssueViewModel)
                {
                    EditIssue.SetIssue(lastIssue.Model);
                }
                if (lastPage is ShowIssueViewModel)
                {
                    ShowIssue.SetIssue(lastIssue);
                }
                Current = lastPage;
            }
            else
            {
                Navigate(_backPage);
            }
        }

        public void ClearHistory()
        {
            _navigateHistory.Clear();
            _issueHistory.Clear();
            _actualIssue = null;
            _backPage = null;
        }
    }
}