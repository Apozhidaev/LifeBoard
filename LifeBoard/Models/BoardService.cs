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

        private List<Issue> _issues;

        private List<IssueLink> _issueLinks;

        private readonly Dictionary<int, HashSet<int>> _parentChildren = new Dictionary<int, HashSet<int>>();

        private readonly Dictionary<int, HashSet<int>> _childParents = new Dictionary<int, HashSet<int>>();

        public void SetFilePath(string path)
        {
            _path = path;
        }

        public bool IsFileExists
        {
            get { return File.Exists(_path); }
        }

        public IEnumerable<Issue> GetIssues()
        {
            return _issues;
        }

        public IEnumerable<Issue> GetIssues(IssueFilter filter)
        {
            return _issues.Where(
                i => filter.Types.Contains(i.Type) &&
                    filter.Statuses.Contains(i.Status) &&
                    filter.Priorities.Contains(i.Priority));
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
            _issues.Remove(issue);
            foreach (var projectIssue in _issueLinks.Where(pi => pi.ChildId == issue.Id || pi.ParentId == issue.Id))
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
                if (_issues.All(issue => issue.Id != id))
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
                                                      }).ToList();
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
                           Issues = _issues.Select(i => new XMLDocuments.V1.Issue
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

        public void CreateIssue(IssueType type, int priority, string summary, string description)
        {
            _issues.Add(new Issue
            {
                Id = NewIssueId(),
                Type = type,
                Status = IssueStatus.Open,
                Priority = priority,
                Summary = summary,
                Description = description
            });
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
    }
}