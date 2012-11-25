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
    /// Логика взаимодействия для EditIssueView.xaml
    /// </summary>
    public partial class EditIssueView : Page
    {
        private readonly EditIssueViewModelBase _model;

        public EditIssueView(EditIssueViewModelBase model)
        {
            InitializeComponent();
            DataContext = model;
            _model = model;
        }

        private void OnDescSelectionChanged(object sender, RoutedEventArgs e)
        {
            //_model.SelectionStart = _desc.SelectionStart;
            _model.SelectionLength = _desc.SelectionLength;
        }
    }
}
