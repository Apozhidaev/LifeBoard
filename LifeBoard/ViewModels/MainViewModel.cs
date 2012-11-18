using LifeBoard.Models;
using LifeBoard.ViewModels.Configuration;
using LifeBoard.ViewModels.Dashboard;
using LifeBoard.ViewModels.Issues;

namespace LifeBoard.ViewModels
{
    public class MainViewModel : ViewModelBase, IFrameViewModel
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
                    _current = value;
                    OnPropertyChanged("Current");
                }
            }
        }

        public DashboardViewModel DashboardPage { get; set; }

        public MainIssuesViewModel IssuesPage { get; set; }

        public ConfigurationViewModel ConfigurationPage { get; set; }
    }
}