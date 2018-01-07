using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Element.Domain
{
    public class PageHeaderElementSettings : ElementSettings
    {
        public long? PageId { get; set; }
        public bool ShowName { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowImage { get; set; }
        public bool ShowCreated { get; set; }
        public bool ShowUpdated { get; set; }
        public bool ShowOccurred { get; set; }
        public bool ShowBreadcrumbs { get; set; }
    }
}
