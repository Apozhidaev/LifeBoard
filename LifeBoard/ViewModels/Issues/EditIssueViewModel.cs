using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class EditIssueViewModel : EditIssueViewModelBase
    {
        private Issue _issue;

        public EditIssueViewModel(IssuesViewModel parent, BoardService boardService)
            : base(parent, boardService)
        {
        }

        public void Submit(Issue issue)
        {
            _issue = issue;
            FromIssue();
            Navigate();
        }

        protected override void Submit()
        {
            ToIssue();
            base.Submit();
        }

        private void FromIssue()
        {
            Type = _issue.Type;
            Priority = _issue.Priority;
            Summary = _issue.Summary;
            Description = _issue.Description;
        }

        private void ToIssue()
        {
            _issue.Type = Type;
            _issue.Priority = Priority;
            _issue.Summary = Summary;
            _issue.Description = Description;
        }
    }
}
