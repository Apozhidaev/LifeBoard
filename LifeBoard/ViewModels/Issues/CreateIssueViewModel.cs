using System.Linq;
using System.Windows;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class CreateIssueViewModel : EditIssueViewModelBase
    {
        public CreateIssueViewModel(INavigatePage parent, Board board)
            : base(parent, board)
        {
            SubmitHeader = (string)Application.Current.FindResource("CreateHeader");
        }

        protected override void OnNavigated()
        {
            ClearForm();
            base.OnNavigated();
        }

        protected override void Submit()
        {
            int id = Board.CreateIssue(Type, Priority, Summary, Description, IsCustomRoot, WebSite);
            var parents = ParentIssues.Select(pi => pi.Model.Id);
            Board.SetParents(id, parents);
            if (!Board.UpdateAttachments(id, Attachments.Select(a => a.FileName).ToList(), FilePaths))
            {
                MessageBox.Show("Неудалось добавить выбранные файла.");
            }
            Board.Submit();
            base.Submit();
        }
    }
}
