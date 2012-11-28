using System.Collections.Generic;

namespace LifeBoard.Models
{
    public class Document
    {
        public Dictionary<int, Issue> Issues { get; set; }

        public List<IssueLink> IssuesLinks { get; set; }

        public bool IsEmpty
        {
            get { return Issues == null || Issues.Count == 0; }
        }
    }
}
