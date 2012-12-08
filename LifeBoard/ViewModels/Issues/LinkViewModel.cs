using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeBoard.ViewModels.Issues
{
    public class LinkViewModel : ParentViewModelBase
    {
        public LinkViewModel(object parent) : base(parent)
        {
        }

        public string LinkName { get; set; }
    }
}
