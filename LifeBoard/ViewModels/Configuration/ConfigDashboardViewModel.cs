using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeBoard.Models.Configs;

namespace LifeBoard.ViewModels.Configuration
{
    public class ConfigDashboardViewModel : ViewModelBase
    {
        public bool IsSortByPriority
        {
            get { return ConfigRepository.Config.Display.Dashboard.IsSortByPriority; }
            set
            {
                if (ConfigRepository.Config.Display.Dashboard.IsSortByPriority != value)
                {
                    ConfigRepository.Config.Display.Dashboard.IsSortByPriority = value;
                    ConfigRepository.Save();
                    OnPropertyChanged("IsSortByPriority");
                }
            }
        }

        public bool IsSortByDeadline
        {
            get { return ConfigRepository.Config.Display.Dashboard.IsSortByDeadline; }
            set
            {
                if (ConfigRepository.Config.Display.Dashboard.IsSortByDeadline != value)
                {
                    ConfigRepository.Config.Display.Dashboard.IsSortByDeadline = value;
                    ConfigRepository.Save();
                    OnPropertyChanged("IsSortByDeadline");
                }
            }
        }
    }
}
