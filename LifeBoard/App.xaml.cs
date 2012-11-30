using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using LifeBoard.Models;
using LifeBoard.Models.Configs;
using LifeBoard.ViewModels;
using LifeBoard.ViewModels.Configuration;
using LifeBoard.Views;
using Microsoft.Win32;

namespace LifeBoard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int ShowWindow(int hwnd, int nCmdShow);

        private Mutex _mutex;

        private readonly Board _board = new Board();

        private void OnStartup(object sender, StartupEventArgs e)
        {
            bool createdNew;
            _mutex = new Mutex(true, "LifeBoard", out createdNew);

            if (createdNew)
            {
                ConfigRepository.Open();

                string documentPath = ConfigRepository.Config.DocumentPath;

                if (Environment.GetCommandLineArgs().Length > 1)
                {
                    documentPath = Environment.GetCommandLineArgs()[1];
                }

                _board.OpenDocument(documentPath);

                ConfigRepository.SetDocumentPath(_board.DocumentPath);

                Global.UpdateResources(ConfigRepository.Config.Language);

                var viewModel = new MainViewModel(_board);
                var view = new MainView(viewModel);
                view.Show();
            }
            else
            {
                Process current = Process.GetCurrentProcess();

                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        ShowWindow((int)process.MainWindowHandle, 3);
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                Current.Shutdown();
            }
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            if (!_board.IsFileExists && !_board.Document.IsEmpty)
            {
                var dlg = new SaveFileDialog();
                dlg.DefaultExt = ".life";
                dlg.Filter = "Life Board documents (.life)|*.life";
                if (dlg.ShowDialog() != true)
                {
                    return;
                }
                if (_board.SaveDocument(dlg.FileName))
                {
                    ConfigRepository.SetDocumentPath(_board.DocumentPath);
                }
            }
            _mutex.Dispose();
        }
    }
}