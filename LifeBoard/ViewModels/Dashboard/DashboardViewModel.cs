using System.Collections.ObjectModel;
using System.Windows.Controls;
using LifeBoard.Models;
using LifeBoard.Views.Dashboard;

namespace LifeBoard.ViewModels.Dashboard
{
    public class DashboardViewModel : PageViewModelBase
    {
        private readonly BoardService _boardService;

        private DashboardView _dashboardView;

        public DashboardViewModel(IFrameViewModel parent, BoardService boardService) 
            : base(parent)
        {
            _boardService = boardService;
            Issues = new ObservableCollection<IssueViewModel>();
            UpdateIssues();
        }

        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        public override Page Page
        {
            get { return _dashboardView ?? (_dashboardView = new DashboardView(this)); }
        }

        public override void Navigate()
        {
            UpdateIssues();
            base.Navigate();
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
    }
}