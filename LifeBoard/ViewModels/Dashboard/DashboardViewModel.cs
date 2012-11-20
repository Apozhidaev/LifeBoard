using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Dashboard;

namespace LifeBoard.ViewModels.Dashboard
{
    public class DashboardViewModel : PageViewModelBase
    {
        private readonly BoardService _boardService;

        private DashboardView _dashboardView;

        private readonly MainViewModel _parent;

        public DashboardViewModel(MainViewModel parent, BoardService boardService) 
            : base(parent)
        {
            _parent = parent;
            _boardService = boardService;
            Issues = new ObservableCollection<IssueViewModel>();
            UpdateIssues();
        }

        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        public override Page Page
        {
            get { return _dashboardView ?? (_dashboardView = new DashboardView(this)); }
        }

        protected override void OnNavigated()
        {
            UpdateIssues();
            base.OnNavigated();
        }

        private void UpdateIssues()
        {
            var issues = _boardService.GetIssues(_boardService.GetInProgressFilter());
            Issues.Clear();
            foreach (var issue in issues)
            {
                Issues.Add(new IssueViewModel(this, issue));
            }
        }

        private DelegateCommand<IssueViewModel> _showCommand;

        public ICommand ShowCommand
        {
            get { return _showCommand ?? (_showCommand = new DelegateCommand<IssueViewModel>(Show)); }
        }

        public void Show(IssueViewModel issue)
        {
            _parent.IssuesPage.Issues.Filter.SetModel(_boardService.GetFullFilter(), _boardService.GetInProgressFilter());
            _parent.Navigate(_parent.IssuesPage);
            _parent.IssuesPage.ClearHistory();
            _parent.IssuesPage.Show(issue);
        }
    }
}