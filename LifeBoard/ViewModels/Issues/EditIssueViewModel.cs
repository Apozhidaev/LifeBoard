using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class EditIssueViewModel
    /// </summary>
    public class EditIssueViewModel : EditIssueViewModelBase
    {
        /// <summary>
        /// The _issue
        /// </summary>
        private Issue _issue;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditIssueViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="board">The board.</param>
        public EditIssueViewModel(INavigatePage parent, Board board)
            : base(parent, board)
        {
            SubmitHeader = (string) Application.Current.FindResource("EditHeader");
        }

        /// <summary>
        /// Sets the issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        public void SetIssue(Issue issue)
        {
            _issue = issue;
            FromIssue();
            FromAttachments();
            ParentIssues.Clear();
            foreach (Issue parent in Board.GetParents(issue.Id))
            {
                ParentIssues.Add(new IssueViewModel(this, parent));
            }
        }

        /// <summary>
        /// Submits this instance.
        /// </summary>
        protected override void Submit()
        {
            int id = _issue.Id;
            if (Board.UpdateAttachments(id, Attachments.Select(a => a.FileName).ToList(), FilePaths))
            {
                ToIssue();
                IEnumerable<int> parents = ParentIssues.Select(pi => pi.Model.Id);
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

        /// <summary>
        /// Froms the issue.
        /// </summary>
        private void FromIssue()
        {
            Type = _issue.Type;
            Priority = _issue.Priority;
            Summary = _issue.Summary;
            Description = _issue.Description;
            IsCustomRoot = _issue.IsCustomRoot;
            Links.Clear();
            foreach (var link in _issue.Links)
            {
                Links.Add(new LinkViewModel(this) {LinkName = link});
            }
        }

        /// <summary>
        /// Froms the attachments.
        /// </summary>
        private void FromAttachments()
        {
            Attachments.Clear();
            FilePaths.Clear();
            foreach (string attachment in Board.GetAttachments(_issue.Id))
            {
                Attachments.Add(new AttachmentViewModel(this) {FileName = attachment});
            }
        }

        /// <summary>
        /// To the issue.
        /// </summary>
        private void ToIssue()
        {
            _issue.Type = Type;
            _issue.Priority = Priority;
            _issue.Summary = Summary;
            _issue.Description = Description;
            _issue.IsCustomRoot = IsCustomRoot;
            _issue.Links.Clear();
            foreach (var link in Links)
            {
                _issue.Links.Add(link.LinkName);
            }
        }

        /// <summary>
        /// Gets the filter issues.
        /// </summary>
        /// <returns>IEnumerable{Issue}.</returns>
        protected override IEnumerable<Issue> GetFilterIssues()
        {
            return Board.GetIssuesExeptChildren(_issue.Id, Query);
        }
    }
}