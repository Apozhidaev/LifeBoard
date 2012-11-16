namespace LifeBoard.Models.XMLDocuments.V1
{
    public class Issue
    {
        public Issue()
        {
            Type = IssueType.Task;
            Status = IssueStatus.Open;
        }

        public int Id { get; set; }

        public int Priority { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public IssueType Type { get; set; }

        public IssueStatus Status { get; set; }
    }
}
