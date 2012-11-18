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

        public CreateIssueViewModel CreateIssue { get; private set; }

        public EditIssueViewModel EditIssue { get; private set; }

        public ShowIssueViewModel ShowIssue { get; private set; }

        public IssuesViewModel(IFrameViewModel parent, BoardService boardService)
            : base(parent)
        {
            _boardService = boardService;
            Filter = new FilterViewModel(this);
            CreateIssue = new CreateIssueViewModel(this, boardService);
            EditIssue = new EditIssueViewModel(this, boardService);
            ShowIssue = new ShowIssueViewModel(this, boardService);
            Issues = new ObservableCollection<IssueViewModel>();
            Filter.SetModel(boardService.GetFullFilter(), boardService.GetDefaultFilter());
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

        private DelegateCommand<IssueViewModel> _showCommand;

        public ICommand ShowCommand
        {
            get { return _showCommand ?? (_showCommand = new DelegateCommand<IssueViewModel>(Show)); }
        }

        public void Show(IssueViewModel issue)
        {
            ShowIssue.Show(issue.Model);
        }

        private DelegateCommand<IssueViewModel> _editCommand;

        public ICommand EditCommand
        {
            get { return _editCommand ?? (_editCommand = new DelegateCommand<IssueViewModel>(Edit)); }
        }

        private void Edit(IssueViewModel issue)
        {
            EditIssue.Submit(issue.Model);
        }

        private DelegateCommand<IssueViewModel> _deleteCommand;

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new DelegateCommand<IssueViewModel>(Delete)); }
        }

        private void Delete(IssueViewModel issue)
        {
            Delete(issue.Model);
        }
    }
}