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
        public DashboardView(DashboardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}