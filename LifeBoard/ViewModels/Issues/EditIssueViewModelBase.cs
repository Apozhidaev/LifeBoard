using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class EditIssueViewModelBase : PageViewModelBase
    {
        private int _priority;
        private IssueType _type;
        private string _summary;
        private string _description;
        private readonly BoardService _boardService;
        private EditIssueView _editIssueView;
        private readonly IssuesViewModel _parent;
        private DelegateCommand _submitCommand;
        private DelegateCommand<IssueViewModel> _addCommand;
        private DelegateCommand<IssueViewModel> _removeCommand;

        public EditIssueViewModelBase(IssuesViewModel parent, BoardService boardService)
            : base(parent.Frame)
        {
            _parent = parent;
            _boardService = boardService;
            Issues = new ObservableCollection<IssueViewModel>();
            ParentIssues = new ObservableCollection<IssueViewModel>();
            Filter = new FilterViewModel(this);
            Filter.SetModel(boardService.GetFullFilter(), boardService.GetFullFilter());
        }

        public FilterViewModel Filter { get; private set; }

        public int Priority
        {
            get { return _priority; }
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged("Priority");
                }
            }
        }

        public IssueType Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    UpdateFilter();
                    Search();
                    OnPropertyChanged("Type");
                }
            }
        }

        public IEnumerable<int> Priorities
        {
            get { return _boardService.GetPriorities(); }
        }

        public IEnumerable<IssueType> Types
        {
            get { return _boardService.GetTypes(); }
        }

        public string Summary
        {
            get { return _summary; }
            set
            {
                if (_summary != value)
                {
                    _summary = value;
                    OnPropertyChanged("Summary");
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public override Page Page
        {
            get { return _editIssueView ?? (_editIssueView = new EditIssueView(this)); }
        }

        public ICommand SubmitCommand
        {
            get { return _submitCommand ?? (_submitCommand = new DelegateCommand(Submit, CanSubmit)); }
        }

        public ICommand BackNavigateCommand
        {
            get { return _parent.NavigateCommand; }
        }

        public ObservableCollection<IssueViewModel> Issues { get; set; }

        public ObservableCollection<IssueViewModel> ParentIssues { get; set; }

        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new DelegateCommand<IssueViewModel>(Add, CanAdd)); }
        }

        public ICommand RemoveCommand
        {
            get { return _removeCommand ?? (_removeCommand = new DelegateCommand<IssueViewModel>(Remove)); }
        }

        protected virtual void Submit()
        {
            _parent.Navigate();
        }

        protected BoardService BoardService
        {
            get { return _boardService; }
        }

        private bool CanSubmit()
        {
            return !String.IsNullOrEmpty(Summary);
        }

        private void Add(IssueViewModel item)
        {
            ParentIssues.Add(item);
        }

        private bool CanAdd(IssueViewModel item)
        {
            return item != null && ParentIssues.All(pi => pi.Model.Id != item.Model.Id);
        }

        private void Remove(IssueViewModel item)
        {
            ParentIssues.Remove(item);
        }

        private DelegateCommand _searchCommand;

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new DelegateCommand(Search)); }
        }

        private void Search()
        {
            var issues = _boardService.GetIssues(Filter.ToModel());
            Issues.Clear();
            foreach (var issue in issues)
            {
                Issues.Add(new IssueViewModel(this, issue));
            }
        }

        private void UpdateFilter()
        {
            var model = BoardService.GetFilter(Type);
            var fiter = Filter.ToModel();
            fiter.Types = new HashSet<IssueType>(model.Types);
            Filter.SetModel(model, fiter);
            var parents = ParentIssues.Where(pi => !model.Types.Contains(pi.IssueType)).ToList();
            foreach (var parent in parents)
            {
                ParentIssues.Remove(parent);
            }
        }
    }
}