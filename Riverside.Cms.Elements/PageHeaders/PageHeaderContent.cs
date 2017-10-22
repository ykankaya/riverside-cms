using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.PageHeaders
{
    public class PageHeaderContent : ElementContent, IPageLinkConsumer
    {
        public Page Page { get; set; }
        public IList<IPageLink> PageHierarchyPageLinks { get; set; }
        public IList<IPageLink> PageLinks { get; set; }
    }
}
