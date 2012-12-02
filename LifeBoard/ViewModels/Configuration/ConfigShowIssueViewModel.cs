using LifeBoard.Models.Configs;

namespace LifeBoard.ViewModels.Configuration
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
        /// Gets or sets a value indicating whether this instance is history as parents.
        /// </summary>
        /// <value><c>true</c> if this instance is history as parents; otherwise, <c>false</c>.</value>
        public bool IsHistoryAsParents
        {
            get { return ConfigRepository.Config.Display.ShowIssue.IsHistoryAsParents; }
            set
            {
                if (ConfigRepository.Config.Display.ShowIssue.IsHistoryAsParents != value)
                {
                    ConfigRepository.Config.Display.ShowIssue.IsHistoryAsParents = value;
                    ConfigRepository.Save();
                    OnPropertyChanged("IsHistoryAsParents");
                }
            }
        }
    }
}