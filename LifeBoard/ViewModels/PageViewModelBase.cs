using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;

namespace LifeBoard.ViewModels
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        private DelegateCommand _navigateCommand;

        protected PageViewModelBase(IFrameViewModel frame)
        {
            Frame = frame;
        }

        public IFrameViewModel Frame { get; private set; }

        public ICommand NavigateCommand
        {
            get { return _navigateCommand ?? (_navigateCommand = new DelegateCommand(Navigate)); }
        }

        public abstract Page Page { get; }

        public bool IsChecked 
        {
            get { return Equals(Frame.Current, this); }
        }

        public virtual void Navigate()
        {
            var old = Frame.Current;
            Frame.Current = this;
            old.OnPropertyChanged("IsChecked");
            OnPropertyChanged("IsChecked");
        }
    }
}