using System.Globalization;

namespace LifeBoard.ViewModels.Issues
{
    public class PriorityViewModel : ParentViewModelBase
    {

        public PriorityViewModel(object parent, int priority, bool isChecked)
            : base(parent)
        {
            Priority = priority;
            IsChecked = isChecked;
        }

        public string Name
        {
            get { return Priority.ToString(CultureInfo.InvariantCulture); }
        }

        public bool IsChecked { get; set; }

        public int Priority { get; private set; }
    }
}
