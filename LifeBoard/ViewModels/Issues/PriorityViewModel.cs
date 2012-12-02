using System.Globalization;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class PriorityViewModel
    /// </summary>
    public class PriorityViewModel : ParentViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        public PriorityViewModel(object parent, int priority, bool isChecked)
            : base(parent)
        {
            Priority = priority;
            IsChecked = isChecked;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return Priority.ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public int Priority { get; private set; }
    }
}