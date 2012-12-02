using System.Windows;
using LifeBoard.Models.Configs;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class ConfigShowIssueViewModel
    /// </summary>
    public class ConfigShowIssueViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigShowIssueViewModel" /> class.
        /// </summary>
        public ConfigShowIssueViewModel()
        {
            Table = new ConfigIssueViewModel(ConfigRepository.Config.Display.ShowIssue.Table);
            Sitebar = new ConfigIssueViewModel(ConfigRepository.Config.Display.ShowIssue.Sitebar);
        }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>The table.</value>
        public ConfigIssueViewModel Table { get; set; }

        /// <summary>
        /// Gets or sets the sitebar.
        /// </summary>
        /// <value>The sitebar.</value>
        public ConfigIssueViewModel Sitebar { get; set; }

        /// <summary>
        /// Gets the history visibility.
        /// </summary>
        /// <value>The history visibility.</value>
        public Visibility HistoryVisibility
        {
            get
            {
                return ConfigRepository.Config.Display.ShowIssue.IsHistoryAsParents
                           ? Visibility.Collapsed
                           : Visibility.Visible;
            }
        }

        /// <summary>
        /// Gets the parents visibility.
        /// </summary>
        /// <value>The parents visibility.</value>
        public Visibility ParentsVisibility
        {
            get
            {
                return ConfigRepository.Config.Display.ShowIssue.IsHistoryAsParents
                           ? Visibility.Visible
                           : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            OnPropertyChanged("Table");
            OnPropertyChanged("Sitebar");
            OnPropertyChanged("HistoryVisibility");
            OnPropertyChanged("ParentsVisibility");
        }
    }
}