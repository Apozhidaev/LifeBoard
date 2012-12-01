using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Models.Configs;
using LifeBoard.Views.Issues;
using Issue = LifeBoard.Models.Issue;

namespace LifeBoard.ViewModels.Issues
{
    public class ShowIssueViewModel : PageViewModelBase
    {
        private Issue _issue;

        private IssueViewModel _issueModel;

        private readonly Board _board;

        private ShowIssueView _showIssueView;

        public ShowIssueViewModel(INavigatePage parent, Board board)
            : base(parent)
        {
            _board = board;
            Parents = new ObservableCollection<IssueViewModel>();
            Children = new ObservableCollection<IssueViewModel>();
            Attachments = new ObservableCollection<AttachmentViewModel>();
            IsActiveChildren = true;
            IsRootChildren = false;
        }

        public IssueViewModel IssueModel
        {
            get { return _issueModel; }
        }

        public ObservableCollection<IssueViewModel> Children { get; private set; }

        public ObservableCollection<IssueViewModel> Parents { get; private set; }

        public ObservableCollection<AttachmentViewModel> Attachments { get; private set; }

        public IEnumerable<IssueStatus> Statuses
        {
            get { return _board.GetStatuses(); }
        }

        public bool IsRootChildren { get; set; }

        public bool IsActiveChildren { get; set; }

        public string Summary
        {
            get { return _issue.Summary; }
        }

        public string Description
        {
            get { return _issue.Description; }
        }

        public string WebSite
        {
            get { return _issue.WebSite; }
        }

        public int Priority
        {
            get { return _issue.Priority; }
        }

        public IssueType IssueType
        {
            get { return _issue.Type; }
        }

        public string CreationDate
        {
            get { return _issue.CreationDate.ToShortDateString(); }
        }

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

        public override Page Page
        {
            get { return _showIssueView ?? (_showIssueView = new ShowIssueView(this)); }
        }

        public Visibility DescriptionVisibility
        {
            get { return String.IsNullOrEmpty(Description) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility WebLinkVisibility
        {
            get { return String.IsNullOrEmpty(WebSite) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility AttachmentsVisibility
        {
            get { return Attachments.Count == 0 ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility ChildrenVisibility
        {
            get { return Children.Count == 0 ? Visibility.Collapsed : Visibility.Visible; }
        }

        public bool IsParentsEnabled
        {
            get { return Parents.Count != 0; }
        }

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

        public void SetIssue(IssueViewModel issue)
        {
            _issueModel = issue;
            _issue = issue.Model;
            Attachments.Clear();
            foreach (var attachment in _board.GetAttachments(_issue.Id))
            {
                Attachments.Add(new AttachmentViewModel(this) { FileName = attachment });
            }
            UpdateParents();
            UpdateChildren();
            UpdateSource();
        }

        private DelegateCommand _updateChildrenCommand;

        public ICommand UpdateChildrenCommand
        {
            get { return _updateChildrenCommand ?? (_updateChildrenCommand = new DelegateCommand(UpdateChildren)); }
        }

        private DelegateCommand _goHttpCommand;

        public ICommand GoHttpCommand
        {
            get { return _goHttpCommand ?? (_goHttpCommand = new DelegateCommand(GoHttp)); }
        }

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

        public async void UpdateChildren()
        {
            var issues = IsRootChildren
                ? await Task<IEnumerable<Issue>>.Factory.StartNew(() => _board.GetRootChildren(_issue.Id))
                : await Task<IEnumerable<Issue>>.Factory.StartNew(() => _board.GetChildren(_issue.Id));           
            if (IsActiveChildren)
            {
                issues = issues.Where(c => c.Status != IssueStatus.Closed);
            }
            Children.Clear();
            foreach (var child in issues)
            {
                Children.Add(new IssueViewModel(this, child));
            }
            OnPropertyChanged("ChildrenVisibility");
        }

        public async void UpdateParents()
        {
            var parents = await Task<IEnumerable<Issue>>.Factory.StartNew(() => _board.GetParents(_issue.Id));
            Parents.Clear();
            foreach (var issue in parents)
            {
                Parents.Add(new IssueViewModel(this, issue));
            }
            if (!IsParentsEnabled && IsParentsChecked)
            {
                IsParentsChecked = false;
            }
            OnPropertyChanged("IsParentsEnabled");
        }

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

        private DelegateCommand<AttachmentViewModel> _openAttachmentCommand;

        private bool _isParentsChecked;

        public ICommand OpenAttachmentCommand
        {
            get { return _openAttachmentCommand ?? (_openAttachmentCommand = new DelegateCommand<AttachmentViewModel>(OpenAttachment)); }
        }

        private void OpenAttachment(AttachmentViewModel file)
        {
            if (!_board.OpenAttachment(_issue.Id, file.FileName))
            {
                MessageBox.Show("Can not open file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
