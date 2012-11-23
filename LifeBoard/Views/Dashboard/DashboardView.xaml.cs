using System.Windows.Controls;
using LifeBoard.ViewModels;
using LifeBoard.ViewModels.Dashboard;

namespace LifeBoard.Views.Dashboard
{
    /// <summary>
    /// Логика взаимодействия для DashboardView.xaml
    /// </summary>
    public partial class DashboardView : Page
    {
        private readonly DashboardViewModel _model;

        public DashboardView(DashboardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            _model = viewModel;
        }

        private void OnIssueDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var issue = _list.SelectedValue as IssueViewModel;
            if (issue != null)
            {
                ((MainViewModel)_model.Parent).Show(issue);
            }
        }
    }
}