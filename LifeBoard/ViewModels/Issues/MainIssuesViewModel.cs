using System.Windows.Controls;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class MainIssuesViewModel : PageViewModelBase, IFramePageViewModel
    {
        private MainIssuesView _mainIssuesView;

        private PageViewModelBase _current;

        public MainIssuesViewModel(IFramePageViewModel parent, BoardService boardService)
            : base(parent)
        {
            _current = new IssuesViewModel(this, boardService);
        }

        public override Page Page
        {
            get { return _mainIssuesView ?? (_mainIssuesView = new MainIssuesView(this)); }
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
    }
}
