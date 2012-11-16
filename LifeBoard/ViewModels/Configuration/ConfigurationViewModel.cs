using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.ViewModels.Issues;
using LifeBoard.Views.Configuration;

namespace LifeBoard.ViewModels.Configuration
{
    public class ConfigurationViewModel : PageViewModelBase
    {
        private readonly BoardService _boardService;

        private ConfigurationView _configurationView;

        public ConfigurationViewModel(MainViewModel parent, BoardService boardService)
            : base(parent)
        {
            _boardService = boardService;
        }

        public override Page Page
        {
            get { return _configurationView ?? (_configurationView = new ConfigurationView(this)); }
        }
    }
}