using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.PageList
{
    public class PageListSettings : ElementSettings
    {
        public long? PageTenantId { get; set; }
        public long? PageId { get; set; }
        public string DisplayName { get; set; }
        public PageSortBy SortBy { get; set; }
        public bool SortAsc { get; set; }
        public bool ShowRelated { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowImage { get; set; }
        public bool ShowBackgroundImage { get; set; }
        public bool ShowCreated { get; set; }
        public bool ShowUpdated { get; set; }
        public bool ShowOccurred { get; set; }
        public bool ShowComments { get; set; }
        public bool ShowTags { get; set; }
        public bool ShowPager { get; set; }
        public string MoreMessage { get; set; }
        public bool Recursive { get; set; }
        public PageType PageType { get; set; }
        public int PageSize { get; set; }
        public string NoPagesMessage { get; set; }
        public string Preamble { get; set; }
    }
}
