using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class IssuesViewModel : PageViewModelBase
    {
        private readonly BoardService _boardService;

        private IssuesView _issuesView;

        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        public IssueFilterViewModel IssueFilter { get; private set; }

        public CreateIssueViewModel CreateIssue { get; private set; }

        public EditIssueViewModel EditIssue { get; private set; }

        public ShowIssueViewModel ShowIssue { get; private set; }

        public IssuesViewModel(IFramePageViewModel parent, BoardService boardService)
            : base(parent)
        {
            _boardService = boardService;
            IssueFilter = new IssueFilterViewModel(this, boardService);
            CreateIssue = new CreateIssueViewModel(this, boardService);
            EditIssue = new EditIssueViewModel(this, boardService);
            ShowIssue = new ShowIssueViewModel(this, boardService);
            Issues = new ObservableCollection<IssueViewModel>();
            Search();
        }

        public void Delete(Issue issue)
        {
            _boardService.DeleteIssue(issue);
            Search();
        }

        private DelegateCommand _searchCommand;

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new DelegateCommand(Search)); }
        }

        private void Search()
        {
            var issues = _boardService.GetIssues(IssueFilter.ToModel());
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

        public override void Navigate()
        {
            base.Navigate();
            Search();
        }
    }
}