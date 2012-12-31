using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class CreateIssueViewModel
    /// </summary>
    public class CreateIssueViewModel : EditIssueViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIssueViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="board">The board.</param>
        public CreateIssueViewModel(INavigatePage parent, Board board)
            : base(parent, board)
        {
            SubmitHeader = (string) Application.Current.FindResource("CreateHeader");
        }

        /// <summary>
        /// Called when [navigated].
        /// </summary>
        protected override void OnNavigated()
        {
            ClearForm();
            base.OnNavigated();
        }

        /// <summary>
        /// Submits this instance.
        /// </summary>
        protected override void Submit()
        {
            int id = Board.CreateIssue(Type, Priority, Summary, Description, IsCustomRoot, Deadline,Links.Select(l => l.LinkName));
            var parents = ParentsViewModel.RelationIssues.Select(pi => pi.Model.Id);
            var children = ChildrenViewModel.RelationIssues.Select(pi => pi.Model.Id);
            Board.SetRelations(id, parents, children);
            if (!Board.UpdateAttachments(id, Attachments.Select(a => a.FileName).ToList(), FilePaths))
            {
                MessageBox.Show("Неудалось добавить выбранные файла.");
            }
            Board.Submit();
            base.Submit();
        }
    }
}