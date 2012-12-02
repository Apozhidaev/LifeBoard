using System.Globalization;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class PageNumberViewModel
    /// </summary>
    public class PageNumberViewModel : ParentViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageNumberViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="number">The number.</param>
        public PageNumberViewModel(object parent, int number) : base(parent)
        {
            Number = number;
        }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        public int Number { get; set; }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header
        {
            get { return Number.ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        protected bool Equals(PageNumberViewModel other)
        {
            return Equals(Number, other.Number);
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
            return Equals((PageNumberViewModel) obj);
        }
    }
}