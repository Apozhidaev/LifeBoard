using System.Windows.Controls;

namespace LifeBoard.ViewModels
{
    public abstract class PageViewModelBase : ParentViewModelBase
    {
        private bool _isNavigated;

        protected PageViewModelBase(object parent) 
            : base(parent)
        {
        }

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

        public abstract Page Page { get; }

        protected virtual void OnNavigated(){ }
    }
}