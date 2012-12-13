namespace LifeBoard.Models.Configs
{
    /// <summary>
    /// Class Display
    /// </summary>
    public class Display
    {
        public Display()
        {
            ShowIssue = new ShowIssue();
            Dashboard = new Dashboard();
        }

        /// <summary>
        /// Gets or sets the show issue.
        /// </summary>
        /// <value>The show issue.</value>
        public ShowIssue ShowIssue { get; set; }

        public Dashboard Dashboard { get; set; }
    }
}