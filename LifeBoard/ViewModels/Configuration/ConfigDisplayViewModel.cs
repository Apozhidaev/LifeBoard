using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LifeBoard.Models.Configs;

namespace LifeBoard.ViewModels.Configuration
{
    public class ConfigDisplayViewModel
    {
        public ConfigDisplayViewModel()
        {
            ShowIssue = new ConfigShowIssueViewModel();
        }

        public ConfigShowIssueViewModel ShowIssue { get; set; }
    }
}
