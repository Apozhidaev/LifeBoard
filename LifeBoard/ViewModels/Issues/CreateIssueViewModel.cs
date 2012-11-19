using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class CreateIssueViewModel : EditIssueViewModelBase
    {
        public CreateIssueViewModel(IFrameViewModel parent, ICommand backNavigateCommand, BoardService boardService)
            : base(parent, backNavigateCommand, boardService)
        {
            SubmitTitle = "Create";
        }

        public override void Navigate()
        {
            ClearForm();
            base.Navigate();
        }

        protected override void Submit()
        {
            int id = BoardService.CreateIssue(Type, Priority, Summary, Description);
            var parents = ParentIssues.Select(pi => pi.Model.Id);
            BoardService.SetParents(id, parents);
            BoardService.Submit();
            base.Submit();
        }
    }
}
