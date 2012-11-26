using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LifeBoard.Models
{
    public class DocumentRepository
    {
        private string _path = "";

        private readonly Document _document = new Document();

        public void SetFilePath(string path)
        {
            _path = path;
        }

        public Document Document
        {
            get { return _document; }
        }

        public bool IsFileExists
        {
            get { return File.Exists(_path); }
        }

        public bool IsPathRooted
        {
            get { return Path.IsPathRooted(_path); }
        }

        public int CreateIssue(IssueType type, int priority, string summary, string description, bool isCustomRoot, string httpLink)
        {
            int id = NewIssueId();
            _document.Issues.Add(id, new Issue
            {
                Id = id,
                Type = type,
                Status = IssueStatus.Open,
                Priority = priority,
                Summary = summary,
                Description = description,
                IsCustomRoot = isCustomRoot,
                WebLink = httpLink,
                CreationDate = DateTime.Now
            });
            return id;
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
            if (!IsPathRooted)
            {
                return;
            }
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

        public void Submit()
        {
            if (!IsFileExists)
            {
                return;
            }
            Save();
        }

        private void SetDocument(XMLDocuments.V1.Document document)
        {
            _document.Issues = document.Issues.Select(i => new Issue
            {
                Id = i.Id,
                Priority = i.Priority,
                Summary = i.Summary,
                Description = i.Description,
                Type = i.Type,
                Status = i.Status,
                CreationDate = i.CreationDate,
                WebLink = i.WebLink,
                IsCustomRoot = i.IsCustomRoot
            }).ToDictionary(i => i.Id);
            _document.IssuesLinks = document.IssuesLinks.Select(pi => new IssueLink
            {
                ChildId = pi.ParentId,
                ParentId = pi.ChildId
            }).ToList();

        }

        private XMLDocuments.V1.Document GetDocoment()
        {
            return new XMLDocuments.V1.Document
            {
                Issues = _document.Issues.Values.Select(i => new XMLDocuments.V1.Issue
                {
                    Id = i.Id,
                    Priority = i.Priority,
                    Summary = i.Summary,
                    Description = i.Description,
                    Type = i.Type,
                    Status = i.Status,
                    CreationDate = i.CreationDate,
                    WebLink = i.WebLink,
                    IsCustomRoot = i.IsCustomRoot
                }).ToArray(),
                IssuesLinks = _document.IssuesLinks.Select(pi => new XMLDocuments.V1.IssueLinks
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

        private int NewIssueId()
        {
            int id = 1;
            while (true)
            {
                if (!_document.Issues.ContainsKey(id))
                {
                    break;
                }
                ++id;
            }
            return id;
        }
    }
}
