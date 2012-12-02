using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Dashboard;

namespace LifeBoard.ViewModels.Dashboard
{
    /// <summary>
    /// Class DashboardViewModel
    /// </summary>
    public class DashboardViewModel : PageViewModelBase
    {
        /// <summary>
        /// The _board
        /// </summary>
        private readonly Board _board;
        /// <summary>
        /// The _custom rroot command
        /// </summary>
        private DelegateCommand _customRrootCommand;

        /// <summary>
        /// The _dashboard view
        /// </summary>
        private DashboardView _dashboardView;
        /// <summary>
        /// The _is custom root
        /// </summary>
        private bool _isCustomRoot;
        /// <summary>
        /// The _is root
        /// </summary>
        private bool _isRoot;
        /// <summary>
        /// The _root command
        /// </summary>
        private DelegateCommand _rootCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="board">The board.</param>
        public DashboardViewModel(object parent, Board board)
            : base(parent)
        {
            _board = board;
            Issues = new ObservableCollection<IssueViewModel>();
            _isCustomRoot = true;
        }

        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <value>The issues.</value>
        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <value>The page.</value>
        public override Page Page
        {
            get { return _dashboardView ?? (_dashboardView = new DashboardView(this)); }
        }

        /// <summary>
        /// Gets the custom root command.
        /// </summary>
        /// <value>The custom root command.</value>
        public ICommand CustomRootCommand
        {
            get { return _customRrootCommand ?? (_customRrootCommand = new DelegateCommand(CustomRoot)); }
        }

        /// <summary>
        /// Gets the root command.
        /// </summary>
        /// <value>The root command.</value>
        public ICommand RootCommand
        {
            get { return _rootCommand ?? (_rootCommand = new DelegateCommand(Root)); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is root.
        /// </summary>
        /// <value><c>true</c> if this instance is root; otherwise, <c>false</c>.</value>
        public bool IsRoot
        {
            get { return _isRoot; }
            set
            {
                if (_isRoot != value)
                {
                    _isRoot = value;
                    OnPropertyChanged("IsRoot");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is custom root.
        /// </summary>
        /// <value><c>true</c> if this instance is custom root; otherwise, <c>false</c>.</value>
        public bool IsCustomRoot
        {
            get { return _isCustomRoot; }
            set
            {
                if (_isCustomRoot != value)
                {
                    _isCustomRoot = value;
                    OnPropertyChanged("IsCustomRoot");
                }
            }
        }

        /// <summary>
        /// Called when [navigated].
        /// </summary>
        protected override void OnNavigated()
        {
            base.OnNavigated();
            AsyncUpdateIssues();
        }

        /// <summary>
        /// Asyncs the update issues.
        /// </summary>
        private async void AsyncUpdateIssues()
        {
            IEnumerable<Issue> issues = IsCustomRoot
                                            ? await
                                              Task<IEnumerable<Issue>>.Factory.StartNew(_board.GetCustomRootIssues)
                                            : await Task<IEnumerable<Issue>>.Factory.StartNew(_board.GetRootIssues);
            Issues.Clear();
            foreach (Issue issue in issues)
            {
                Issues.Add(new IssueViewModel(this, issue));
            }
        }

        /// <summary>
        /// Updates the issues.
        /// </summary>
        private void UpdateIssues()
        {
            IEnumerable<Issue> issues = IsCustomRoot ? _board.GetCustomRootIssues() : _board.GetRootIssues();
            Issues.Clear();
            foreach (Issue issue in issues)
            {
                Issues.Add(new IssueViewModel(this, issue));
            }
        }

        /// <summary>
        /// Customs the root.
        /// </summary>
        private void CustomRoot()
        {
            GoRoot(true);
        }

        /// <summary>
        /// Roots this instance.
        /// </summary>
        private void Root()
        {
            GoRoot(false);
        }

        /// <summary>
        /// Goes the root.
        /// </summary>
        /// <param name="isCustom">if set to <c>true</c> [is custom].</param>
        private void GoRoot(bool isCustom)
        {
            IsRoot = !isCustom;
            IsCustomRoot = isCustom;
            AsyncUpdateIssues();
        }
    }
}