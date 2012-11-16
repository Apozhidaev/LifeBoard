using System.Windows.Controls;
using LifeBoard.ViewModels.Issues;

namespace LifeBoard.Views.Issues
{
    /// <summary>
    /// Логика взаимодействия для CreateIssueView.xaml
    /// </summary>
    public partial class CreateIssueView : Page
    {
        public CreateIssueView(CreateIssueViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}
