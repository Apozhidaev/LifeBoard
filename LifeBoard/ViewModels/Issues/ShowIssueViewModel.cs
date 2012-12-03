using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class ShowIssueViewModel
    /// </summary>
    public class ShowIssueViewModel : PageViewModelBase
    {
        /// <summary>
        /// The _board
        /// </summary>
        private readonly Board _board;
        /// <summary>
        /// The _go HTTP command
        /// </summary>
        private DelegateCommand _goHttpCommand;
        /// <summary>
        /// The _is parents checked
        /// </summary>
        private bool _isParentsChecked;
        /// <summary>
        /// The _issue
        /// </summary>
        private Issue _issue;

        /// <summary>
        /// The _issue model
        /// </summary>
        private IssueViewModel _issueModel;
        /// <summary>
        /// The _open attachment command
        /// </summary>
        private DelegateCommand<AttachmentViewModel> _openAttachmentCommand;

        /// <summary>
        /// The _show issue view
        /// </summary>
        private ShowIssueView _showIssueView;
        /// <summary>
        /// The _update children command
        /// </summary>
        private DelegateCommand _updateChildrenCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowIssueViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="board">The board.</param>
        public ShowIssueViewModel(INavigatePage parent, Board board)
            : base(parent)
        {
            _board = board;
            Parents = new ObservableCollection<IssueViewModel>();
            Children = new ObservableCollection<IssueViewModel>();
            Attachments = new ObservableCollection<AttachmentViewModel>();
            IsActiveChildren = true;
            IsRootChildren = true;
        }

        /// <summary>
        /// Gets the issue model.
        /// </summary>
        /// <value>The issue model.</value>
        public IssueViewModel IssueModel
        {
            get { return _issueModel; }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public ObservableCollection<IssueViewModel> Children { get; private set; }

        /// <summary>
        /// Gets the parents.
        /// </summary>
        /// <value>The parents.</value>
        public ObservableCollection<IssueViewModel> Parents { get; private set; }

        /// <summary>
        /// Gets the attachments.
        /// </summary>
        /// <value>The attachments.</value>
        public ObservableCollection<AttachmentViewModel> Attachments { get; private set; }

        /// <summary>
        /// Gets the statuses.
        /// </summary>
        /// <value>The statuses.</value>
        public IEnumerable<IssueStatus> Statuses
        {
            get { return _board.GetStatuses(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is root children.
        /// </summary>
        /// <value><c>true</c> if this instance is root children; otherwise, <c>false</c>.</value>
        public bool IsRootChildren { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active children.
        /// </summary>
        /// <value><c>true</c> if this instance is active children; otherwise, <c>false</c>.</value>
        public bool IsActiveChildren { get; set; }

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary
        {
            get { return _issue.Summary; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return _issue.Description; }
        }

        /// <summary>
        /// Gets the web site.
        /// </summary>
        /// <value>The web site.</value>
        public string WebSite
        {
            get { return _issue.WebSite; }
        }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public int Priority
        {
            get { return _issue.Priority; }
        }

        /// <summary>
        /// Gets the type of the issue.
        /// </summary>
        /// <value>The type of the issue.</value>
        public IssueType IssueType
        {
            get { return _issue.Type; }
        }

        /// <summary>
        /// Gets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        public string CreationDate
        {
            get { return _issue.CreationDate.ToShortDateString(); }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public IssueStatus Status
        {
            get { return _issue.Status; }
            set
            {
                if (_issue.Status != value)
                {
                    _issue.Status = value;
                    _board.Submit();
                    OnPropertyChanged("Status");
                }
            }
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <value>The page.</value>
        public override Page Page
        {
            get { return _showIssueView ?? (_showIssueView = new ShowIssueView(this)); }
        }

        /// <summary>
        /// Gets the description visibility.
        /// </summary>
        /// <value>The description visibility.</value>
        public Visibility DescriptionVisibility
        {
            get { return String.IsNullOrEmpty(Description) ? Visibility.Collapsed : Visibility.Visible; }
        }

        /// <summary>
        /// Gets the web link visibility.
        /// </summary>
        /// <value>The web link visibility.</value>
        public Visibility WebLinkVisibility
        {
            get { return String.IsNullOrEmpty(WebSite) ? Visibility.Collapsed : Visibility.Visible; }
        }

        /// <summary>
        /// Gets the attachments visibility.
        /// </summary>
        /// <value>The attachments visibility.</value>
        public Visibility AttachmentsVisibility
        {
            get { return Attachments.Count == 0 ? Visibility.Collapsed : Visibility.Visible; }
        }

        /// <summary>
        /// Gets the children visibility.
        /// </summary>
        /// <value>The children visibility.</value>
        public Visibility ChildrenVisibility
        {
            get { return Children.Count == 0 ? Visibility.Collapsed : Visibility.Visible; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is parents enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is parents enabled; otherwise, <c>false</c>.</value>
        public bool IsParentsEnabled
        {
            get { return Parents.Count != 0; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is parents checked.
        /// </summary>
        /// <value><c>true</c> if this instance is parents checked; otherwise, <c>false</c>.</value>
        public bool IsParentsChecked
        {
            get { return _isParentsChecked; }
            set
            {
                if (_isParentsChecked != value)
                {
                    _isParentsChecked = value;
                    OnPropertyChanged("IsParentsChecked");
                }
            }
        }

        /// <summary>
        /// Gets the update children command.
        /// </summary>
        /// <value>The update children command.</value>
        public ICommand UpdateChildrenCommand
        {
            get { return _updateChildrenCommand ?? (_updateChildrenCommand = new DelegateCommand(UpdateChildren)); }
        }

        /// <summary>
        /// Gets the go HTTP command.
        /// </summary>
        /// <value>The go HTTP command.</value>
        public ICommand GoHttpCommand
        {
            get { return _goHttpCommand ?? (_goHttpCommand = new DelegateCommand(GoHttp)); }
        }

        /// <summary>
        /// Gets the open attachment command.
        /// </summary>
        /// <value>The open attachment command.</value>
        public ICommand OpenAttachmentCommand
        {
            get
            {
                return _openAttachmentCommand ??
                       (_openAttachmentCommand = new DelegateCommand<AttachmentViewModel>(OpenAttachment));
            }
        }

        /// <summary>
        /// Sets the issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        public void SetIssue(IssueViewModel issue)
        {
            _issueModel = issue;
            _issue = issue.Model;
            Attachments.Clear();
            foreach (string attachment in _board.GetAttachments(_issue.Id))
            {
                Attachments.Add(new AttachmentViewModel(this) {FileName = attachment});
            }
            UpdateParents();
            UpdateChildren();
            UpdateSource();
        }

        /// <summary>
        /// Goes the HTTP.
        /// </summary>
        public void GoHttp()
        {
            try
            {
                Process.Start(WebSite);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to link", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Updates the children.
        /// </summary>
        public async void UpdateChildren()
        {
            IEnumerable<Issue> issues = IsRootChildren
                                            ? await
                                              Task<IEnumerable<Issue>>.Factory.StartNew(
                                                  () => _board.GetRootChildren(_issue.Id))
                                            : await
                                              Task<IEnumerable<Issue>>.Factory.StartNew(
                                                  () => _board.GetChildren(_issue.Id));
            if (IsActiveChildren)
            {
                issues = issues.Where(c => c.Status != IssueStatus.Closed);
            }
            Children.Clear();
            foreach (Issue child in issues)
            {
                Children.Add(new IssueViewModel(this, child));
            }
            OnPropertyChanged("ChildrenVisibility");
        }

        /// <summary>
        /// Updates the parents.
        /// </summary>
        public async void UpdateParents()
        {
            IEnumerable<Issue> parents =
                await Task<IEnumerable<Issue>>.Factory.StartNew(() => _board.GetParents(_issue.Id));
            Parents.Clear();
            foreach (Issue issue in parents)
            {
                Parents.Add(new IssueViewModel(this, issue));
            }
            if (!IsParentsEnabled && IsParentsChecked)
            {
                IsParentsChecked = false;
            }
            OnPropertyChanged("IsParentsEnabled");
        }

        /// <summary>
        /// Updates the source.
        /// </summary>
        private void UpdateSource()
        {
            OnPropertyChanged("IssueModel");
            OnPropertyChanged("Summary");
            OnPropertyChanged("Description");
            OnPropertyChanged("Priority");
            OnPropertyChanged("IssueType");
            OnPropertyChanged("Status");
            OnPropertyChanged("WebSite");
            OnPropertyChanged("CreationDate");
            OnPropertyChanged("DescriptionVisibility");
            OnPropertyChanged("WebLinkVisibility");
            OnPropertyChanged("AttachmentsVisibility");

            var config = Page.Resources["Config"] as ConfigShowIssueViewModel;
            if (config != null)
            {
                config.Update();
            }
        }

        /// <summary>
        /// Opens the attachment.
        /// </summary>
        /// <param name="file">The file.</param>
        private void OpenAttachment(AttachmentViewModel file)
        {
            if (!_board.OpenAttachment(_issue.Id, file.FileName))
            {
                MessageBox.Show("Can not open file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}