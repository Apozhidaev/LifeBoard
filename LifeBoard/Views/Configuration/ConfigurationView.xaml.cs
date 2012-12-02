using System.Windows.Controls;
using LifeBoard.ViewModels.Configuration;

namespace LifeBoard.Views.Configuration
{
    /// <summary>
    /// Логика взаимодействия для ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : Page
    {
        public ConfigurationView(ConfigurationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}