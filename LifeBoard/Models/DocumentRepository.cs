using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using LifeBoard.Models.XMLDocuments.V1;

namespace LifeBoard.Models
{
    /// <summary>
    /// Class DocumentRepository
    /// </summary>
    public class DocumentRepository
    {
        /// <summary>
        /// The attachments folder
        /// </summary>
        public static readonly string AttachmentsFolder = "Attachments";
        /// <summary>
        /// The _document
        /// </summary>
        private readonly Document _document = new Document();
        /// <summary>
        /// The _document folder
        /// </summary>
        private string _documentFolder;
        /// <summary>
        /// The _document path
        /// </summary>
        private string _documentPath;

        /// <summary>
        /// Gets the document path.
        /// </summary>
        /// <value>The document path.</value>
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

        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>The document.</value>
        public Document Document
        {
            get { return _document; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is file exists.
        /// </summary>
        /// <value><c>true</c> if this instance is file exists; otherwise, <c>false</c>.</value>
        public bool IsFileExists
        {
            get { return File.Exists(DocumentPath); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is path rooted.
        /// </summary>
        /// <value><c>true</c> if this instance is path rooted; otherwise, <c>false</c>.</value>
        public bool IsPathRooted
        {
            get { return Path.IsPathRooted(DocumentPath); }
        }

        /// <summary>
        /// Gets or sets the document folder.
        /// </summary>
        /// <value>The document folder.</value>
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

        /// <summary>
        /// Gets the attachments path.
        /// </summary>
        /// <value>The attachments path.</value>
        public string AttachmentsPath { get; private set; }

        /// <summary>
        /// Deletes the issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        public void DeleteIssue(Issue issue)
        {
            Document.Issues.Remove(issue.Id);
            foreach (
                IssueLink projectIssue in
                    Document.IssuesLinks.Where(pi => pi.ChildId == issue.Id || pi.ParentId == issue.Id).ToList())
            {
                Document.IssuesLinks.Remove(projectIssue);
            }
            DeleteAttachments(issue.Id);
        }

        /// <summary>
        /// Sets the parents.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="parents">The parents.</param>
        public void SetParents(int id, IEnumerable<int> parents)
        {
            foreach (IssueLink issueLink in Document.IssuesLinks.Where(l => l.ChildId == id).ToList())
            {
                Document.IssuesLinks.Remove(issueLink);
            }
            foreach (int parent in parents)
            {
                Document.IssuesLinks.Add(new IssueLink { ChildId = id, ParentId = parent });
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
                                             Deadline = deadline,
                                             CreationDate = DateTime.Now,
                                             Links = links.ToList()
                                         });
            return id;
        }

        /// <summary>
        /// Opens the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
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
            catch
            {
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return false;
        }

        /// <summary>
        /// News this instance.
        /// </summary>
        public void New()
        {
            SetDocument(CreateDocoment());
            DocumentPath = String.Empty;
        }

        /// <summary>
        /// Clears the backups.
        /// </summary>
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

        /// <summary>
        /// Saves the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
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
                catch
                {
                }
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
            catch
            {
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
            return isSave;
        }

        /// <summary>
        /// Submits this instance.
        /// </summary>
        public void Submit()
        {
            if (!IsFileExists)
            {
                return;
            }
            Save(DocumentPath);
        }

        /// <summary>
        /// Sets the document.
        /// </summary>
        /// <param name="document">The document.</param>
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
                                                                   Deadline = i.Deadline,
                                                                   Links = i.Links != null ? i.Links.ToList() : new List<string>(),
                                                                   CreationDate = i.CreationDate,
                                                                   IsCustomRoot = i.IsCustomRoot
                                                               }).ToDictionary(i => i.Id);
            foreach (var issue in document.Issues)
            {
                if (!String.IsNullOrEmpty(issue.WebSite))
                {
                    _document.Issues[issue.Id].Links.Add(issue.WebSite);
                }
            }
            _document.IssuesLinks = document.IssuesLinks.Select(pi => new IssueLink
                                                                          {
                                                                              ChildId = pi.ParentId,
                                                                              ParentId = pi.ChildId
                                                                          }).ToList();
        }

