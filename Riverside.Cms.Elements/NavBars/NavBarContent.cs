using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.NavBars
{
    public class NavBarContent : ElementContent, IAdministrationContent
    {
        public IAdministrationOptions Options { get; set; }
        public List<NavBarPageTab> PageTabs { get; set; }
    }
}
