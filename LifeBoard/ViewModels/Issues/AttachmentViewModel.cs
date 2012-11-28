using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeBoard.ViewModels.Issues
{
    public class AttachmentViewModel : ParentViewModelBase
    {
        public AttachmentViewModel(object parent) : base(parent)
        {
        }

        public string FileName { get; set; }
    }
}
