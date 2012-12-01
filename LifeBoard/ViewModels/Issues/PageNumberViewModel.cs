using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeBoard.ViewModels.Issues
{
    public class PageNumberViewModel : ParentViewModelBase
    {
        public PageNumberViewModel(object parent, int number) : base(parent)
        {
            Number = number;
        }

        public int Number { get; set; }

        public string Header
        {
            get { return Number.ToString(CultureInfo.InvariantCulture); }
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        protected bool Equals(PageNumberViewModel other)
        {
            return Equals(Number, other.Number);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PageNumberViewModel)obj);
        }
    }
}
