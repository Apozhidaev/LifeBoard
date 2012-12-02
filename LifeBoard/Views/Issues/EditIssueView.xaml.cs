using System.Windows.Controls;
using LifeBoard.ViewModels.Issues;

namespace LifeBoard.Views.Issues
{
    /// <summary>
    /// Логика взаимодействия для EditIssueView.xaml
    /// </summary>
    public partial class EditIssueView : Page
    {
        public EditIssueView(EditIssueViewModelBase model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}