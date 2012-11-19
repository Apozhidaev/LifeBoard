using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LifeBoard.ViewModels
{
    public interface INavigatePage
    {
        ICommand NavigateCommand { get; }

        ICommand BackCommand { get; }
    }
}
