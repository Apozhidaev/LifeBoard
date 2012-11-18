using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class TypeViewModel : ParentViewModelBase
    {
        public TypeViewModel(object parent, IssueType issueType, bool isChecked)
            : base(parent)
        {
            IssueType = issueType;
            IsChecked = isChecked;
        }

        public string Name 
        {
            get { return IssueType.ToString(); }
        }

        public bool IsChecked { get; set; }

        public IssueType IssueType { get; private set; }
    }
}
