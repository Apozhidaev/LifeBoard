using System;
namespace LifeBoard.Models.XMLDocuments.V1
{
    public class Issue
    {
        public int Id { get; set; }

        public int Priority { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public IssueType Type { get; set; }

        public IssueStatus Status { get; set; }

        public bool IsCustomRoot { get; set; }

        public DateTime CreationDate { get; set; }

        public string WebSite { get; set; }
    }
}
