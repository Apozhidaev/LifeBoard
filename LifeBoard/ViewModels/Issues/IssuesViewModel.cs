using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class IssuesViewModel
    /// </summary>
    public class IssuesViewModel : PageViewModelBase
    {
        /// <summary>
        /// The page count
        /// </summary>
        private const int PageCount = 20;

        /// <summary>
        /// The _board
        /// </summary>
        private readonly Board _board;

        /// <summary>
        /// The _issues view
        /// </summary>
        private IssuesView _issuesView;
        /// <summary>
        /// The _page number corrent
        /// </summary>
        private PageNumberViewModel _pageNumberCorrent;
        /// <summary>
        /// The _search command
        /// </summary>
        private DelegateCommand _searchCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="IssuesViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="board">The board.</param>
        public IssuesViewModel(object parent, Board board)
            : base(parent)
        {
            _board = board;
            Filter = new FilterViewModel(this);
            PageNumbers = new ObservableCollection<PageNumberViewModel>();
            Issues = new ObservableCollection<IssueViewModel>();
            AllIssues = new ObservableCollection<IssueViewModel>();
            Filter.SetModel(board.GetFullFilter(), board.GetDefaultFilter());
            AsyncSearch();
        }

        /// <summary>
        /// Gets the page numbers.
        /// </summary>
        /// <value>The page numbers.</value>
        public ObservableCollection<PageNumberViewModel> PageNumbers { get; private set; }

        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <value>The issues.</value>
        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        /// <summary>
        /// Gets all issues.
        /// </summary>
        /// <value>All issues.</value>
        public ObservableCollection<IssueViewModel> AllIssues { get; private set; }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public FilterViewModel Filter { get; private set; }

        /// <summary>
        /// Gets or sets the page number corrent.
        /// </summary>
        /// <value>The page number corrent.</value>
        public PageNumberViewModel PageNumberCorrent
        {
            get { return _pageNumberCorrent; }
            set
            {
                if (!Equals(_pageNumberCorrent, value))
                {
                    _pageNumberCorrent = value;
                    if (_pageNumberCorrent != null)
                    {
                        GoPage(_pageNumberCorrent);
                    }
                    OnPropertyChanged("PageNumberCorrent");
                }
            }
        }

        /// <summary>
        /// Gets the pagenator visibility.
        /// </summary>
        /// <value>The pagenator visibility.</value>
        public Visibility PagenatorVisibility
        {
            get { return PageNumbers.Count == 0 ? Visibility.Collapsed : Visibility.Visible; }
        }

        /// <summary>
        /// Gets the search command.
        /// </summary>
        /// <value>The search command.</value>
        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new DelegateCommand(AsyncSearch)); }
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <value>The page.</value>
        public override Page Page
        {
            get { return _issuesView ?? (_issuesView = new IssuesView(this)); }
        }

        /// <summary>
        /// Goes the page.
        /// </summary>
        /// <param name="numberModel">The number model.</param>
        public void GoPage(PageNumberViewModel numberModel)
        {
            Issues.Clear();
            int start = PageCount*(numberModel.Number - 1);
            int count = PageCount*numberModel.Number;
            for (int i = start; i < count && i < AllIssues.Count; i++)
            {
                Issues.Add(AllIssues[i]);
            }
        }

        /// <summary>
        /// Asyncs the search.
        /// </summary>
        public async void AsyncSearch()
        {
            List<Issue> issues =
                await Task<List<Issue>>.Factory.StartNew(() => _board.GetIssues(Filter.ToModel()).ToList());

            List<IssueViewModel> removeAllList = AllIssues.Where(i => !issues.Contains(i.Model)).ToList();

            bool hasChanged = false;

            foreach (IssueViewModel issueViewModel in removeAllList)
            {
                AllIssues.Remove(issueViewModel);
                hasChanged = true;
            }

            foreach (Issue issue in issues)
            {
                var model = new IssueViewModel(this, issue);
                if (!AllIssues.Contains(model))
                {
                    AllIssues.Add(model);
                    hasChanged = true;
                }
            }

            SortIssues(AllIssues);

            int skip = PageNumberCorrent != null ? (PageNumberCorrent.Number - 1)*PageCount : 0;

            List<IssueViewModel> pageIssues = AllIssues.Skip(skip).Take(PageCount).ToList();

            List<IssueViewModel> removeList = Issues.Where(i => !pageIssues.Contains(i)).ToList();

            foreach (IssueViewModel issueViewModel in removeList)
            {
                Issues.Remove(issueViewModel);
            }

            foreach (IssueViewModel issue in pageIssues)
            {
                if (!Issues.Contains(issue))
                {
                    Issues.Add(issue);
                }
            }

            if (hasChanged)
            {
                SortIssues(Issues);
                PageNumbers.Clear();
                if (AllIssues.Count > PageCount)
                {
                    for (int i = 0; i <= (AllIssues.Count - 1)/PageCount; i++)
                    {
                        PageNumbers.Add(new PageNumberViewModel(this, i + 1));
                    }
                    PageNumberCorrent = new PageNumberViewModel(this, 1);
                }
                OnPropertyChanged("PagenatorVisibility");
            }
            else
            {
                foreach (IssueViewModel issueViewModel in Issues)
                {
                    issueViewModel.UpdateSource();
                }
            }
        }

        /// <summary>
        /// Called when [navigated].
        /// </summary>
        protected override void OnNavigated()
        {
            base.OnNavigated();
            AsyncSearch();
        }

        /// <summary>
        /// Sorts the issues.
        /// </summary>
        /// <param name="issues">The issues.</param>
        private void SortIssues(ObservableCollection<IssueViewModel> issues)
        {
            List<IssueViewModel> list = issues.OrderBy(i => i.Model.Id).ToList();
            for (int i = 0; i < issues.Count; i++)
            {
                issues[i] = list[i];
            }
        }
    }
}