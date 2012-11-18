using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace LifeBoard.Models
{
    public class BoardService
    {
        private string _path = "";

        private Dictionary<int, Issue> _issues;

        private List<IssueLink> _issueLinks;

        private readonly Dictionary<int, HashSet<int>> _parentChildren = new Dictionary<int, HashSet<int>>();

        private readonly Dictionary<int, HashSet<int>> _childParents = new Dictionary<int, HashSet<int>>();

        /// <summary>
        /// Refactoring
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parents"></param>
        public void SetParents(int id, IEnumerable<int> parents)
        {
            foreach (var issueLink in _issueLinks.Where(l => l.ChildId == id).ToList())
            {
                _issueLinks.Remove(issueLink);
            }
            foreach (var parent in parents)
            {
                _issueLinks.Add(new IssueLink {ChildId = id, ParentId = parent});
            }
            UpdateLinks();
        }

        public void SetFilePath(string path)
        {
            _path = path;
        }

        public bool IsFileExists
        {
            get { return File.Exists(_path); }
        }

        public IEnumerable<Issue> GetParents(int id)
        {
            return _issues.Values.Where(i => _parentChildren.ContainsKey(i.Id) && _parentChildren[i.Id].Contains(id));
        }

        public IEnumerable<Issue> GetChildren(int id)
        {
            return _issues.Values.Where(i => _childParents.ContainsKey(i.Id) && _childParents[i.Id].Contains(id));
        }

        public IEnumerable<Issue> GetAllChildren(int id)
        {
            var allChildren = new HashSet<int>();
            SetChildren(id, allChildren);
            return allChildren.Select(child => _issues[child]);
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
            return _issues.Values;
        }

        public IEnumerable<Issue> GetIssues(IssueFilter filter)
        {
            return _issues.Values.Where(
                i => filter.Types.Contains(i.Type) &&
                    filter.Statuses.Contains(i.Status) &&
                    filter.Priorities.Contains(i.Priority));
        }

        public IEnumerable<Issue> GetIssuesExeptChildren(int id, IssueFilter filter)
        {
            var allChildren = new HashSet<int>();
            SetChildren(id, allChildren);
            return GetIssues(filter).Where(i => i.Id != id && !allChildren.Contains(i.Id));
        }

        public void Open()
        {
            FileStream fs = null;
            try
            {
                var serializer = new XmlSerializer(typeof(XMLDocuments.V1.Document));
                fs = new FileStream(_path, FileMode.Open);
                SetDocument((XMLDocuments.V1.Document)serializer.Deserialize(fs));
            }
            catch
            {
                SetDocument(CreateDocoment());
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        public void Save()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(XMLDocuments.V1.Document));
                writer = new StreamWriter(_path);
                serializer.Serialize(writer, GetDocoment());
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        public void DeleteIssue(Issue issue)
        {
            _issues.Remove(issue.Id);
            foreach (var projectIssue in _issueLinks.Where(pi => pi.ChildId == issue.Id || pi.ParentId == issue.Id).ToList())
            {
                _issueLinks.Remove(projectIssue);
            }
            UpdateLinks();
        }

        private int NewIssueId()
        {
            int id = 1;
            while (true)
            {
                if (!_issues.ContainsKey(id))
                {
                    break;
                }
                ++id;
            }
            return id;
        }

        private void SetDocument(XMLDocuments.V1.Document document)
        {
            _issues = document.Issues.Select(i => new Issue
                                                      {
                                                          Id = i.Id,
                                                          Priority = i.Priority,
                                                          Summary = i.Summary,
                                                          Description = i.Description,
                                                          Type = i.Type,
                                                          Status = i.Status
                                                      }).ToDictionary(i => i.Id);
            _issueLinks = document.IssuesLinks.Select(pi => new IssueLink
                                                                     {
                                                                         ChildId = pi.ParentId,
                                                                         ParentId = pi.ChildId
                                                                     }).ToList();

            UpdateLinks();

        }

        private void UpdateLinks()
        {
            _parentChildren.Clear();
            _childParents.Clear();
            foreach (var projectIssue in _issueLinks)
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

        private XMLDocuments.V1.Document GetDocoment()
        {
            return new XMLDocuments.V1.Document
                       {
                           Issues = _issues.Values.Select(i => new XMLDocuments.V1.Issue
                                                            {
                                                                Id = i.Id,
                                                                Priority = i.Priority,
                                                                Summary = i.Summary,
                                                                Description = i.Description,
                                                                Type = i.Type,
                                                                Status = i.Status
                                                            }).ToArray(),
                           IssuesLinks = _issueLinks.Select(pi => new XMLDocuments.V1.IssueLinks
                                                                           {
                                                                               ParentId = pi.ChildId,
                                                                               ChildId = pi.ParentId
                                                                           }).ToArray()
                       };
        }

        private XMLDocuments.V1.Document CreateDocoment()
        {
            return new XMLDocuments.V1.Document
                       {
                           Issues = new XMLDocuments.V1.Issue[0],
                           IssuesLinks = new XMLDocuments.V1.IssueLinks[0]
                       };
        }

        public int CreateIssue(IssueType type, int priority, string summary, string description)
        {
            int id = NewIssueId();
            _issues.Add(id,new Issue
            {
                Id = id,
                Type = type,
                Status = IssueStatus.Open,
                Priority = priority,
                Summary = summary,
                Description = description
            });
            return id;
        }

        public IEnumerable<int> GetPriorities()
        {
            return new[] { 1, 2, 3, 4, 5 };
        }

        public IEnumerable<IssueType> GetTypes()
        {
            return new[] { IssueType.Task, IssueType.Story, IssueType.Epic };
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
                filter.Types = new HashSet<IssueType>(GetTypes());
            }
            filter.Priorities = new HashSet<int>(GetPriorities());
            filter.Statuses = new HashSet<IssueStatus>(GetStatuses());
            return filter;
        }
    }
}