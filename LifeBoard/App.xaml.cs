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
        private readonly BoardService _boardService = new BoardService();

        private void OnStartup(object sender, StartupEventArgs e)
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                _boardService.SetFilePath(Environment.GetCommandLineArgs()[1]);
            }
            if (!_boardService.IsFileExists)
            {
                var dlg = new OpenFileDialog();
                dlg.DefaultExt = ".life";
                dlg.Filter = "Life Board documents (.life)|*.life";
                if (dlg.ShowDialog() == true)
                {
                    _boardService.SetFilePath(dlg.FileName);
                }
            }
            _boardService.Open();
            var view = new MainView(new MainViewModel(_boardService));
            view.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            if(!_boardService.IsFileExists)
            {
                var dlg = new SaveFileDialog();
                dlg.DefaultExt = ".life";
                dlg.Filter = "Life Board documents (.life)|*.life";
                if (dlg.ShowDialog() != true)
                {
                    return;
                }
                _boardService.SetFilePath(dlg.FileName);
            }
            _boardService.Save();
        }
    }
}