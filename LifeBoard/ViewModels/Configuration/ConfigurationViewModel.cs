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
    public class ConfigurationViewModel : PageViewModelBase
    {
        private readonly Board _board;

        private ConfigurationView _configurationView;

        public ConfigurationViewModel(object parent, Board board)
            : base(parent)
        {
            _board = board;
            ConfigDisplay = new ConfigDisplayViewModel();
        }

        public ConfigDisplayViewModel ConfigDisplay { get; private set; }

        private DelegateCommand _openDocumentCommand;

        public ICommand OpenDocumentCommand
        {
            get { return _openDocumentCommand ?? (_openDocumentCommand = new DelegateCommand(OpenDocument)); }
        }

        public void OpenDocument()
        {
            var dlg = new OpenFileDialog();
            dlg.DefaultExt = ".life";
            dlg.Filter = "Life Board documents (.life)|*.life";
            if (dlg.ShowDialog() == true)
            {
                if(_board.OpenDocument(dlg.FileName))
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

        public string DocumentPath
        {
            get
            {
                return Path.IsPathRooted(ConfigRepository.Config.DocumentPath)
                           ? ConfigRepository.Config.DocumentPath
                           : "Не указан";
            }
        }

        public override Page Page
        {
            get { return _configurationView ?? (_configurationView = new ConfigurationView(this)); }
        }

        public Language Language
        {
            get { return ConfigRepository.Config.Language; }
            set
            {
                if (ConfigRepository.Config.Language != value)
                {
                    ConfigRepository.Config.Language = value;
                    ConfigRepository.Save();
                    UpdateResources(value);                
                    OnPropertyChanged("Language");
                }
            }
        }

        public IEnumerable<Language> Languages
        {
            get { return Enum.GetValues(typeof(Language)).Cast<Language>(); }
        }

        public static void UpdateResources(Language language)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            string resourse = "Assets/StringResources.xaml";
            if (language == Language.Russian)
            {
                resourse = "Assets/StringResources.ru.xaml";
            }
            Application.Current.Resources.MergedDictionaries.Add(
                (ResourceDictionary)Application.LoadComponent(
                new Uri(resourse, UriKind.Relative)));
            Application.Current.Resources.MergedDictionaries.Add(
                (ResourceDictionary)Application.LoadComponent(
                new Uri("Assets/Styles.xaml", UriKind.Relative)));
        }

       
    }
}