using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;

namespace LifeBoard.ViewModels
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        private DelegateCommand _navigateCommand;

        protected PageViewModelBase(IFramePageViewModel framePage)
        {
            FramePage = framePage;
        }

        public IFramePageViewModel FramePage { get; private set; }

        public ICommand NavigateCommand
        {
            get { return _navigateCommand ?? (_navigateCommand = new DelegateCommand(Navigate)); }
        }

        public abstract Page Page { get; }

        public bool IsChecked 
        {
            get { return Equals(FramePage.Current, this); }
        }

        public virtual void Navigate()
        {
            var old = FramePage.Current;
            FramePage.Current = this;
            old.OnPropertyChanged("IsChecked");
            OnPropertyChanged("IsChecked");
        }
    }
}