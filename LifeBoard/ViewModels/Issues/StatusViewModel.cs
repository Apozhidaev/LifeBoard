using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class StatusViewModel : ParentViewModelBase
    {
        public StatusViewModel(object parent, IssueStatus status, bool isChecked) 
            : base(parent)
        {
            IssueStatus = status;
            IsChecked = isChecked;
        }

        public string Name
        {
            get { return IssueStatus.ToString(); }
        }

        public bool IsChecked { get; set; }

        public IssueStatus IssueStatus { get; private set; }
    }
}
