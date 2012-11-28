using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Issues;

namespace LifeBoard.ViewModels.Issues
{
    public class IssuesViewModel : PageViewModelBase
    {
        private readonly Board _board;

        private IssuesView _issuesView;

        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        public FilterViewModel Filter { get; private set; }

        public IssuesViewModel(object parent, Board board)
            : base(parent)
        {
            _board = board;
            Filter = new FilterViewModel(this);
            Issues = new ObservableCollection<IssueViewModel>();
            Filter.SetModel(board.GetFullFilter(), board.GetDefaultFilter());
            Search();
        }

        private DelegateCommand _searchCommand;

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new DelegateCommand(Search)); }
        }

        public async void Search()
        {
            bool isClear = false;
            foreach (var issue in await Task<IEnumerable<Issue>>.Factory.StartNew(() => _board.GetIssues(Filter.ToModel())))
            {
                if(!isClear)
                {
                    Issues.Clear();
                    isClear = true;
                }
                Issues.Add(new IssueViewModel(this, issue));
            }
            if (!isClear)
            {
                Issues.Clear();
            }
        }

        public override Page Page
        {
            get { return _issuesView ?? (_issuesView = new IssuesView(this)); }
        }

        protected override void OnNavigated()
        {
            base.OnNavigated();
            Search();
        }
    }
}