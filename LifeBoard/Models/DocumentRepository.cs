using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LifeBoard.Models
{
    public class DocumentRepository
    {
        private readonly Document _document = new Document();

        public string DocumentPath
        {
            get { return _documentPath; }
            private set
            {
                if (_documentPath != value)
                {
                    _documentPath = value;
                    if (Path.IsPathRooted(value))
                    {
                        DocumentFolder = Path.GetDirectoryName(value);
                    }
                }
            }
        }

        public Document Document
        {
            get { return _document; }
        }

        public bool IsFileExists
        {
            get { return File.Exists(DocumentPath); }
        }

        public bool IsPathRooted
        {
            get { return Path.IsPathRooted(DocumentPath); }
        }

        public void DeleteIssue(Issue issue)
        {
            Document.Issues.Remove(issue.Id);
            foreach (var projectIssue in Document.IssuesLinks.Where(pi => pi.ChildId == issue.Id || pi.ParentId == issue.Id).ToList())
            {
                Document.IssuesLinks.Remove(projectIssue);
            }
            DeleteAttachments(issue.Id);
        }

        public void SetParents(int id, IEnumerable<int> parents)
        {
            foreach (var issueLink in Document.IssuesLinks.Where(l => l.ChildId == id).ToList())
            {
                Document.IssuesLinks.Remove(issueLink);
            }
            foreach (var parent in parents)
            {
                Document.IssuesLinks.Add(new IssueLink { ChildId = id, ParentId = parent });
            }
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
                WebSite = httpLink,
                CreationDate = DateTime.Now
            });
            return id;
        }

        public bool Open(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                return false;
            }
            FileStream fs = null;
            try
            {
                var serializer = new XmlSerializer(typeof(XMLDocuments.V1.Document));
                fs = new FileStream(path, FileMode.Open);
                SetDocument((XMLDocuments.V1.Document)serializer.Deserialize(fs));
                DocumentPath = path;
                return true;
            }
            catch { }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return false;
        }

        public void New()
        {
            SetDocument(CreateDocoment());
            DocumentPath = String.Empty;
        }

        private void ClearBackups()
        {
            int backupMax = Global.BackupMaxCount;
            var files = new List<string>(Directory.GetFiles(Global.BackupFolder));
            files.Sort();
            for (int i = 0; i < files.Count - backupMax; i++)
            {
                File.Delete(files[i]);
            }
        }

        public bool Save(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                return false;
            }
            if (File.Exists(path))
            {
                try
                {
                    ClearBackups();
                    File.Move(path, Global.NewBackupFile);
                }
                catch { }
            }
            bool isSave = false;
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(XMLDocuments.V1.Document));
                writer = new StreamWriter(path);
                serializer.Serialize(writer, GetDocoment());
                DocumentPath = path;
                isSave = true;
            }
            catch { }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
            return isSave;
        }

        public void Submit()
        {
            if (!IsFileExists)
            {
                return;
            }
            Save(DocumentPath);
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
                WebSite = i.WebSite,
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
                    WebSite = i.WebSite,
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


        public static readonly string AttachmentsFolder = "Attachments";

        private string _documentFolder;
        private string _documentPath;

        public string DocumentFolder
        {
            get { return _documentFolder; }
            set
            {
                if (_documentFolder != value)
                {
                    _documentFolder = value;
                    AttachmentsPath = String.Format("{0}\\{1}", DocumentFolder, AttachmentsFolder);
                }
            }
        }

        public string AttachmentsPath { get; private set; }

        public void ClearDirectory()
        {
            foreach (var directory in Directory.GetDirectories(AttachmentsPath))
            {
                if (Directory.GetFiles(directory).Length == 0)
                {
                    try
                    {
                        Directory.Delete(directory);
                    }
                    catch { }
                }
            }
        }

        public IEnumerable<string> GetAttachments(int documentId)
        {
            string dir = String.Format("{0}\\{1}", AttachmentsPath, documentId);
            if (Directory.Exists(dir))
            {
                return Directory.GetFiles(dir);
            }
            return new string[0];
        }

        public bool UpdateAttachments(int documentId, List<string> attachments, Dictionary<string, string> filePaths)
        {
            try
            {
                string dir = String.Format("{0}\\{1}", AttachmentsPath, documentId);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                foreach (var filePath in Directory.GetFiles(dir))
                {
                    string file = Path.GetFileName(filePath);
                    if (!attachments.Contains(file))
                    {
                        File.Delete(filePath);
                    }
                }
                foreach (var kvp in filePaths)
                {
                    string filePath = String.Format("{0}\\{1}", dir, kvp.Key);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    File.Copy(kvp.Value, filePath);
                }
                ClearDirectory();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool OpenAttachment(int documentId, string fileName)
        {
            try
            {
                string filePath = String.Format("{0}\\{1}\\{2}", AttachmentsPath, documentId, fileName);
                Process.Start(filePath);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void DeleteAttachments(int documentId)
        {
            string dir = String.Format("{0}\\{1}", AttachmentsPath, documentId);
            if (Directory.Exists(dir))
            {
                try
                {
                    Directory.Delete(dir, true);

                }
                catch (Exception)
                {
                }
            }

        }
    }
}
