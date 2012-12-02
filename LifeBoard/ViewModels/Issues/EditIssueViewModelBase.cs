using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;
using Microsoft.Win32;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class EditIssueViewModelBase
    /// </summary>
    public class EditIssueViewModelBase : PageViewModelBase
    {
        /// <summary>
        /// The _board
        /// </summary>
        private readonly Board _board;
        /// <summary>
        /// The _parent
        /// </summary>
        private readonly INavigatePage _parent;
        /// <summary>
        /// The _add attachment command
        /// </summary>
        private DelegateCommand _addAttachmentCommand;
        /// <summary>
        /// The _add command
        /// </summary>
        private DelegateCommand<IssueViewModel> _addCommand;
        /// <summary>
        /// The _clear command
        /// </summary>
        private DelegateCommand _clearCommand;
        /// <summary>
        /// The _description
        /// </summary>
        private string _description;
        /// <summary>
        /// The _edit issue view
        /// </summary>
        private EditIssueView _editIssueView;
        /// <summary>
        /// The _insert command
        /// </summary>
        private DelegateCommand<string> _insertCommand;
        /// <summary>
        /// The _is custom root
        /// </summary>
        private bool _isCustomRoot;
        /// <summary>
        /// The _priority
        /// </summary>
        private int _priority;
        /// <summary>
        /// The _query
        /// </summary>
        private string _query = String.Empty;
        /// <summary>
        /// The _remove attachment command
        /// </summary>
        private DelegateCommand<AttachmentViewModel> _removeAttachmentCommand;
        /// <summary>
        /// The _remove command
        /// </summary>
        private DelegateCommand<IssueViewModel> _removeCommand;
        /// <summary>
        /// The _selection start
        /// </summary>
        private int _selectionStart;
        /// <summary>
        /// The _submit command
        /// </summary>
        private DelegateCommand _submitCommand;
        /// <summary>
        /// The _summary
        /// </summary>
        private string _summary;
        /// <summary>
        /// The _type
        /// </summary>
        private IssueType _type;
        /// <summary>
        /// The _web site
        /// </summary>
        private string _webSite;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditIssueViewModelBase" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="board">The board.</param>
        public EditIssueViewModelBase(INavigatePage parent, Board board)
            : base(parent)
        {
            _parent = parent;
            _board = board;
            Issues = new ObservableCollection<IssueViewModel>();
            ParentIssues = new ObservableCollection<IssueViewModel>();
            Attachments = new ObservableCollection<AttachmentViewModel>();
            FilePaths = new Dictionary<string, string>();
            SubmitHeader = "Submit";
        }

        #region Commands

        /// <summary>
        /// The _search command
        /// </summary>
        private DelegateCommand _searchCommand;

        /// <summary>
        /// Gets the add attachment command.
        /// </summary>
        /// <value>The add attachment command.</value>
        public ICommand AddAttachmentCommand
        {
            get { return _addAttachmentCommand ?? (_addAttachmentCommand = new DelegateCommand(AddAttachment)); }
        }

        /// <summary>
        /// Gets the remove attachment command.
        /// </summary>
        /// <value>The remove attachment command.</value>
        public ICommand RemoveAttachmentCommand
        {
            get
            {
                return _removeAttachmentCommand ??
                       (_removeAttachmentCommand = new DelegateCommand<AttachmentViewModel>(RemoveAttachment));
            }
        }

        /// <summary>
        /// Gets the submit command.
        /// </summary>
        /// <value>The submit command.</value>
        public ICommand SubmitCommand
        {
            get { return _submitCommand ?? (_submitCommand = new DelegateCommand(Submit, CanSubmit)); }
        }

        /// <summary>
        /// Gets the insert command.
        /// </summary>
        /// <value>The insert command.</value>
        public ICommand InsertCommand
        {
            get { return _insertCommand ?? (_insertCommand = new DelegateCommand<string>(Insert)); }
        }

        /// <summary>
        /// Gets the add command.
        /// </summary>
        /// <value>The add command.</value>
        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new DelegateCommand<IssueViewModel>(Add, CanAdd)); }
        }

        /// <summary>
        /// Gets the remove command.
        /// </summary>
        /// <value>The remove command.</value>
        public ICommand RemoveCommand
        {
            get { return _removeCommand ?? (_removeCommand = new DelegateCommand<IssueViewModel>(Remove)); }
        }

        /// <summary>
        /// Gets the search command.
        /// </summary>
        /// <value>The search command.</value>
        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new DelegateCommand(Search)); }
        }

        /// <summary>
        /// Inserts the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private void Insert(string obj)
        {
            int index = SelectionStart + 1;
            Description = Description.Remove(SelectionStart, SelectionLength).Insert(SelectionStart, obj);
            SelectionStart = index;
        }

        /// <summary>
        /// Submits this instance.
        /// </summary>
        protected virtual void Submit()
        {
            _parent.BackCommand.Execute(null);
        }

        /// <summary>
        /// Adds the attachment.
        /// </summary>
        private void AddAttachment()
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                string fileName = Path.GetFileName(dlg.FileName);
                if (fileName != null && Attachments.All(f => f.FileName != fileName))
                {
                    FilePaths.Add(fileName, dlg.FileName);
                    Attachments.Add(new AttachmentViewModel(this) {FileName = fileName});
                }
                else
                {
                    MessageBox.Show("Файл с таким именем уже существует");
                    AddAttachment();
                }
            }
        }

        /// <summary>
        /// Removes the attachment.
        /// </summary>
        /// <param name="file">The file.</param>
        private void RemoveAttachment(AttachmentViewModel file)
        {
            Attachments.Remove(file);
            FilePaths.Remove(file.FileName);
        }

        /// <summary>
        /// Determines whether this instance can submit.
        /// </summary>
        /// <returns><c>true</c> if this instance can submit; otherwise, <c>false</c>.</returns>
        private bool CanSubmit()
        {
            return !String.IsNullOrEmpty(Summary);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        private void Add(IssueViewModel item)
        {
            ParentIssues.Add(item);
        }

        /// <summary>
        /// Determines whether this instance can add the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if this instance can add the specified item; otherwise, <c>false</c>.</returns>
        private bool CanAdd(IssueViewModel item)
        {
            return item != null && ParentIssues.All(pi => pi.Model.Id != item.Model.Id);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        private void Remove(IssueViewModel item)
        {
            ParentIssues.Remove(item);
        }

        /// <summary>
        /// Searches this instance.
        /// </summary>
        private async void Search()
        {
            Issues.Clear();
            foreach (Issue issue in await Task<IEnumerable<Issue>>.Factory.StartNew(GetFilterIssues))
            {
                Issues.Add(new IssueViewModel(this, issue));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file paths.
        /// </summary>
        /// <value>The file paths.</value>
        public Dictionary<string, string> FilePaths { get; private set; }

        /// <summary>
        /// Gets or sets the submit header.
        /// </summary>
        /// <value>The submit header.</value>
        public string SubmitHeader { get; set; }

        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <value>The issues.</value>
        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        /// <summary>
        /// Gets the parent issues.
        /// </summary>
        /// <value>The parent issues.</value>
        public ObservableCollection<IssueViewModel> ParentIssues { get; private set; }

        /// <summary>
        /// Gets the attachments.
        /// </summary>
        /// <value>The attachments.</value>
        public ObservableCollection<AttachmentViewModel> Attachments { get; private set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
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

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
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

        /// <summary>
        /// Gets the priorities.
        /// </summary>
        /// <value>The priorities.</value>
        public IEnumerable<int> Priorities
        {
            get { return _board.GetPriorities(); }
        }

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <value>The types.</value>
        public IEnumerable<IssueType> Types
        {
            get { return _board.GetTypes(); }
        }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
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

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
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

        /// <summary>
        /// Gets or sets the web site.
        /// </summary>
        /// <value>The web site.</value>
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

        /// <summary>
        /// Gets or sets a value indicating whether this instance is custom root.
        /// </summary>
        /// <value><c>true</c> if this instance is custom root; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <value>The page.</value>
        public override Page Page
        {
            get { return _editIssueView ?? (_editIssueView = new EditIssueView(this)); }
        }

        /// <summary>
        /// Called when [navigated].
        /// </summary>
        protected override void OnNavigated()
        {
            Search();
            base.OnNavigated();
        }

        #endregion

        /// <summary>
        /// Gets the board.
        /// </summary>
        /// <value>The board.</value>
        protected Board Board
        {
            get { return _board; }
        }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        public string Query
        {
            get { return _query; }
            set
            {
                if (_query != value)
                {
                    _query = value;
                    Search();
                    OnPropertyChanged("Query");
                }
            }
        }

        /// <summary>
        /// Gets the clear command.
        /// </summary>
        /// <value>The clear command.</value>
        public ICommand ClearCommand
        {
            get { return _clearCommand ?? (_clearCommand = new DelegateCommand(Clear)); }
        }

        /// <summary>
        /// Gets or sets the length of the selection.
        /// </summary>
        /// <value>The length of the selection.</value>
        public int SelectionLength { get; set; }

        /// <summary>
        /// Gets or sets the selection start.
        /// </summary>
        /// <value>The selection start.</value>
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

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Query = String.Empty;
        }

        /// <summary>
        /// Adds the parent.
        /// </summary>
        /// <param name="issue">The issue.</param>
        public void AddParent(IssueViewModel issue)
        {
            ParentIssues.Add(issue);
        }

        /// <summary>
        /// Gets the filter issues.
        /// </summary>
        /// <returns>IEnumerable{Issue}.</returns>
        protected virtual IEnumerable<Issue> GetFilterIssues()
        {
            return _board.GetIssues(Query);
        }

        /// <summary>
        /// Clears the form.
        /// </summary>
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
    }
}