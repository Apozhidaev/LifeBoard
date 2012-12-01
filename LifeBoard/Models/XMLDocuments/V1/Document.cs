using System;
namespace LifeBoard.Models.XMLDocuments.V1
{
    public class Document
    {
        public Document()
        {
            Version = new Version(1, 0);
        }

        public Version Version { get; set; }

        public Issue[] Issues { get; set; }

        public IssueLinks[] IssuesLinks { get; set; }
    }
}
