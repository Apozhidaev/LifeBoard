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
        public ShowIssueView(ShowIssueViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}
