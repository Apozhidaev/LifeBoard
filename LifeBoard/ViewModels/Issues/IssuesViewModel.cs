using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class IssuesViewModel : PageViewModelBase
    {
        private readonly Board _board;

        private IssuesView _issuesView;

        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        public FilterViewModel Filter { get; private set; }

        public IssuesViewModel(object parent, Board board)
            : base(parent)
        {
            _board = board;
            Filter = new FilterViewModel(this);
            Issues = new ObservableCollection<IssueViewModel>();
            Filter.SetModel(board.GetFullFilter(), board.GetDefaultFilter());
            AsyncSearch();
        }

        private DelegateCommand _searchCommand;

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new DelegateCommand(AsyncSearch)); }
        }

        public async void AsyncSearch()
        {
            var issues = await Task<List<Issue>>.Factory.StartNew(() => _board.GetIssues(Filter.ToModel()).ToList());

            var removeList = Issues.Where(i => !issues.Contains(i.Model)).ToList();

            foreach (var issueViewModel in removeList)
            {
                Issues.Remove(issueViewModel);
            }

            foreach (var issue in issues)
            {
                var model = new IssueViewModel(this, issue);
                if (!Issues.Contains(model))
                {
                    Issues.Add(new IssueViewModel(this, issue));
                }
            }
        }

        private void Search()
        {
            var issues = _board.GetIssues(Filter.ToModel());
            Issues.Clear();
            foreach (var issue in issues)
            {
                Issues.Add(new IssueViewModel(this, issue));
            }
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