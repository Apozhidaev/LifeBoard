using System;
using System.Windows;
using LifeBoard.Models;
using LifeBoard.ViewModels;
using LifeBoard.Views;
using Microsoft.Win32;

namespace LifeBoard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly DocumentRepository _repository = new DocumentRepository();

        private void OnStartup(object sender, StartupEventArgs e)
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                _repository.SetFilePath(Environment.GetCommandLineArgs()[1]);
            }
            if (!_repository.IsFileExists)
            {
                var dlg = new OpenFileDialog();
                dlg.DefaultExt = ".life";
                dlg.Filter = "Life Board documents (.life)|*.life";
                if (dlg.ShowDialog() == true)
                {
                    _repository.SetFilePath(dlg.FileName);
                }
            }
            _repository.Open();
            var view = new MainView(new MainViewModel(new BoardService(_repository)));
            view.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            if (!_repository.IsFileExists)
            {
                var dlg = new SaveFileDialog();
                dlg.DefaultExt = ".life";
                dlg.Filter = "Life Board documents (.life)|*.life";
                if (dlg.ShowDialog() != true)
                {
                    return;
                }
                _repository.SetFilePath(dlg.FileName);
            }
            _repository.Save();
        }
    }
}