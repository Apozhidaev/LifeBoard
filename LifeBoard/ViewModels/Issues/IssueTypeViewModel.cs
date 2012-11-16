using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class IssueTypeViewModel : ViewModelBase
    {
        public IssueTypeViewModel(IssueType issueType)
        {
            IssueType = issueType;
            IsChecked = true;
        }

        public string Name 
        {
            get { return IssueType.ToString(); }
        }

        public bool IsChecked { get; set; }

        public IssueType IssueType { get; private set; }
    }
}
