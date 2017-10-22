using Riverside.Cms.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Pages
{
    public class PageZoneContent : ElementContent
    {
        public long PageId { get; set; }
        public long PageZoneId { get; set; }
    }
}
