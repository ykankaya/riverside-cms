using Riverside.Cms.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Shares
{
    public class ShareContent : ElementContent
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Via { get; set; }
        public string Hashtags { get; set; }
        public string Image { get; set; }
        public string IsVideo { get; set; }
    }
}
