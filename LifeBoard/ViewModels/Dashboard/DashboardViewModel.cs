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

        public DashboardViewModel(object parent, BoardService boardService) 
            : base(parent)
        {
            _boardService = boardService;
            Issues = new ObservableCollection<IssueViewModel>();
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
            var issues = _boardService.GetRootIssues();
            Issues.Clear();
            foreach (var issue in issues)
            {
                Issues.Add(new IssueViewModel(this, issue));
            }
        }
    }
}