        /// <summary>
        /// Gets the docoment.
        /// </summary>
        /// <returns>XMLDocuments.V1.Document.</returns>
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
                                                                                Deadline = i.Deadline,
                                                                                CreationDate = i.CreationDate,
                                                                                Links = i.Links.ToArray(),
                                                                                IsCustomRoot = i.IsCustomRoot
                                                                            }).ToArray(),
                           IssuesLinks = _document.IssuesLinks.Select(pi => new IssueLinks
                                                                                {
                                                                                    ParentId = pi.ChildId,
                                                                                    ChildId = pi.ParentId
                                                                                }).ToArray()
                       };
        }

        /// <summary>
        /// Creates the docoment.
        /// </summary>
        /// <returns>XMLDocuments.V1.Document.</returns>
        private XMLDocuments.V1.Document CreateDocoment()
        {
            return new XMLDocuments.V1.Document
                       {
                           Issues = new[]
                            {
                                new XMLDocuments.V1.Issue
                                {
                                    Id = 1,
                                    Type = IssueType.Epic,
                                    Status = IssueStatus.Open,
                                    CreationDate = DateTime.Now,
                                    Summary = "Развитие",
                                    Description = "Все что касается моего развития.",
                                    Priority = 1,
                                    IsCustomRoot = true
                                },
                                new XMLDocuments.V1.Issue
                                {
                                    Id = 2,
                                    Type = IssueType.Epic,
                                    Status = IssueStatus.Open,
                                    CreationDate = DateTime.Now,
                                    Summary = "Карьера",
                                    Description = "Все что связано с работой.",
                                    Priority = 1,
                                    IsCustomRoot = true
                                },
                                new XMLDocuments.V1.Issue
                                {
                                    Id = 2,
                                    Type = IssueType.Epic,
                                    Status = IssueStatus.Open,
                                    CreationDate = DateTime.Now,
                                    Summary = "Личная жизнь",
                                    Description = "Отношения, семья.",
                                    Priority = 1,
                                    IsCustomRoot = true
                                }
                            },
                           IssuesLinks = new IssueLinks[0]
                       };
        }

        /// <summary>
        /// News the issue id.
        /// </summary>
        /// <returns>System.Int32.</returns>
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


        /// <summary>
        /// Clears the directory.
        /// </summary>
        public void ClearDirectory()
        {
            foreach (string directory in Directory.GetDirectories(AttachmentsPath))
            {
                if (Directory.GetFiles(directory).Length == 0)
                {
                    try
                    {
                        Directory.Delete(directory);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Gets the attachments.
        /// </summary>
        /// <param name="documentId">The document id.</param>
        /// <returns>IEnumerable{System.String}.</returns>
        public IEnumerable<string> GetAttachments(int documentId)
        {
            string dir = String.Format("{0}\\{1}", AttachmentsPath, documentId);
            if (Directory.Exists(dir))
            {
                return Directory.GetFiles(dir);
            }
            return new string[0];
        }

        /// <summary>
        /// Updates the attachments.
        /// </summary>
        /// <param name="documentId">The document id.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool UpdateAttachments(int documentId, List<string> attachments, Dictionary<string, string> filePaths)
        {
            if (!IsFileExists)
            {
                return attachments.Count == 0;
            }
            try
            {
                string dir = String.Format("{0}\\{1}", AttachmentsPath, documentId);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                foreach (string filePath in Directory.GetFiles(dir))
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

        /// <summary>
        /// Opens the attachment.
        /// </summary>
        /// <param name="documentId">The document id.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
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

        /// <summary>
        /// Deletes the attachments.
        /// </summary>
        /// <param name="documentId">The document id.</param>
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