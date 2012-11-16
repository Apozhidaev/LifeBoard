using System.Collections.Generic;

namespace LifeBoard.Models
{
    public class Document
    {
        public List<Issue> Issues { get; set; }

        public List<IssueLink> ManyIssues { get; set; }
    }
}
