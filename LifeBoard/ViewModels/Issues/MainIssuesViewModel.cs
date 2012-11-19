using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class MainIssuesViewModel : PageViewModelBase, INavigatePage
    {
        private readonly BoardService _boardService;

        private MainIssuesView _mainIssuesView;

        private PageViewModelBase _current;

        private Issue _actualIssue;

        private readonly Stack<Issue> _issueHistory = new Stack<Issue>();

        private readonly Stack<PageViewModelBase> _navigateHistory = new Stack<PageViewModelBase>();

        public MainIssuesViewModel(object parent, BoardService boardService)
            : base(parent)
        {
            _boardService = boardService;
            Issues = new IssuesViewModel(this, boardService);
            CreateIssue = new CreateIssueViewModel(this, boardService);
            EditIssue = new EditIssueViewModel(this, boardService);
            ShowIssue = new ShowIssueViewModel(this, boardService);
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
                    _current.IsNavigated = false;
                    _current = value;
                    _current.IsNavigated = true;
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
            _actualIssue = issue.Model;
            ShowIssue.SetIssue(issue.Model);
            Navigate(ShowIssue);
        }

        private DelegateCommand<IssueViewModel> _editCommand;

        public ICommand EditCommand
        {
            get { return _editCommand ?? (_editCommand = new DelegateCommand<IssueViewModel>(Edit)); }
        }

        private void Edit(IssueViewModel issue)
        {
            _actualIssue = issue.Model;
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
            Delete(issue.Model);
        }

        public void Delete(Issue issue)
        {
            _boardService.DeleteIssue(issue);
            _boardService.Submit();
            Issues.Search();
        }

        private DelegateCommand<PageViewModelBase> _navigateCommand;

        public ICommand NavigateCommand
        {
            get { return _navigateCommand ?? (_navigateCommand = new DelegateCommand<PageViewModelBase>(Navigate)); }
        }

        private void Navigate(PageViewModelBase pageViewModel)
        {
            if (Equals(pageViewModel, Issues))
            {
                _navigateHistory.Clear();
                _issueHistory.Clear();
                _actualIssue = null;
            }
            else
            {
                _navigateHistory.Push(pageViewModel);
                _issueHistory.Push(_actualIssue);
            }
            Current = pageViewModel;
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
                    EditIssue.SetIssue(lastIssue);
                }
                if (lastPage is ShowIssueViewModel)
                {
                    ShowIssue.SetIssue(lastIssue);
                }
                Current = lastPage;
            }
            else
            {
                Navigate(Issues);
            }
        }
    }
}
