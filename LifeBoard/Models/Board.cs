using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LifeBoard.Models
{
    /// <summary>
    /// Class Board
    /// </summary>
    public class Board
    {
        /// <summary>
        /// The _child parents
        /// </summary>
        private readonly Dictionary<int, List<int>> _childParents = new Dictionary<int, List<int>>();

        /// <summary>
        /// The _document repository
        /// </summary>
        private readonly DocumentRepository _documentRepository = new DocumentRepository();

        /// <summary>
        /// The _parent children
        /// </summary>
        private readonly Dictionary<int, List<int>> _parentChildren = new Dictionary<int, List<int>>();

        /// <summary>
        /// Gets the document path.
        /// </summary>
        /// <value>The document path.</value>
        public string DocumentPath
        {
            get { return _documentRepository.DocumentPath; }
        }

        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>The document.</value>
        public Document Document
        {
            get { return _documentRepository.Document; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is file exists.
        /// </summary>
        /// <value><c>true</c> if this instance is file exists; otherwise, <c>false</c>.</value>
        public bool IsFileExists
        {
            get { return _documentRepository.IsFileExists; }
        }

        /// <summary>
        /// Submits this instance.
        /// </summary>
        public void Submit()
        {
            _documentRepository.Submit();
        }

        /// <summary>
        /// Opens the document.
        /// </summary>
        /// <param name="documentPath">The document path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool OpenDocument(string documentPath)
        {
            bool isOpen = _documentRepository.Open(documentPath);
            if (!isOpen)
            {
                _documentRepository.New();
            }
            UpdateLinks();
            return isOpen;
        }

        /// <summary>
        /// Saves the document.
        /// </summary>
        /// <param name="documentPath">The document path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool SaveDocument(string documentPath)
        {
            return _documentRepository.Save(documentPath);
        }

        /// <summary>
        /// Sets the parents.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="parents">The parents.</param>
        /// <param name="children"> </param>
        public void SetRelations(int id, IEnumerable<int> parents, IEnumerable<int> children)
        {
            _documentRepository.SetRelations(id, parents, children);
            UpdateLinks();
        }

        /// <summary>
        /// Gets the parents.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IEnumerable{Issue}.</returns>
        public IEnumerable<Issue> GetParents(int id)
        {
            if (_childParents.ContainsKey(id))
            {
                return _childParents[id].Select(t => Document.Issues[t]);
            }
            return new List<Issue>();
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IEnumerable{Issue}.</returns>
        public IEnumerable<Issue> GetChildren(int id)
        {
            if (_parentChildren.ContainsKey(id))
            {
                return _parentChildren[id].Select(t => Document.Issues[t]);
            }
            return new List<Issue>();
        }

        /// <summary>
        /// Gets the root children.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IEnumerable{Issue}.</returns>
        public IEnumerable<Issue> GetRootChildren(int id)
        {
            List<Issue> children = GetChildren(id).ToList();
            List<Issue> allChildren = GetAllChildren(id).ToList();
            return children.Where(child => allChildren.All(c => !_childParents[child.Id].Contains(c.Id))).ToList();
        }

        /// <summary>
        /// Gets all children.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IEnumerable{Issue}.</returns>
        public IEnumerable<Issue> GetAllChildren(int id)
        {
            var allChildren = new HashSet<int>();
            AddChildrenTo(id, allChildren);
            return allChildren.Select(child => Document.Issues[child]);
        }

        public IEnumerable<Issue> GetAllParents(int id)
        {
            var allParents = new HashSet<int>();
            AddParentsTo(id, allParents);
            return allParents.Select(parent => Document.Issues[parent]);
        }

        /// <summary>
        /// Adds the children to.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="allChildren">All children.</param>
        private void AddChildrenTo(int id, HashSet<int> allChildren)
        {
            if (_parentChildren.ContainsKey(id))
            {
                foreach (int child in _parentChildren[id])
                {
                    allChildren.Add(child);
                    AddChildrenTo(child, allChildren);
                }
            }
        }

        private void AddParentsTo(int id, HashSet<int> allParents)
        {
            if (_childParents.ContainsKey(id))
            {
                foreach (int parent in _childParents[id])
                {
                    allParents.Add(parent);
                    AddParentsTo(parent, allParents);
                }
            }
        }

        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <returns>IEnumerable{Issue}.</returns>
        public IEnumerable<Issue> GetIssues()
        {
            return Document.Issues.Values;
        }

        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>IEnumerable{Issue}.</returns>
        public IEnumerable<Issue> GetIssues(IssueFilter filter)
        {
            if (filter.HasQuery)
            {
                return Document.Issues.Values.Where(
                    i => ((!filter.HasDeadline) || (filter.HasDeadline && HasDeadline(i, filter.IsActualDeadline))) &&
                        filter.Types.Contains(i.Type) &&
                         filter.Statuses.Contains(i.Status) &&
                         filter.Priorities.Contains(i.Priority) &&
                         Regex.IsMatch(i.Summary.ToLower(), filter.Query.ToLower()));
            }
            return Document.Issues.Values.Where(
                i => ((!filter.HasDeadline) || (filter.HasDeadline && HasDeadline(i, filter.IsActualDeadline))) &&
                    filter.Types.Contains(i.Type) &&
                     filter.Statuses.Contains(i.Status) &&
                     filter.Priorities.Contains(i.Priority));
        }

        private bool HasDeadline(Issue issue, bool isActual)
        {
            if (isActual)
            {
                DateTime date;
                if (!DateTime.TryParse(issue.Deadline, out date))
                {
                    date = DateTime.MinValue;
                }
                return date > DateTime.Now;
            }
            return !String.IsNullOrWhiteSpace(issue.Deadline);
        }

        /// <summary>
        /// Gets the root issues.
        /// </summary>
        /// <returns>IEnumerable{Issue}.</returns>
        public IEnumerable<Issue> GetRootIssues()
        {
            return Document.Issues.Values.Where(i => !_childParents.ContainsKey(i.Id));
        }

        /// <summary>
        /// Gets the custom root issues.
        /// </summary>
        /// <returns>IEnumerable{Issue}.</returns>
        public IEnumerable<Issue> GetCustomRootIssues()
        {
            return Document.Issues.Values.Where(i => i.IsCustomRoot);
        }

        /// <summary>
        /// Gets the issues exept children.
        /// </summary>
        /// <param name="children">The ids.</param>
        /// <param name="query">The query.</param>
        /// <returns>IEnumerable{Issue}.</returns>
        public IEnumerable<Issue> GetIssuesExeptChildren(IEnumerable<int> children, string query)
        {
            var allChildren = new HashSet<int>();
            foreach (var child in children)
            {
                allChildren.Add(child);
                AddChildrenTo(child, allChildren);
            }
            return GetIssues(query).Where(i => !allChildren.Contains(i.Id));
        }

        public IEnumerable<Issue> GetIssuesExeptParents(IEnumerable<int> parents, string query)
        {
            var allParents = new HashSet<int>();
            foreach (var parent in parents)
            {
                allParents.Add(parent);
                AddParentsTo(parent, allParents);
            }
            return GetIssues(query).Where(i => !allParents.Contains(i.Id));
        }

        /// <summary>
        /// Deletes the issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        public void DeleteIssue(Issue issue)
        {
            _documentRepository.DeleteIssue(issue);
            UpdateLinks();
        }

        /// <summary>
        /// Updates the links.
        /// </summary>
        private void UpdateLinks()
        {
            _parentChildren.Clear();
            _childParents.Clear();
            foreach (IssueLink projectIssue in Document.IssuesLinks.OrderBy(il => il.Order))
            {
                if (!_parentChildren.ContainsKey(projectIssue.ParentId))
                {
                    _parentChildren.Add(projectIssue.ParentId, new List<int>());
                }
                _parentChildren[projectIssue.ParentId].Add(projectIssue.ChildId);

                if (!_childParents.ContainsKey(projectIssue.ChildId))
                {
                    _childParents.Add(projectIssue.ChildId, new List<int>());
                }
                _childParents[projectIssue.ChildId].Add(projectIssue.ParentId);
            }
        }

        /// <summary>
        /// Creates the issue.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="description">The description.</param>
        /// <param name="isCustomRoot">if set to <c>true</c> [is custom root].</param>
        /// <param name="deadline"> </param>
        /// <param name="links">The HTTP link.</param>
        /// <returns>System.Int32.</returns>
        public int CreateIssue(IssueType type, int priority, string summary, string description, bool isCustomRoot, string deadline, IEnumerable<string> links)
        {
            return _documentRepository.CreateIssue(type, priority, summary, description, isCustomRoot, deadline, links);
        }

        /// <summary>
        /// Gets the priorities.
        /// </summary>
        /// <returns>IEnumerable{System.Int32}.</returns>
        public IEnumerable<int> GetPriorities()
        {
            return new[] { 1, 2, 3, 4, 5 };
        }

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <returns>IEnumerable{IssueType}.</returns>
        public IEnumerable<IssueType> GetTypes()
        {
            return new[] { IssueType.Note, IssueType.Task, IssueType.Story, IssueType.Epic };
        }

        /// <summary>
        /// Gets the statuses.
        /// </summary>
        /// <returns>IEnumerable{IssueStatus}.</returns>
        public IEnumerable<IssueStatus> GetStatuses()
        {
            return new[] { IssueStatus.Open, IssueStatus.InProgress, IssueStatus.Resolved, IssueStatus.Closed };
        }

        /// <summary>
        /// Gets the default filter.
        /// </summary>
        /// <returns>IssueFilter.</returns>
        public IssueFilter GetDefaultFilter()
        {
            var filter = new IssueFilter();
            filter.Priorities = new HashSet<int>(GetPriorities());
            filter.Statuses =
                new HashSet<IssueStatus>(new[] { IssueStatus.Open, IssueStatus.InProgress, IssueStatus.Resolved });
            filter.Types = new HashSet<IssueType>(GetTypes());
            return filter;
        }

        /// <summary>
        /// Gets the full filter.
        /// </summary>
        /// <returns>IssueFilter.</returns>
        public IssueFilter GetFullFilter()
        {
            var filter = new IssueFilter();
            filter.Priorities = new HashSet<int>(GetPriorities());
            filter.Statuses = new HashSet<IssueStatus>(GetStatuses());
            filter.Types = new HashSet<IssueType>(GetTypes());
            return filter;
        }

        /// <summary>
        /// Updates the attachments.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool UpdateAttachments(int id, List<string> attachments, Dictionary<string, string> filePaths)
        {
            return _documentRepository.UpdateAttachments(id, attachments, filePaths);
        }

        /// <summary>
        /// Opens the attachment.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool OpenAttachment(int id, string fileName)
        {
            return _documentRepository.OpenAttachment(id, fileName);
        }

        /// <summary>
        /// Gets the attachments.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IEnumerable{System.String}.</returns>
        public IEnumerable<string> GetAttachments(int id)
        {
            return _documentRepository.GetAttachments(id).Select(Path.GetFileName);
        }

        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>IEnumerable{Issue}.</returns>
        public IEnumerable<Issue> GetIssues(string query)
        {
            if (String.IsNullOrEmpty(query))
            {
                return Document.Issues.Values;
            }
            return Document.Issues.Values.Where(i => Regex.IsMatch(i.Summary.ToLower(), query.ToLower()));
        }
    }
}