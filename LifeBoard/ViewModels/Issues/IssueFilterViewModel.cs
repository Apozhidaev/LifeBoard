using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class IssueFilterViewModel
    {
        private readonly IssuesViewModel _service;
        private readonly BoardService _boardService;

        public IssueFilterViewModel(IssuesViewModel service, BoardService boardService)
        {
            _service = service;
            _boardService = boardService;
            Types = new ObservableCollection<IssueTypeViewModel>(_boardService.GetTypes().Select(t => new IssueTypeViewModel(service, t)));
            Statuses = new ObservableCollection<IssueStatusViewModel>(_boardService.GetStatuses().Select(t => new IssueStatusViewModel(service, t)));
            Priorities = new ObservableCollection<IssuePriorityViewModel>(_boardService.GetPriorities().Select(t => new IssuePriorityViewModel(service, t)));
        }

        public ObservableCollection<IssueTypeViewModel> Types { get; set; }

        public ObservableCollection<IssueStatusViewModel> Statuses { get; set; }

        public ObservableCollection<IssuePriorityViewModel> Priorities { get; set; }

        public IssueFilter ToModel()
        {
            var model = new IssueFilter();
            model.Types = new HashSet<IssueType>(Types.Where(t => t.IsChecked).Select(t => t.IssueType));
            model.Statuses = new HashSet<IssueStatus>(Statuses.Where(t => t.IsChecked).Select(t => t.IssueStatus));
            model.Priorities = new HashSet<int>(Priorities.Where(t => t.IsChecked).Select(t => t.Priority));
            return model;
        }
    }
}
