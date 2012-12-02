using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Models.Configs;
using LifeBoard.Views.Configuration;
using Microsoft.Win32;

namespace LifeBoard.ViewModels.Configuration
{
    /// <summary>
    /// Class ConfigurationViewModel
    /// </summary>
    public class ConfigurationViewModel : PageViewModelBase
    {
        /// <summary>
        /// The _board
        /// </summary>
        private readonly Board _board;

        /// <summary>
        /// The _configuration view
        /// </summary>
        private ConfigurationView _configurationView;
        /// <summary>
        /// The _open document command
        /// </summary>
        private DelegateCommand _openDocumentCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="board">The board.</param>
        public ConfigurationViewModel(object parent, Board board)
            : base(parent)
        {
            _board = board;
            ConfigDisplay = new ConfigDisplayViewModel();
        }

        /// <summary>
        /// Gets the config file.
        /// </summary>
        /// <value>The config file.</value>
        public string ConfigFile
        {
            get { return Global.ConfigFile; }
        }

        /// <summary>
        /// Gets the backup folder.
        /// </summary>
        /// <value>The backup folder.</value>
        public string BackupFolder
        {
            get { return Global.BackupFolder; }
        }

        /// <summary>
        /// Gets the config display.
        /// </summary>
        /// <value>The config display.</value>
        public ConfigDisplayViewModel ConfigDisplay { get; private set; }

        /// <summary>
        /// Gets the open document command.
        /// </summary>
        /// <value>The open document command.</value>
        public ICommand OpenDocumentCommand
        {
            get { return _openDocumentCommand ?? (_openDocumentCommand = new DelegateCommand(OpenDocument)); }
        }

        /// <summary>
        /// Gets the document path.
        /// </summary>
        /// <value>The document path.</value>
        public string DocumentPath
        {
            get
            {
                return Path.IsPathRooted(ConfigRepository.Config.DocumentPath)
                           ? ConfigRepository.Config.DocumentPath
                           : "Не указан";
            }
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <value>The page.</value>
        public override Page Page
        {
            get { return _configurationView ?? (_configurationView = new ConfigurationView(this)); }
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        public Language Language
        {
            get { return ConfigRepository.Config.Language; }
            set
            {
                if (ConfigRepository.Config.Language != value)
                {
                    ConfigRepository.Config.Language = value;
                    ConfigRepository.Save();
                    Global.UpdateResources(value);
                    OnPropertyChanged("Language");
                }
            }
        }

        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <value>The languages.</value>
        public IEnumerable<Language> Languages
        {
            get { return Enum.GetValues(typeof (Language)).Cast<Language>(); }
        }

        /// <summary>
        /// Opens the document.
        /// </summary>
        public void OpenDocument()
        {
            var dlg = new OpenFileDialog();
            dlg.DefaultExt = ".life";
            dlg.Filter = "Life Board documents (.life)|*.life";
            if (dlg.ShowDialog() == true)
            {
                if (_board.OpenDocument(dlg.FileName))
                {
                    ConfigRepository.SetDocumentPath(_board.DocumentPath);
                    OnPropertyChanged("DocumentPath");
                }
                else
                {
                    MessageBox.Show("Нудалось окрыть файл");
                }
            }
        }
    }
}