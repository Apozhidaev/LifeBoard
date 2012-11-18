using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class CreateIssueViewModel : EditIssueViewModelBase
    {
        public CreateIssueViewModel(IssuesViewModel parent, BoardService boardService)
            : base(parent, boardService)
        {
        }

        private void ClearForm()
        {
            Type = IssueType.Task;
            Priority = 3;
            Summary = String.Empty;
            Description = String.Empty;
        }

        public override void Navigate()
        {
            ClearForm();
            base.Navigate();
        }

        protected override void Submit()
        {
            BoardService.CreateIssue(Type, Priority, Summary, Description);
            base.Submit();
        }
    }
}
