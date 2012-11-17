using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace LifeBoard.ViewModels.Issues
{
    public class IssuePriorityViewModel : ViewModelBase
    {
        private readonly IssuesViewModel _service;

        public IssuePriorityViewModel(IssuesViewModel service, int priority)
        {
            _service = service;
            Priority = priority;
            IsChecked = true;
        }

        public string Name
        {
            get { return Priority.ToString(CultureInfo.InvariantCulture); }
        }

        public bool IsChecked { get; set; }

        public int Priority { get; private set; }

        public IssuesViewModel Parent
        {
            get { return _service; }
        }
    }
}
