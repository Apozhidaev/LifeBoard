using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.ViewModels;
using LifeBoard.ViewModels.Issues;

namespace LifeBoard.Views.Issues
{
    /// <summary>
    /// Логика взаимодействия для IssuesView.xaml
    /// </summary>
    public partial class IssuesView : Page
    {
        private readonly IssuesViewModel _model;

        public IssuesView(IssuesViewModel model)
        {
            _model = model;
            InitializeComponent();
            DataContext = model;
        }

        private void OnDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var issue = _dataGrid.SelectedValue as IssueViewModel;
            if (issue != null)
            {
                ((MainViewModel)_model.Parent).Show(issue);
            }
        }
    }
}