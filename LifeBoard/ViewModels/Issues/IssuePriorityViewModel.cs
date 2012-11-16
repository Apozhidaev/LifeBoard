using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeBoard.ViewModels.Issues
{
    public class IssuePriorityViewModel : ViewModelBase
    {
        public IssuePriorityViewModel(int priority)
        {
            Priority = priority;
            IsChecked = true;
        }

        public string Name
        {
            get { return Priority.ToString(); }
        }

        public bool IsChecked { get; set; }

        public int Priority { get; private set; }
    }
}
