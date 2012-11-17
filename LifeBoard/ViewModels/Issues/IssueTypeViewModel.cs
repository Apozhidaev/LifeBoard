using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class IssueTypeViewModel : ViewModelBase
    {
        private readonly IssuesViewModel _service;

        public IssueTypeViewModel(IssuesViewModel service, IssueType issueType)
        {
            _service = service;
            IssueType = issueType;
            IsChecked = true;
        }

        public string Name 
        {
            get { return IssueType.ToString(); }
        }

        public bool IsChecked { get; set; }

        public IssueType IssueType { get; private set; }

        public IssuesViewModel Parent
        {
            get { return _service; }
        }
    }
}
