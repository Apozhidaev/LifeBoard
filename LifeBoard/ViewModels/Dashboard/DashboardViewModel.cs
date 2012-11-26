using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;
using LifeBoard.Views.Dashboard;

namespace LifeBoard.ViewModels.Dashboard
{
    public class DashboardViewModel : PageViewModelBase
    {
        private bool _isRoot;

        private bool _isCustomRoot;

        private readonly BoardService _boardService;

        private DashboardView _dashboardView;

        public DashboardViewModel(object parent, BoardService boardService) 
            : base(parent)
        {
            _boardService = boardService;
            Issues = new ObservableCollection<IssueViewModel>();
            _isCustomRoot = true;
        }

        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        public override Page Page
        {
            get { return _dashboardView ?? (_dashboardView = new DashboardView(this)); }
        }

        protected override void OnNavigated()
        {
            UpdateIssues();
            base.OnNavigated();
        }

        private void UpdateIssues()
        {
            var issues = IsCustomRoot ? _boardService.GetCustomRootIssues() : _boardService.GetRootIssues();
            Issues.Clear();
            foreach (var issue in issues)
            {
                Issues.Add(new IssueViewModel(this, issue));
            }
        }

        private DelegateCommand _customRrootCommand;

        public ICommand CustomRootCommand
        {
            get { return _customRrootCommand ?? (_customRrootCommand = new DelegateCommand(CustomRoot)); }
        }

        private void CustomRoot()
        {
            GoRoot(true);
        }

        private DelegateCommand _rootCommand;

        public ICommand RootCommand
        {
            get { return _rootCommand ?? (_rootCommand = new DelegateCommand(Root)); }
        }

        private void Root()
        {
            GoRoot(false);
        }

        private void GoRoot(bool isCustom)
        {
            IsRoot = !isCustom;
            IsCustomRoot = isCustom;
            UpdateIssues();
        }

        public bool IsRoot
        {
            get { return _isRoot; }
            set
            {
                if (_isRoot != value)
                {
                    _isRoot = value;
                    OnPropertyChanged("IsRoot");
                }
            }
        }

        public bool IsCustomRoot
        {
            get { return _isCustomRoot; }
            set
            {
                if (_isCustomRoot != value)
                {
                    _isCustomRoot = value;
                    OnPropertyChanged("IsCustomRoot");
                }
            }
        }
    }
}