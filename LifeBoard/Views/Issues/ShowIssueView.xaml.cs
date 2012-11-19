using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LifeBoard.ViewModels;
using LifeBoard.ViewModels.Issues;

namespace LifeBoard.Views.Issues
{
    /// <summary>
    /// Логика взаимодействия для ShowIssueView.xaml
    /// </summary>
    public partial class ShowIssueView : Page
    {
        private readonly ShowIssueViewModel _model;

        public ShowIssueView(ShowIssueViewModel model)
        {
            InitializeComponent();
            DataContext = model;
            _model = model;
        }

        private void OnDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var issue = _dataGrid.SelectedValue as IssueViewModel;
            if (issue != null)
            {
                ((MainIssuesViewModel)_model.Parent).Show(issue);
            }
        }
    }
}
