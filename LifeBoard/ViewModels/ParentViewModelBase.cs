namespace LifeBoard.ViewModels
{
    /// <summary>
    /// Class ParentViewModelBase
    /// </summary>
    public abstract class ParentViewModelBase : ViewModelBase
    {
        /// <summary>
        /// The _parent
        /// </summary>
        private readonly object _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentViewModelBase" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        protected ParentViewModelBase(object parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public object Parent
        {
            get { return _parent; }
        }
    }
}