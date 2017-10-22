using Riverside.Cms.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.NavBars
{
    public class NavBarSettings : ElementSettings
    {
        public string NavBarName { get; set; }
        public bool ShowLoggedOnUserOptions { get; set; }
        public bool ShowLoggedOffUserOptions { get; set; }
        public List<NavBarTab> Tabs { get; set; }
    }
}
