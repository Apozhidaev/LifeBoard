using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LifeBoard.Models
{
    public class Board
    {
        private readonly DocumentRepository _documentRepository = new DocumentRepository();

        private readonly Dictionary<int, HashSet<int>> _parentChildren = new Dictionary<int, HashSet<int>>();

        private readonly Dictionary<int, HashSet<int>> _childParents = new Dictionary<int, HashSet<int>>();

        public string DocumentPath
        {
            get { return _documentRepository.DocumentPath; }
        }

        public Document Document
        {
            get { return _documentRepository.Document; }
        }

        public bool IsFileExists
        {
            get { return _documentRepository.IsFileExists; }
        }

        public void Submit()
        {
            _documentRepository.Submit();
        }

        public bool OpenDocument(string documentPath)
        {
            bool isOpen = _documentRepository.Open(documentPath);
            if(!isOpen)
            {
                _documentRepository.New();
            }
            UpdateLinks();
            return isOpen;
        }

        public bool SaveDocument(string documentPath)
        {
            return _documentRepository.Save(documentPath);
        }

        public void SetParents(int id, IEnumerable<int> parents)
        {
            _documentRepository.SetParents(id, parents);
            UpdateLinks();
        }

        public IEnumerable<Issue> GetParents(int id)
        {
            return Document.Issues.Values.Where(i => _parentChildren.ContainsKey(i.Id) && _parentChildren[i.Id].Contains(id));
        }

        public IEnumerable<Issue> GetChildren(int id)
        {
            return Document.Issues.Values.Where(i => _childParents.ContainsKey(i.Id) && _childParents[i.Id].Contains(id));
        }

        public IEnumerable<Issue> GetRootChildren(int id)
        {
            var children = GetChildren(id).ToList();
            var allChildren = GetAllChildren(id).ToList();
            return children.Where(child => allChildren.All(c => !_childParents[child.Id].Contains(c.Id))).ToList();
        }

        public IEnumerable<Issue> GetAllChildren(int id)
        {
            var allChildren = new HashSet<int>();
            AddChildrenTo(id, allChildren);
            return allChildren.Select(child => Document.Issues[child]);
        }

        private void AddChildrenTo(int id, HashSet<int> allChildren)
        {
            if (_parentChildren.ContainsKey(id))
            {
                foreach (var child in _parentChildren[id])
                {
                    allChildren.Add(child);
                    AddChildrenTo(child, allChildren);
                }
            }
        }

        public IEnumerable<Issue> GetIssues()
        {
            return Document.Issues.Values;
        }

        public IEnumerable<Issue> GetIssues(IssueFilter filter)
        {
            if (filter.HasQuery)
            {
                return Document.Issues.Values.Where(
                    i => filter.Types.Contains(i.Type) &&
                         filter.Statuses.Contains(i.Status) &&
                         filter.Priorities.Contains(i.Priority) &&
                         Regex.IsMatch(i.Summary.ToLower(), filter.Query.ToLower()));
            }
            return Document.Issues.Values.Where(
                    i => filter.Types.Contains(i.Type) &&
                         filter.Statuses.Contains(i.Status) &&
                         filter.Priorities.Contains(i.Priority));
        }

        public IEnumerable<Issue> GetRootIssues()
        {
            return Document.Issues.Values.Where(i => !_childParents.ContainsKey(i.Id));
        }

        public IEnumerable<Issue> GetCustomRootIssues()
        {
            return Document.Issues.Values.Where(i => i.IsCustomRoot);
        }

        public IEnumerable<Issue> GetIssuesExeptChildren(int id, string query)
        {
            var allChildren = new HashSet<int>();
            AddChildrenTo(id, allChildren);
            return GetIssues(query).Where(i => i.Id != id && !allChildren.Contains(i.Id));
        }

        public void DeleteIssue(Issue issue)
        {
            _documentRepository.DeleteIssue(issue);
            UpdateLinks();
        }

        private void UpdateLinks()
        {
            _parentChildren.Clear();
            _childParents.Clear();
            foreach (var projectIssue in Document.IssuesLinks)
            {
                if (!_parentChildren.ContainsKey(projectIssue.ParentId))
                {
                    _parentChildren.Add(projectIssue.ParentId, new HashSet<int>());
                }
                _parentChildren[projectIssue.ParentId].Add(projectIssue.ChildId);

                if (!_childParents.ContainsKey(projectIssue.ChildId))
                {
                    _childParents.Add(projectIssue.ChildId, new HashSet<int>());
                }
                _childParents[projectIssue.ChildId].Add(projectIssue.ParentId);
            }
        }

        public int CreateIssue(IssueType type, int priority, string summary, string description, bool isCustomRoot, string httpLink)
        {
            return _documentRepository.CreateIssue(type, priority, summary, description, isCustomRoot, httpLink);
        }

        public IEnumerable<int> GetPriorities()
        {
            return new[] { 1, 2, 3, 4, 5 };
        }

        public IEnumerable<IssueType> GetTypes()
        {
            return new[] { IssueType.Note, IssueType.Task, IssueType.Story, IssueType.Epic };
        }

        public IEnumerable<IssueStatus> GetStatuses()
        {
            return new[] {IssueStatus.Open, IssueStatus.InProgress, IssueStatus.Resolved, IssueStatus.Closed};
        }

        public IssueFilter GetDefaultFilter()
        {
            var filter = new IssueFilter();
            filter.Priorities = new HashSet<int>(GetPriorities());
            filter.Statuses = new HashSet<IssueStatus>(new[] { IssueStatus.Open, IssueStatus.InProgress, IssueStatus.Resolved });
            filter.Types = new HashSet<IssueType>(new[] { IssueType.Epic });
            return filter;
        }

        public IssueFilter GetFullFilter()
        {
            var filter = new IssueFilter();
            filter.Priorities = new HashSet<int>(GetPriorities());
            filter.Statuses = new HashSet<IssueStatus>(GetStatuses());
            filter.Types = new HashSet<IssueType>(GetTypes());
            return filter;
        }

        public bool UpdateAttachments(int id, List<string> attachments, Dictionary<string, string> filePaths)
        {
            return _documentRepository.UpdateAttachments(id, attachments, filePaths);
        }

        public bool OpenAttachment(int id, string fileName)
        {
            return _documentRepository.OpenAttachment(id, fileName);
        }

        public IEnumerable<string> GetAttachments(int id)
        {
            return _documentRepository.GetAttachments(id).Select(Path.GetFileName);
        }

        public IEnumerable<Issue> GetIssues(string query)
        {
            if(String.IsNullOrEmpty(query))
            {
                return Document.Issues.Values;
            }
            return Document.Issues.Values.Where(i => Regex.IsMatch(i.Summary.ToLower(), query.ToLower()));
        }
    }
}