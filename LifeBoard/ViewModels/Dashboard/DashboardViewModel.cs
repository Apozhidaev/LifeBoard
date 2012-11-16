using System.Windows.Controls;
using LifeBoard.Models;
using LifeBoard.Views.Dashboard;

namespace LifeBoard.ViewModels.Dashboard
{
    public class DashboardViewModel : PageViewModelBase
    {
        private readonly BoardService _model;

        private DashboardView _dashboardView;

        public DashboardViewModel(MainViewModel parent, BoardService model) : base(parent)
        {
            _model = model;
        }

        public override Page Page
        {
            get { return _dashboardView ?? (_dashboardView = new DashboardView(this)); }
        }
    }
}