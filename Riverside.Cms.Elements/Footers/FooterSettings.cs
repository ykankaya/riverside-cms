using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Footers
{
    public class FooterSettings : ElementSettings
    {
        public string Message { get; set; }
        public bool ShowLoggedOnUserOptions { get; set; }
        public bool ShowLoggedOffUserOptions { get; set; }
    }
}
