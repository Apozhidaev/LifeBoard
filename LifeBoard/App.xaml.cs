using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using LifeBoard.Models;
using LifeBoard.Models.Configs;
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
        /// <summary>
        /// The _board
        /// </summary>
        private readonly Board _board = new Board();
        /// <summary>
        /// The _mutex
        /// </summary>
        private Mutex _mutex;

        /// <summary>
        /// Sets the foreground window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="nCmdShow">The n CMD show.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ShowWindow(int hwnd, int nCmdShow);

        /// <summary>
        /// Called when [startup].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StartupEventArgs" /> instance containing the event data.</param>
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
                        ShowWindow((int) process.MainWindowHandle, 3);
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                Current.Shutdown();
            }
        }

        /// <summary>
        /// Called when [exit].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExitEventArgs" /> instance containing the event data.</param>
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