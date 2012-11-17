using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class IssueStatusViewModel : ViewModelBase
    {
        private readonly IssuesViewModel _service;

        public IssueStatusViewModel(IssuesViewModel service, IssueStatus status)
        {
            _service = service;
            IssueStatus = status;
            IsChecked = true;
        }

        public string Name
        {
            get { return IssueStatus.ToString(); }
        }

        public bool IsChecked { get; set; }

        public IssueStatus IssueStatus { get; private set; }

        public IssuesViewModel Parent
        {
            get { return _service; }
        }
    }
}
