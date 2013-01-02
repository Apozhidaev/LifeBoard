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

        private readonly Board _board;

        private readonly INavigatePage _parent;

        private DelegateCommand _addAttachmentCommand;

        private DelegateCommand _addLinkCommand;

        private DelegateCommand _clearDeadlineCommand;

        private string _description;

        private EditIssueView _editIssueView;

        private DelegateCommand<string> _insertCommand;

        private bool _isCustomRoot;

        private int _priority;

        private DelegateCommand<AttachmentViewModel> _removeAttachmentCommand;

        private DelegateCommand<LinkViewModel> _removeLinkCommand;

        private int _selectionStart;

        private DelegateCommand _submitCommand;

        private string _summary;

        private IssueType _type;

        private string _link;

        private string _deadline;

        public EditIssueViewModelBase(INavigatePage parent, Board board)
            : base(parent)
        {
            _parent = parent;
            _board = board;
            Attachments = new ObservableCollection<AttachmentViewModel>();
            Links = new ObservableCollection<LinkViewModel>();
            FilePaths = new Dictionary<string, string>();
            SubmitHeader = "Submit";

            ParentsViewModel = new EditRelationViewModel();
            ChildrenViewModel = new EditRelationViewModel();

            ParentsViewModel.AddCommand = new DelegateCommand<IssueViewModel>(AddParent, ParentsViewModel.CanAdd);
            ParentsViewModel.RemoveCommand = new DelegateCommand<IssueViewModel>(RemoveParent);
            ParentsViewModel.SearchCommand = new DelegateCommand(ParentSearch);

            ChildrenViewModel.AddCommand = new DelegateCommand<IssueViewModel>(AddChild, ChildrenViewModel.CanAdd);
            ChildrenViewModel.RemoveCommand = new DelegateCommand<IssueViewModel>(RemoveChild);
            ChildrenViewModel.SearchCommand = new DelegateCommand(ChildSearch);
        }

        public EditRelationViewModel ParentsViewModel { get; private set; }

        public EditRelationViewModel ChildrenViewModel { get; private set; }

        /// <summary>
        /// Adds the parent.
        /// </summary>
        /// <param name="issue">The issue.</param>
        public void AddParent(IssueViewModel issue)
        {
            ParentsViewModel.RelationIssues.Add(issue);
            ChildSearch();
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        private void RemoveParent(IssueViewModel item)
        {
            ParentsViewModel.RelationIssues.Remove(item);
            ChildSearch();
        }

        /// <summary>
        /// Adds the parent.
        /// </summary>
        /// <param name="issue">The issue.</param>
        private void AddChild(IssueViewModel issue)
        {
            ChildrenViewModel.RelationIssues.Add(issue);
            ParentSearch();
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        private void RemoveChild(IssueViewModel item)
        {
            ChildrenViewModel.RelationIssues.Remove(item);
            ParentSearch();
        }

        /// <summary>
        /// Searches this instance.
        /// </summary>
        private async void ParentSearch()
        {
            ParentsViewModel.Issues.Clear();
            foreach (Issue issue in await Task<IEnumerable<Issue>>.Factory.StartNew(GetParentIssues))
            {
                ParentsViewModel.Issues.Add(new IssueViewModel(ParentsViewModel, issue));
            }
        }

        /// <summary>
        /// Searches this instance.
        /// </summary>
        private async void ChildSearch()
        {
            ChildrenViewModel.Issues.Clear();
            foreach (Issue issue in await Task<IEnumerable<Issue>>.Factory.StartNew(GetChildIssues))
            {
                ChildrenViewModel.Issues.Add(new IssueViewModel(ChildrenViewModel, issue));
            }
        }

        /// <summary>
        /// Gets the filter issues.
        /// </summary>
        /// <returns>IEnumerable{Issue}.</returns>
        protected virtual IEnumerable<Issue> GetParentIssues()
        {
            return _board.GetIssuesExeptChildren(ChildrenViewModel.RelationIssues.Select(ci => ci.Model.Id), ParentsViewModel.Query);
        }

        /// <summary>
        /// Gets the filter issues.
        /// </summary>
        /// <returns>IEnumerable{Issue}.</returns>
        protected virtual IEnumerable<Issue> GetChildIssues()
        {
            return _board.GetIssuesExeptParents(ParentsViewModel.RelationIssues.Select(pi => pi.Model.Id), ChildrenViewModel.Query);
        }

        #region Commands
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

        public ICommand AddLinkCommand
        {
            get { return _addLinkCommand ?? (_addLinkCommand = new DelegateCommand(AddLink, CanAddLink)); }
        }

        public ICommand RemoveLinkCommand
        {
            get
            {
                return _removeLinkCommand ??
                       (_removeLinkCommand = new DelegateCommand<LinkViewModel>(RemoveLink));
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


        private void AddLink()
        {
            Links.Add(new LinkViewModel(this) {LinkName = Link});
            Link = String.Empty;
        }

        private bool CanAddLink()
        {
            return !String.IsNullOrEmpty(Link);
        }

        private void RemoveLink(LinkViewModel link)
        {
            Links.Remove(link);
        }

        /// <summary>
        /// Determines whether this instance can submit.
        /// </summary>
        /// <returns><c>true</c> if this instance can submit; otherwise, <c>false</c>.</returns>
        private bool CanSubmit()
        {
            return !String.IsNullOrEmpty(Summary);
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
        /// Gets the attachments.
        /// </summary>
        /// <value>The attachments.</value>
        public ObservableCollection<AttachmentViewModel> Attachments { get; private set; }

        public ObservableCollection<LinkViewModel> Links { get; private set; }

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
        public string Link
        {
            get { return _link; }
            set
            {
                if (_link != value)
                {
                    _link = value;
                    OnPropertyChanged("Link");
                }
            }
        }

        public string Deadline
        {
            get { return _deadline; }
            set
            {
                if (_deadline != value)
                {
                    _deadline = value;
                    OnPropertyChanged("Deadline");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is custom root.public string Deadline { get; set; }
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
            ParentSearch();
            ChildSearch();
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

        public ICommand ClearDeadlineCommand
        {
            get { return _clearDeadlineCommand ?? (_clearDeadlineCommand = new DelegateCommand(ClearDeadline)); }
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


        public void ClearDeadline()
        {
            Deadline = String.Empty;
        }

        /// <summary>
        /// Clears the form.
        /// </summary>
        public virtual void ClearForm()
        {
            Type = IssueType.Note;
            Priority = 3;
            Summary = String.Empty;
            Description = String.Empty;
            IsCustomRoot = false;
            Link = String.Empty;
            Deadline = String.Empty;
            Attachments.Clear();
            FilePaths.Clear();
            Links.Clear();
            ParentsViewModel.ClearForm();
            ChildrenViewModel.ClearForm();
        }
    }
}