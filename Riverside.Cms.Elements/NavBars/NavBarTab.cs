using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.NavBars
{
    public class NavBarTab
    {
        public long ElementId { get; set; }
        public long NavBarTabId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public long TenantId { get; set; }
        public long PageId { get; set; }
    }
}
