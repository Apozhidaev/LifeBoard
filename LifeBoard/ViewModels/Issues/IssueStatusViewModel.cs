using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class IssueStatusViewModel : ViewModelBase
    {
        public IssueStatusViewModel(IssueStatus status)
        {
            IssueStatus = status;
            IsChecked = true;
        }

        public string Name
        {
            get { return IssueStatus.ToString(); }
        }

        public bool IsChecked { get; set; }

        public IssueStatus IssueStatus { get; private set; }
    }
}
