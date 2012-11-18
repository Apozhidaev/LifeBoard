using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeBoard.ViewModels
{
    public abstract class ParentViewModelBase : ViewModelBase
    {
        private readonly object _parent;

        protected ParentViewModelBase(object parent)
        {
            _parent = parent;
        }

        public object Parent
        {
            get { return _parent; }
        }
    }
}
