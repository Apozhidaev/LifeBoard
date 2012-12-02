using System.Windows.Controls;

namespace LifeBoard.ViewModels
{
    /// <summary>
    /// Class PageViewModelBase
    /// </summary>
    public abstract class PageViewModelBase : ParentViewModelBase
    {
        /// <summary>
        /// The _is navigated
        /// </summary>
        private bool _isNavigated;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewModelBase" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        protected PageViewModelBase(object parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is navigated.
        /// </summary>
        /// <value><c>true</c> if this instance is navigated; otherwise, <c>false</c>.</value>
        public bool IsNavigated
        {
            get { return _isNavigated; }
            set
            {
                if (_isNavigated != value)
                {
                    _isNavigated = value;
                    OnPropertyChanged("IsNavigated");
                    if (_isNavigated)
                    {
                        OnNavigated();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <value>The page.</value>
        public abstract Page Page { get; }

        /// <summary>
        /// Called when [navigated].
        /// </summary>
        protected virtual void OnNavigated()
        {
        }
    }
}