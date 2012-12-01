using System.Windows;
using LifeBoard.Models.Configs;

namespace LifeBoard.ViewModels.Issues
{
    public class ConfigShowIssueViewModel : ViewModelBase
    {
        public ConfigShowIssueViewModel()
        {
            Table = new ConfigIssueViewModel(ConfigRepository.Config.Display.ShowIssue.Table);
            Sitebar = new ConfigIssueViewModel(ConfigRepository.Config.Display.ShowIssue.Sitebar);
        }

        public ConfigIssueViewModel Table { get; set; }

        public ConfigIssueViewModel Sitebar { get; set; }

        public Visibility HistoryVisibility
        {
            get { return ConfigRepository.Config.Display.ShowIssue.IsHistoryAsParents ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility ParentsVisibility
        {
            get { return ConfigRepository.Config.Display.ShowIssue.IsHistoryAsParents ? Visibility.Visible : Visibility.Collapsed; }
        }

        public void Update()
        {
            OnPropertyChanged("Table");
            OnPropertyChanged("Sitebar");
            OnPropertyChanged("HistoryVisibility");
            OnPropertyChanged("ParentsVisibility");
        }
    }
}
