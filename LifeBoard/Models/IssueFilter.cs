using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeBoard.Models
{
    public class IssueFilter
    {
        public IssueFilter()
        {
            Query = String.Empty;
        }

        public String Query { get; set; }

        public HashSet<int> Priorities { get; set; }

        public HashSet<IssueType> Types { get; set; }

        public HashSet<IssueStatus> Statuses { get; set; }

        public bool HasQuery
        {
            get { return !String.IsNullOrEmpty(Query); }
        }
    }
}
