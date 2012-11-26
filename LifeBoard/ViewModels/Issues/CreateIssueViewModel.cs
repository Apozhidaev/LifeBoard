using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class CreateIssueViewModel : EditIssueViewModelBase
    {
        public CreateIssueViewModel(INavigatePage parent, BoardService boardService)
            : base(parent, boardService)
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
            int id = BoardService.CreateIssue(Type, Priority, Summary, Description, IsCustomRoot, WebLink);
            var parents = ParentIssues.Select(pi => pi.Model.Id);
            BoardService.SetParents(id, parents);
            BoardService.Submit();
            base.Submit();
        }
    }
}
