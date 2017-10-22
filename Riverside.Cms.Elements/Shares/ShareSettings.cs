using Riverside.Cms.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Shares
{
    public class ShareSettings : ElementSettings
    {
        public string DisplayName { get; set; }
        public bool ShareOnDigg { get; set; }
        public bool ShareOnFacebook { get; set; }
        public bool ShareOnGoogle { get; set; }
        public bool ShareOnLinkedIn { get; set; }
        public bool ShareOnPinterest { get; set; }
        public bool ShareOnReddit { get; set; }
        public bool ShareOnStumbleUpon { get; set; }
        public bool ShareOnTumblr { get; set; }
        public bool ShareOnTwitter { get; set; }
    }
}
