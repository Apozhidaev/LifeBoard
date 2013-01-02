using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.ViewModels.Configuration;
using LifeBoard.ViewModels.Dashboard;
using LifeBoard.ViewModels.Issues;

namespace LifeBoard.ViewModels
{
    /// <summary>
    /// Class MainViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase, INavigatePage
    {
        /// <summary>
        /// The _board
        /// </summary>
        private readonly Board _board;
        /// <summary>
        /// The _issue history
        /// </summary>
        private readonly Stack<IssueViewModel> _issueHistory = new Stack<IssueViewModel>();
        /// <summary>
        /// The _navigate history
        /// </summary>
        private readonly Stack<PageViewModelBase> _navigateHistory = new Stack<PageViewModelBase>();
        /// <summary>
        /// The _show history
        /// </summary>
        private readonly ObservableCollection<IssueViewModel> _showHistory = new ObservableCollection<IssueViewModel>();
        /// <summary>
        /// The _actual issue
        /// </summary>
        private IssueViewModel _actualIssue;
        /// <summary>
        /// The _back command
        /// </summary>
        private DelegateCommand _backCommand;
        /// <summary>
        /// The _back page
        /// </summary>
        private PageViewModelBase _backPage;
        /// <summary>
        /// The _create command
        /// </summary>
        private DelegateCommand<IssueViewModel> _createCommand;
        /// <summary>
        /// The _current
        /// </summary>
        private PageViewModelBase _current;
        /// <summary>
        /// The _delete back command
        /// </summary>
        private DelegateCommand<IssueViewModel> _deleteBackCommand;
        /// <summary>
        /// The _delete command
        /// </summary>
        private DelegateCommand<IssueViewModel> _deleteCommand;
        /// <summary>
        /// The _edit command
        /// </summary>
        private DelegateCommand<IssueViewModel> _editCommand;

        /// <summary>
        /// The _is history checked
        /// </summary>
        private bool _isHistoryChecked;
        /// <summary>
        /// The _navigate command
        /// </summary>
        private DelegateCommand<PageViewModelBase> _navigateCommand;
        /// <summary>
        /// The _show command
        /// </summary>
        private DelegateCommand<IssueViewModel> _showCommand;

        private DelegateCommand<IssueViewModel> _showOnDashboardCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// <param name="board">The board.</param>
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

        public ICommand ShowOnDashboardCommand
        {
            get { return _showOnDashboardCommand ?? (_showOnDashboardCommand = new DelegateCommand<IssueViewModel>(ShowOnDashboard)); }
        }

        private void ShowOnDashboard(IssueViewModel issue)
        {
            issue.IsCustomRoot = !issue.IsCustomRoot;
            _board.Submit();
        }

        private DelegateCommand<IssueViewModel> _historyBackCommand;

        public ICommand HistoryBackCommand
        {
            get { return _historyBackCommand ?? (_historyBackCommand = new DelegateCommand<IssueViewModel>(HistoryBack)); }
        }

        private void HistoryBack(IssueViewModel issue)
        {
            Show(issue);
            while (_issueHistory.Count > ShowHistory.Count)
            {
                _issueHistory.Pop();
                _navigateHistory.Pop();
            }
        }


        /// <summary>
        /// Gets the show history.
        /// </summary>
        /// <value>The show history.</value>
        public ObservableCollection<IssueViewModel> ShowHistory
        {
            get { return _showHistory; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is history checked.
        /// </summary>
        /// <value><c>true</c> if this instance is history checked; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Gets a value indicating whether this instance is history enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is history enabled; otherwise, <c>false</c>.</value>
        public bool IsHistoryEnabled
        {
            get { return ShowHistory.Count != 0; }
        }

        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        /// <value>The current.</value>
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

        /// <summary>
        /// Gets the dashboard.
        /// </summary>
        /// <value>The dashboard.</value>
        public DashboardViewModel Dashboard { get; private set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public ConfigurationViewModel Configuration { get; private set; }

        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <value>The issues.</value>
        public IssuesViewModel Issues { get; private set; }

        /// <summary>
        /// Gets the create issue.
        /// </summary>
        /// <value>The create issue.</value>
        public CreateIssueViewModel CreateIssue { get; private set; }

        /// <summary>
        /// Gets the edit issue.
        /// </summary>
        /// <value>The edit issue.</value>
        public EditIssueViewModel EditIssue { get; private set; }

        /// <summary>
        /// Gets the show issue.
        /// </summary>
        /// <value>The show issue.</value>
        public ShowIssueViewModel ShowIssue { get; private set; }

        /// <summary>
        /// Gets the show command.
        /// </summary>
        /// <value>The show command.</value>
        public ICommand ShowCommand
        {
            get { return _showCommand ?? (_showCommand = new DelegateCommand<IssueViewModel>(Show)); }
        }

        /// <summary>
        /// Gets the create command.
        /// </summary>
        /// <value>The create command.</value>
        public ICommand CreateCommand
        {
            get { return _createCommand ?? (_createCommand = new DelegateCommand<IssueViewModel>(Create)); }
        }

        /// <summary>
        /// Gets the edit command.
        /// </summary>
        /// <value>The edit command.</value>
        public ICommand EditCommand
        {
            get { return _editCommand ?? (_editCommand = new DelegateCommand<IssueViewModel>(Edit)); }
        }

        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <value>The delete command.</value>
        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new DelegateCommand<IssueViewModel>(Delete)); }
        }

        /// <summary>
        /// Gets the delete back command.
        /// </summary>
        /// <value>The delete back command.</value>
        public ICommand DeleteBackCommand
        {
            get { return _deleteBackCommand ?? (_deleteBackCommand = new DelegateCommand<IssueViewModel>(DeleteBack)); }
        }

        #region INavigatePage Members

        /// <summary>
        /// Gets the navigate command.
        /// </summary>
        /// <value>The navigate command.</value>
        public ICommand NavigateCommand
        {
            get { return _navigateCommand ?? (_navigateCommand = new DelegateCommand<PageViewModelBase>(Navigate)); }
        }

        /// <summary>
        /// Gets the back command.
        /// </summary>
        /// <value>The back command.</value>
        public ICommand BackCommand
        {
            get { return _backCommand ?? (_backCommand = new DelegateCommand(Back)); }
        }

        #endregion

        /// <summary>
        /// Shows the specified issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        public void Show(IssueViewModel issue)
        {
            if (issue == null || (issue.Equals(_actualIssue)))
            {
                return;
            }

            SetShowHistory(issue);

            _actualIssue = issue;
            ShowIssue.SetIssue(issue);
            Navigate(ShowIssue);
        }

        /// <summary>
        /// Sets the show history.
        /// </summary>
        /// <param name="issue">The issue.</param>
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
                foreach (IssueViewModel issueViewModel in removeList)
                {
                    _showHistory.Remove(issueViewModel);
                }
            }
            _showHistory.Add(issue);
            OnPropertyChanged("IsHistoryEnabled");
        }

        /// <summary>
        /// Creates the specified issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        private void Create(IssueViewModel issue)
        {
            CreateIssue.ClearForm();
            if (issue != null)
            {
                CreateIssue.AddParent(issue);
            }
            Navigate(CreateIssue);
        }

        /// <summary>
        /// Edits the specified issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        private void Edit(IssueViewModel issue)
        {
            if (issue == null)
            {
                return;
            }
            _actualIssue = issue;
            EditIssue.SetIssue(issue.Model);
            Navigate(EditIssue);
        }

        /// <summary>
        /// Deletes the specified issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        private void Delete(IssueViewModel issue)
        {
            if (issue == null)
            {
                return;
            }
            Delete(issue, false);
        }

        /// <summary>
        /// Deletes the back.
        /// </summary>
        /// <param name="issue">The issue.</param>
        private void DeleteBack(IssueViewModel issue)
        {
            if (issue == null)
            {
                return;
            }
            Delete(issue, true);
        }

        /// <summary>
        /// Deletes the specified issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        /// <param name="isGoBack">if set to <c>true</c> [is go back].</param>
        private void Delete(IssueViewModel issue, bool isGoBack)
        {
            if (MessageBox.Show((string) Application.Current.FindResource("DeleteMessage"),
                                (string) Application.Current.FindResource("DeleteHeader"),
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

        /// <summary>
        /// Deletes the specified issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
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


        /// <summary>
        /// Navigates the specified page view model.
        /// </summary>
        /// <param name="pageViewModel">The page view model.</param>
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

        /// <summary>
        /// Determines whether [is root page] [the specified page].
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns><c>true</c> if [is root page] [the specified page]; otherwise, <c>false</c>.</returns>
        public bool IsRootPage(PageViewModelBase page)
        {
            return page == Dashboard || page == Issues || page == Configuration;
        }

        /// <summary>
        /// Backs this instance.
        /// </summary>
        private void Back()
        {
            if (_navigateHistory.Count > 1)
            {
                _navigateHistory.Pop();
                _issueHistory.Pop();
                PageViewModelBase lastPage = _navigateHistory.Peek();
                IssueViewModel lastIssue = _issueHistory.Peek();
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

        /// <summary>
        /// Clears the history.
        /// </summary>
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