using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.ViewModels.Configuration;
using LifeBoard.ViewModels.Dashboard;
using LifeBoard.ViewModels.Issues;

namespace LifeBoard.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private PageViewModelBase _current;

        public MainViewModel(BoardService boardService)
        {
            DashboardPage = new DashboardViewModel(this, boardService);
            IssuesPage = new MainIssuesViewModel(this, boardService);
            ConfigurationPage = new ConfigurationViewModel(this, boardService);
            _current = DashboardPage;
        }

        public PageViewModelBase Current
        {
            get { return _current; }
            set
            {
                if (!Equals(_current, value))
                {
                    _current.IsNavigated = false;
                    _current = value;
                    _current.IsNavigated = true;
                    OnPropertyChanged("Current");
                }
            }
        }

        public DashboardViewModel DashboardPage { get; private set; }

        public MainIssuesViewModel IssuesPage { get; private set; }

        public ConfigurationViewModel ConfigurationPage { get; private set; }

        private DelegateCommand<PageViewModelBase> _navigateCommand;

        public ICommand NavigateCommand
        {
            get { return _navigateCommand ?? (_navigateCommand = new DelegateCommand<PageViewModelBase>(Navigate)); }
        }

        public virtual void Navigate(PageViewModelBase pageViewModel)
        {
            Current = pageViewModel;
        }
    }
}