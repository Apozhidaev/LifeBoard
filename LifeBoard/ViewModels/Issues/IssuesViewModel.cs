using System.Collections.ObjectModel;
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

        public FilterViewModel Filter { get; private set; }

        public IssuesViewModel(IFrameViewModel parent, BoardService boardService)
            : base(parent)
        {
            _boardService = boardService;
            Filter = new FilterViewModel(this);
            Issues = new ObservableCollection<IssueViewModel>();
            Filter.SetModel(boardService.GetFullFilter(), boardService.GetDefaultFilter());
            Search();
        }

        private DelegateCommand _searchCommand;

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new DelegateCommand(Search)); }
        }

        public void Search()
        {
            var issues = _boardService.GetIssues(Filter.ToModel());
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