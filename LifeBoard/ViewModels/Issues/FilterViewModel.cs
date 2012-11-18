using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class FilterViewModel : ParentViewModelBase
    {
        public FilterViewModel(object parent)
            : base(parent)
        {
            Types = new ObservableCollection<TypeViewModel>();
            Statuses = new ObservableCollection<StatusViewModel>();
            Priorities = new ObservableCollection<PriorityViewModel>();
        }

        public ObservableCollection<TypeViewModel> Types { get; set; }

        public ObservableCollection<StatusViewModel> Statuses { get; set; }

        public ObservableCollection<PriorityViewModel> Priorities { get; set; }

        public void SetModel(IssueFilter model, IssueFilter filter)
        {
            Types.Clear();
            Statuses.Clear();
            Priorities.Clear();
            foreach (var type in model.Types.Select(t => new TypeViewModel(Parent, t, filter.Types.Contains(t))))
            {
                Types.Add(type);
            }
            foreach (var status in model.Statuses.Select(t => new StatusViewModel(Parent, t, filter.Statuses.Contains(t))))
            {
                Statuses.Add(status);
            }
            foreach (var priority in model.Priorities.Select(t => new PriorityViewModel(Parent, t, filter.Priorities.Contains(t))))
            {
                Priorities.Add(priority);
            }
        }

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
