using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class MainIssuesViewModel : PageViewModelBase, IFrameViewModel
    {
        private readonly BoardService _boardService;

        private MainIssuesView _mainIssuesView;

        private PageViewModelBase _current;

        public MainIssuesViewModel(IFrameViewModel parent, BoardService boardService)
            : base(parent)
        {
            _boardService = boardService;
            Issues = new IssuesViewModel(this, boardService);
            CreateIssue = new CreateIssueViewModel(this, Issues.NavigateCommand, boardService);
            EditIssue = new EditIssueViewModel(this, Issues.NavigateCommand, boardService);
            ShowIssue = new ShowIssueViewModel(this, Issues.NavigateCommand, boardService);
            _current = Issues;
        }

        public IssuesViewModel Issues { get; private set; }

        public CreateIssueViewModel CreateIssue { get; private set; }

        public EditIssueViewModel EditIssue { get; private set; }

        public ShowIssueViewModel ShowIssue { get; private set; }

        public override Page Page
        {
            get { return _mainIssuesView ?? (_mainIssuesView = new MainIssuesView(this)); }
        }

        public PageViewModelBase Current
        {
            get { return _current; }
            set
            {
                if (!Equals(_current, value))
                {
                    _current = value;
                    OnPropertyChanged("Current");
                }
            }
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
            EditIssue.Edit(issue.Model);
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

        public void Delete(Issue issue)
        {
            _boardService.DeleteIssue(issue);
            _boardService.Submit();
            Issues.Search();
        }
    }
}
