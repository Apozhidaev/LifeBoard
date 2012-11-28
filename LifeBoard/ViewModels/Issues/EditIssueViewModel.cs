using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class EditIssueViewModel : EditIssueViewModelBase
    {
        private Issue _issue;

        public EditIssueViewModel(INavigatePage parent, Board board)
            : base(parent, board)
        {
            SubmitHeader = (string)Application.Current.FindResource("EditHeader");
        }

        public void SetIssue(Issue issue)
        {
            _issue = issue;
            FromIssue();
            FromAttachments();
            ParentIssues.Clear();
            foreach (var parent in Board.GetParents(issue.Id))
            {
                ParentIssues.Add(new IssueViewModel(this, parent));
            }
        }

        protected override void Submit()
        {
            
            int id = _issue.Id;
            if (Board.UpdateAttachments(id, Attachments.Select(a => a.FileName).ToList(), FilePaths)) 
            {
                ToIssue();
                var parents = ParentIssues.Select(pi => pi.Model.Id);
                Board.SetParents(id, parents);
                Board.Submit();
                base.Submit();
            }
            else
            {
                MessageBox.Show("Неудалось добавить выбранные файла.");
                FromAttachments();
            }
        }

        private void FromIssue()
        {
            Type = _issue.Type;
            Priority = _issue.Priority;
            Summary = _issue.Summary;
            Description = _issue.Description;
            IsCustomRoot = _issue.IsCustomRoot;
            WebSite = _issue.WebSite;
        }

        private void FromAttachments()
        {
            Attachments.Clear();
            FilePaths.Clear();
            foreach (var attachment in Board.GetAttachments(_issue.Id))
            {
                Attachments.Add(new AttachmentViewModel(this) { FileName = attachment });
            }
        }

        private void ToIssue()
        {
            _issue.Type = Type;
            _issue.Priority = Priority;
            _issue.Summary = Summary;
            _issue.Description = Description;
            _issue.IsCustomRoot = IsCustomRoot;
            _issue.WebSite = WebSite;
        }

        protected override IEnumerable<Issue> GetFilterIssues()
        {
            return Board.GetIssuesExeptChildren(_issue.Id, Query);
        }
    }
}
