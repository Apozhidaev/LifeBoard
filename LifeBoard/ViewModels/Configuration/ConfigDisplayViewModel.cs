namespace LifeBoard.ViewModels.Configuration
{
    /// <summary>
    /// Class ConfigDisplayViewModel
    /// </summary>
    public class ConfigDisplayViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigDisplayViewModel" /> class.
        /// </summary>
        public ConfigDisplayViewModel()
        {
            ShowIssue = new ConfigShowIssueViewModel();
            Dashboard = new ConfigDashboardViewModel();
        }

        /// <summary>
        /// Gets or sets the show issue.
        /// </summary>
        /// <value>The show issue.</value>
        public ConfigShowIssueViewModel ShowIssue { get; set; }

        public ConfigDashboardViewModel Dashboard { get; set; }
    }
}