using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeBoard.Models
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

        public bool IsCustomRoot { get; set; }

        public DateTime CreationDate { get; set; }

        public string WebLink { get; set; }

        public IssueStatus NextStatus
        { 
            get
            {
                if (Status == IssueStatus.Closed)
                {
                    return IssueStatus.Open;
                }
                var status = (int)Status;
                ++status;
                return (IssueStatus)status;
            }
        }

        public void NextState()
        {
            Status = NextStatus;
        }
    }
}
