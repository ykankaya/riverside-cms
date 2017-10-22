using Riverside.Cms.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.NavBars
{
    public class NavBarPageTab
    {
        public Page Page { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
