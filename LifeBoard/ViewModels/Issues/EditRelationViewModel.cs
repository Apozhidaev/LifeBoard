using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LifeBoard.Commands;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class EditRelationViewModel : ViewModelBase
    {
        
        private string _query = String.Empty;

        private DelegateCommand _clearCommand;

        public EditRelationViewModel()
        {
            Issues = new ObservableCollection<IssueViewModel>();
            RelationIssues = new ObservableCollection<IssueViewModel>();
        }

        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <value>The issues.</value>
        public ObservableCollection<IssueViewModel> Issues { get; private set; }

        /// <summary>
        /// Gets the parent issues.
        /// </summary>
        /// <value>The parent issues.</value>
        public ObservableCollection<IssueViewModel> RelationIssues { get; private set; }

        /// <summary>
        /// Gets the add command.
        /// </summary>
        /// <value>The add command.</value>
        public ICommand AddCommand { get; set; }

        /// <summary>
        /// Gets the remove command.
        /// </summary>
        /// <value>The remove command.</value>
        public ICommand RemoveCommand{ get; set; }

        /// <summary>
        /// Gets the search command.
        /// </summary>
        /// <value>The search command.</value>
        public ICommand SearchCommand { get; set; }

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
                    SearchCommand.Execute(null);
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
            get { return _clearCommand ?? (_clearCommand = new DelegateCommand(ClearQuery)); }
        }

        /// <summary>
        /// Clears the form.
        /// </summary>
        public void ClearForm()
        {
            Issues.Clear();
            RelationIssues.Clear();
            ClearQuery();
        }

        public bool CanAdd(IssueViewModel item)
        {
            return item != null && RelationIssues.All(pi => pi.Model.Id != item.Model.Id);
        }

        private void ClearQuery()
        {
            Query = String.Empty;
        }
    }
}
