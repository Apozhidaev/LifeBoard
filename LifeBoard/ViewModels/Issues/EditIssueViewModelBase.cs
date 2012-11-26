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
        private int _selectionStart;
        private string _webSite;
        private bool _isCustomRoot;
        private EditIssueView _editIssueView;  
        private readonly INavigatePage _parent;
        private readonly BoardService _boardService;    
        private DelegateCommand _submitCommand;
        private DelegateCommand<IssueViewModel> _addCommand;
        private DelegateCommand<IssueViewModel> _removeCommand;
        private DelegateCommand<string> _insertCommand;

        public EditIssueViewModelBase(INavigatePage parent, BoardService boardService)
            : base(parent)
        {
            _parent = parent;
            _boardService = boardService;
            Issues = new ObservableCollection<IssueViewModel>();
            ParentIssues = new ObservableCollection<IssueViewModel>();
            Filter = new FilterViewModel(this);
            Filter.SetModel(boardService.GetFullFilter(), boardService.GetFullFilter());
            SubmitHeader = "Submit";
        }

        #region Commands

        public ICommand SubmitCommand
        {
            get { return _submitCommand ?? (_submitCommand = new DelegateCommand(Submit, CanSubmit)); }
        }

        public ICommand InsertCommand
        {
            get { return _insertCommand ?? (_insertCommand = new DelegateCommand<string>(Insert)); }
        }

        private void Insert(string obj)
        {
            int index = SelectionStart + 1;
            Description = Description.Remove(SelectionStart, SelectionLength).Insert(SelectionStart, obj);
            SelectionStart = index;
        }

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
            _parent.BackCommand.Execute(null);
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
            var issues = GetFilterIssues();
            Issues.Clear();
            foreach (var issue in issues)
            {
                Issues.Add(new IssueViewModel(this, issue));
            }
        }

        #endregion

        #region Properties

        public string SubmitHeader { get; set; }

        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        public ObservableCollection<IssueViewModel> ParentIssues { get; private set; }

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

        public string WebSite
        {
            get { return _webSite; }
            set
            {
                if (_webSite != value)
                {
                    _webSite = value;
                    OnPropertyChanged("WebSite");
                }
            }
        }

        public bool IsCustomRoot
        {
            get { return _isCustomRoot; }
            set
            {
                if (_isCustomRoot != value)
                {
                    _isCustomRoot = value;
                    OnPropertyChanged("IsCustomRoot");
                }
            }
        }

        #endregion

        #region Overrides

        public override Page Page
        {
            get { return _editIssueView ?? (_editIssueView = new EditIssueView(this)); }
        }

        protected override void OnNavigated()
        {
            Search();
            base.OnNavigated();
        }

        #endregion

        protected BoardService BoardService
        {
            get { return _boardService; }
        }

        public void AddParent(IssueViewModel issue)
        {
            ParentIssues.Add(issue);
        }

        protected virtual IEnumerable<Issue> GetFilterIssues()
        {
            return _boardService.GetIssues(Filter.ToModel());
        }

        protected virtual void ClearForm()
        {
            Type = IssueType.Note;
            Priority = 3;
            Summary = String.Empty;
            Description = String.Empty;
            IsCustomRoot = false;
            WebSite = String.Empty;
            Issues.Clear();
            ParentIssues.Clear();
        }

        public int SelectionLength { get; set; }

        public int SelectionStart
        {
            get { return _selectionStart; }
            set
            {
                if (_selectionStart != value)
                {
                    _selectionStart = value;
                    OnPropertyChanged("SelectionStart");
                }
            }
        }
    }
}