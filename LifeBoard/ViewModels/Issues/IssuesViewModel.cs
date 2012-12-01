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
    public class IssuesViewModel : PageViewModelBase
    {
        private const int PageCount = 20;

        private readonly Board _board;

        private IssuesView _issuesView;

        public ObservableCollection<PageNumberViewModel> PageNumbers { get; private set; }

        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        public ObservableCollection<IssueViewModel> AllIssues { get; private set; }

        public FilterViewModel Filter { get; private set; }

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

        public Visibility PagenatorVisibility
        {
            get { return PageNumbers.Count == 0 ? Visibility.Collapsed : Visibility.Visible; }
        }

        private DelegateCommand<PageNumberViewModel> _goPageCommand;

        public ICommand GoPageCommand
        {
            get { return _goPageCommand ?? (_goPageCommand = new DelegateCommand<PageNumberViewModel>(GoPage)); }
        }

        public void GoPage(PageNumberViewModel numberModel)
        {
            Issues.Clear();
            int start = PageCount * (numberModel.Number - 1);
            int count = PageCount * numberModel.Number;
            for (int i = start; i < count && i < AllIssues.Count; i++)
            {
                Issues.Add(AllIssues[i]);
            }
            PageNumberCorrent = numberModel;
        }

        private DelegateCommand _searchCommand;
        private PageNumberViewModel _pageNumberCorrent;

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new DelegateCommand(AsyncSearch)); }
        }

        public async void AsyncSearch()
        {
            var issues = await Task<List<Issue>>.Factory.StartNew(() => _board.GetIssues(Filter.ToModel()).ToList());

            AllIssues.Clear();
            foreach (var issue in issues)
            {
                AllIssues.Add(new IssueViewModel(this, issue));
            }

            var pageIssues = AllIssues.Take(PageCount).ToList();

            var removeList = Issues.Where(i => !pageIssues.Contains(i)).ToList();

            foreach (var issueViewModel in removeList)
            {
                Issues.Remove(issueViewModel);
            }

            foreach (var issue in pageIssues)
            {
                if (!Issues.Contains(issue))
                {
                    Issues.Add(issue);
                }
            }

            PageNumbers.Clear();
            for (int i = 0; i < (AllIssues.Count - 1) / PageCount; i++)
            {
                PageNumbers.Add(new PageNumberViewModel(this, i + 1));
            }
            if (PageNumbers.Count > 0)
            {
                PageNumberCorrent = new PageNumberViewModel(this, 1);
            }
            OnPropertyChanged("PagenatorVisibility");
        }

        public override Page Page
        {
            get { return _issuesView ?? (_issuesView = new IssuesView(this)); }
        }

        protected override void OnNavigated()
        {
            base.OnNavigated();
            AsyncSearch();
        }
    }
}