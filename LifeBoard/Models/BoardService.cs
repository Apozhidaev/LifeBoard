using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace LifeBoard.Models
{
    public class BoardService
    {
        private readonly Document _document;

        private readonly DocumentRepository _repository;

        private readonly Dictionary<int, HashSet<int>> _parentChildren = new Dictionary<int, HashSet<int>>();

        private readonly Dictionary<int, HashSet<int>> _childParents = new Dictionary<int, HashSet<int>>();

        public BoardService(DocumentRepository repository)
        {
            _document = repository.Document;
            _repository = repository;
            UpdateLinks();
        }

        public void Submit()
        {
            _repository.Submit();
        }

        /// <summary>
        /// Refactoring
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parents"></param>
        public void SetParents(int id, IEnumerable<int> parents)
        {
            foreach (var issueLink in _document.IssuesLinks.Where(l => l.ChildId == id).ToList())
            {
                _document.IssuesLinks.Remove(issueLink);
            }
            foreach (var parent in parents)
            {
                _document.IssuesLinks.Add(new IssueLink { ChildId = id, ParentId = parent });
            }
            UpdateLinks();
        }

        public IEnumerable<Issue> GetParents(int id)
        {
            return _document.Issues.Values.Where(i => _parentChildren.ContainsKey(i.Id) && _parentChildren[i.Id].Contains(id));
        }

        public IEnumerable<Issue> GetChildren(int id)
        {
            return _document.Issues.Values.Where(i => _childParents.ContainsKey(i.Id) && _childParents[i.Id].Contains(id));
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
            SetChildren(id, allChildren);
            return allChildren.Select(child => _document.Issues[child]);
        }

        private void SetChildren(int id, HashSet<int> allChildren)
        {
            if (_parentChildren.ContainsKey(id))
            {
                foreach (var child in _parentChildren[id])
                {
                    allChildren.Add(child);
                    SetChildren(child, allChildren);
                }
            }
        }

        public IEnumerable<Issue> GetIssues()
        {
            return _document.Issues.Values;
        }

        public IEnumerable<Issue> GetIssues(IssueFilter filter)
        {
            return _document.Issues.Values.Where(
                i => filter.Types.Contains(i.Type) &&
                    filter.Statuses.Contains(i.Status) &&
                    filter.Priorities.Contains(i.Priority));
        }

        public IEnumerable<Issue> GetRootIssues()
        {
            return _document.Issues.Values.Where(i => !_childParents.ContainsKey(i.Id));
        }

        public IEnumerable<Issue> GetCustomRootIssues()
        {
            return _document.Issues.Values.Where(i => i.IsCustomRoot);
        }

        public IEnumerable<Issue> GetIssuesExeptChildren(int id, IssueFilter filter)
        {
            var allChildren = new HashSet<int>();
            SetChildren(id, allChildren);
            return GetIssues(filter).Where(i => i.Id != id && !allChildren.Contains(i.Id));
        }

        public void DeleteIssue(Issue issue)
        {
            _document.Issues.Remove(issue.Id);
            foreach (var projectIssue in _document.IssuesLinks.Where(pi => pi.ChildId == issue.Id || pi.ParentId == issue.Id).ToList())
            {
                _document.IssuesLinks.Remove(projectIssue);
            }
            UpdateLinks();
        }

        private void UpdateLinks()
        {
            _parentChildren.Clear();
            _childParents.Clear();
            foreach (var projectIssue in _document.IssuesLinks)
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
            return _repository.CreateIssue(type, priority, summary, description, isCustomRoot, httpLink);
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

        public IssueFilter GetInProgressFilter()
        {
            var filter = new IssueFilter();
            filter.Priorities = new HashSet<int>(GetPriorities());
            filter.Statuses = new HashSet<IssueStatus>(new[] {IssueStatus.InProgress});
            filter.Types = new HashSet<IssueType>(GetTypes());
            return filter;
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

        public IssueFilter GetFilter(IssueType type)
        {
            var filter = new IssueFilter();
            if (type == IssueType.Epic)
            {
                filter.Types = new HashSet<IssueType>(new[] { IssueType.Epic });
            }
            else if (type == IssueType.Story)
            {
                filter.Types = new HashSet<IssueType>(new[] { IssueType.Story, IssueType.Epic });
            }
            else 
            {
                filter.Types = new HashSet<IssueType>(new[] { IssueType.Task, IssueType.Story, IssueType.Epic });
            }
            filter.Priorities = new HashSet<int>(GetPriorities());
            filter.Statuses = new HashSet<IssueStatus>(GetStatuses());
            return filter;
        }
    }
}