using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class FilterViewModel
    /// </summary>
    public class FilterViewModel : ParentViewModelBase
    {
        /// <summary>
        /// The _issues view model
        /// </summary>
        private readonly IssuesViewModel _issuesViewModel;
        /// <summary>
        /// The _clear command
        /// </summary>
        private DelegateCommand _clearCommand;
        /// <summary>
        /// The _query
        /// </summary>
        private string _query = String.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public FilterViewModel(object parent)
            : base(parent)
        {
            Types = new ObservableCollection<TypeViewModel>();
            Statuses = new ObservableCollection<StatusViewModel>();
            Priorities = new ObservableCollection<PriorityViewModel>();
            _issuesViewModel = parent as IssuesViewModel;
        }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        public string Query
        {
            get { return _query; }
            set
            {
                if (_query != value)
                {
                    _query = value;
                    _issuesViewModel.AsyncSearch();
                    OnPropertyChanged("Query");
                }
            }
        }

        /// <summary>
        /// Gets the clear command.
        /// </summary>
        /// <value>The clear command.</value>
        public ICommand ClearCommand
        {
            get { return _clearCommand ?? (_clearCommand = new DelegateCommand(Clear)); }
        }

        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>The types.</value>
        public ObservableCollection<TypeViewModel> Types { get; set; }

        /// <summary>
        /// Gets or sets the statuses.
        /// </summary>
        /// <value>The statuses.</value>
        public ObservableCollection<StatusViewModel> Statuses { get; set; }

        /// <summary>
        /// Gets or sets the priorities.
        /// </summary>
        /// <value>The priorities.</value>
        public ObservableCollection<PriorityViewModel> Priorities { get; set; }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Query = String.Empty;
        }

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="filter">The filter.</param>
        public void SetModel(IssueFilter model, IssueFilter filter)
        {
            Types.Clear();
            Statuses.Clear();
            Priorities.Clear();
            foreach (
                TypeViewModel type in model.Types.Select(t => new TypeViewModel(Parent, t, filter.Types.Contains(t))))
            {
                Types.Add(type);
            }
            foreach (
                StatusViewModel status in
                    model.Statuses.Select(t => new StatusViewModel(Parent, t, filter.Statuses.Contains(t))))
            {
                Statuses.Add(status);
            }
            foreach (
                PriorityViewModel priority in
                    model.Priorities.Select(t => new PriorityViewModel(Parent, t, filter.Priorities.Contains(t))))
            {
                Priorities.Add(priority);
            }
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <returns>IssueFilter.</returns>
        public IssueFilter ToModel()
        {
            var model = new IssueFilter();
            model.Query = Query;
            model.Types = new HashSet<IssueType>(Types.Where(t => t.IsChecked).Select(t => t.IssueType));
            model.Statuses = new HashSet<IssueStatus>(Statuses.Where(t => t.IsChecked).Select(t => t.IssueStatus));
            model.Priorities = new HashSet<int>(Priorities.Where(t => t.IsChecked).Select(t => t.Priority));
            return model;
        }
    }
}