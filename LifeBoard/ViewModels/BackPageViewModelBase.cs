using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LifeBoard.ViewModels
{
    public abstract class BackPageViewModelBase : PageViewModelBase
    {
        private readonly ICommand _backNavigateCommand;

        protected BackPageViewModelBase(IFrameViewModel parent, ICommand backNavigateCommand) : base(parent)
        {
            _backNavigateCommand = backNavigateCommand;
        }

        public ICommand BackNavigateCommand
        {
            get { return _backNavigateCommand; }
        }
    }
}
