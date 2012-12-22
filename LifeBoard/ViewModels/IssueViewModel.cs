using System;
using System.Windows;
using LifeBoard.Models;

namespace LifeBoard.ViewModels
{
    /// <summary>
    /// Class IssueViewModel
    /// </summary>
    public class IssueViewModel : ViewModelBase
    {
        /// <summary>
        /// The _issue
        /// </summary>
        private readonly Issue _issue;

        /// <summary>
        /// The _parent
        /// </summary>
        private readonly object _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="IssueViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="issue">The issue.</param>
        public IssueViewModel(object parent, Issue issue)
        {
            _parent = parent;
            _issue = issue;
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

        public string Deadline
        {
            get { return _issue.Deadline; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is custom root.public string Deadline { get; set; }
        /// </summary>
        /// <value><c>true</c> if this instance is custom root; otherwise, <c>false</c>.</value>
        public bool IsCustomRoot
        {
            get { return _issue.IsCustomRoot; }
            set
            {
                if (_issue.IsCustomRoot != value)
                {
                    _issue.IsCustomRoot = value;
                    OnPropertyChanged("IsCustomRoot");
                }
            }
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
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public IssueStatus Status
        {
            get { return _issue.Status; }
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
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public Issue Model
        {
            get { return _issue; }
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public object Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        protected bool Equals(IssueViewModel other)
        {
            return Equals(_issue, other._issue);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return (_issue != null ? _issue.GetHashCode() : 0);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">Объект, который требуется сравнить с текущим объектом.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IssueViewModel) obj);
        }

        /// <summary>
        /// Updates the source.
        /// </summary>
        public void UpdateSource()
        {
            OnPropertyChanged("Summary");
            OnPropertyChanged("Description");
            OnPropertyChanged("Deadline");
            OnPropertyChanged("Priority");
            OnPropertyChanged("IssueType");
            OnPropertyChanged("Status");
            OnPropertyChanged("CreationDate");
            OnPropertyChanged("IsCustomRoot");
        }
    }
}