using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;

namespace LifeBoard.ViewModels
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        private DelegateCommand _navigateCommand;

        protected PageViewModelBase(IFrameViewModel parent)
        {
            Parent = parent;
        }

        public IFrameViewModel Parent { get; private set; }

        public ICommand NavigateCommand
        {
            get { return _navigateCommand ?? (_navigateCommand = new DelegateCommand(Navigate)); }
        }

        public abstract Page Page { get; }

        public bool IsChecked 
        {
            get { return Equals(Parent.Current, this); }
        }

        public virtual void Navigate()
        {
            var old = Parent.Current;
            Parent.Current = this;
            old.OnPropertyChanged("IsChecked");
            OnPropertyChanged("IsChecked");
        }
    }
}