using System.Windows.Input;

namespace LifeBoard.ViewModels
{
    /// <summary>
    /// Interface INavigatePage
    /// </summary>
    public interface INavigatePage
    {
        /// <summary>
        /// Gets the navigate command.
        /// </summary>
        /// <value>The navigate command.</value>
        ICommand NavigateCommand { get; }

        /// <summary>
        /// Gets the back command.
        /// </summary>
        /// <value>The back command.</value>
        ICommand BackCommand { get; }
    }
}