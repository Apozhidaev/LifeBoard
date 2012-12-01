using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private bool _isHistoryChecked;

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

        //private DelegateCommand<IssueViewModel> _historyBackCommand;

        //public ICommand HistoryBackCommand
        //{
        //    get { return _historyBackCommand ?? (_historyBackCommand = new DelegateCommand<IssueViewModel>(DeleteBack)); }
        //}

        //private void HistoryBack(IssueViewModel issue)
        //{
            
        //}

        public ObservableCollection<IssueViewModel> ShowHistory
        {
            get { return _showHistory; }
        }

        private readonly ObservableCollection<IssueViewModel> _showHistory = new ObservableCollection<IssueViewModel>();

        public bool IsHistoryChecked
        {
            get { return _isHistoryChecked; }
            set
            {
                if (_isHistoryChecked != value)
                {
                    _isHistoryChecked = value;
                    OnPropertyChanged("IsHistoryChecked");
                }
            }
        }

        public bool IsHistoryEnabled
        {
            get { return ShowHistory.Count != 0; }
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
                }
            }
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
            if(issue == null || (issue.Equals(_actualIssue)))
            {
                return;
            }

            SetShowHistory(issue);

            _actualIssue = issue;
            ShowIssue.SetIssue(issue);
            Navigate(ShowIssue);
        }

        private void SetShowHistory(IssueViewModel issue)
        {
            IsHistoryChecked = false;
            var removeList = new List<IssueViewModel>();
            int index = _showHistory.IndexOf(issue);
            if (index >= 0)
            {
                for (int i = _showHistory.Count - 1; i >= index; i--)
                {
                    removeList.Add(_showHistory[i]);
                }
                foreach (var issueViewModel in removeList)
                {
                    _showHistory.Remove(issueViewModel);
                }
            }
            _showHistory.Add(issue);
            OnPropertyChanged("IsHistoryEnabled");
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
            if(issue == null)
            {
                return;
            }
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
            if(issue == null)
            {
                return;
            }
            Delete(issue, false);
        }

        private DelegateCommand<IssueViewModel> _deleteBackCommand;

        public ICommand DeleteBackCommand
        {
            get { return _deleteBackCommand ?? (_deleteBackCommand = new DelegateCommand<IssueViewModel>(DeleteBack)); }
        }

        private void DeleteBack(IssueViewModel issue)
        {
            if (issue == null)
            {
                return;
            }
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
                Issues.AsyncSearch();
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
                    _actualIssue = lastIssue;
                    EditIssue.SetIssue(lastIssue.Model);
                }
                if (lastPage is ShowIssueViewModel)
                {
                    _actualIssue = lastIssue;
                    SetShowHistory(lastIssue);
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
            _showHistory.Clear();
            _navigateHistory.Clear();
            _issueHistory.Clear();
            _actualIssue = null;
            _backPage = null;
        }
    }
}