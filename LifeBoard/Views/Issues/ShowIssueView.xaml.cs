using System.Windows.Controls;
using LifeBoard.ViewModels.Issues;

namespace LifeBoard.Views.Issues
{
    /// <summary>
    /// Логика взаимодействия для ShowIssueView.xaml
    /// </summary>
    public partial class ShowIssueView : Page
    {
        public ShowIssueView(ShowIssueViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}