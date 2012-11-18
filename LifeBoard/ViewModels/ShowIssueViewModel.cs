﻿using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.ViewModels.Issues;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels
{
    public class ShowIssueViewModel : BackPageViewModelBase
    {
        private Issue _issue;

        private readonly BoardService _boardService;

        private ShowIssueView _showIssueView;

        public ShowIssueViewModel(IFrameViewModel parent, ICommand backNavigateCommand, BoardService boardService)
            : base(parent, backNavigateCommand)
        {
            _boardService = boardService;
            Children = new ObservableCollection<IssueViewModel>();
        }

        public ObservableCollection<IssueViewModel> Children { get; private set; }

        public string Summary
        {
            get { return _issue.Summary; }
        }

        public string Description
        {
            get { return _issue.Description; }
        }

        public int Priority
        {
            get { return _issue.Priority; }
        }

        public IssueType IssueType
        {
            get { return _issue.Type; }
        }

        public IssueStatus Status
        {
            get { return _issue.Status; }
        }

        public IssueStatus NextStatus
        {
            get { return _issue.NextStatus; }
        }

        public override Page Page
        {
            get { return _showIssueView ?? (_showIssueView = new ShowIssueView(this)); }
        }

        public void Show(Issue issue)
        {
            _issue = issue;
            Children.Clear();
            foreach (var child in _boardService.GetChildren(_issue.Id))
            {
                Children.Add(new IssueViewModel(this, child));
            }
            UpdateSource();
            Navigate();
        }

        private void UpdateSource()
        {
            OnPropertyChanged("Summary");
            OnPropertyChanged("Description");
            OnPropertyChanged("Priority");
            OnPropertyChanged("IssueType");
            OnPropertyChanged("Status");
            OnPropertyChanged("NextStatus");
        }

        private DelegateCommand _nextStateCommand;

        public ICommand NextStateCommand
        {
            get { return _nextStateCommand ?? (_nextStateCommand = new DelegateCommand(NextState)); }
        }

        private void NextState()
        {
            _issue.NextState();
            OnPropertyChanged("Status");
            OnPropertyChanged("NextStatus");
        }
    }
}
