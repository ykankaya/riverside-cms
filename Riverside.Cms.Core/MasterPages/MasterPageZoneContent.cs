using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Element content required for master page zone administration.
    /// </summary>
    public class MasterPageZoneContent : ElementContent
    {
        public long MasterPageId { get; set; }
        public long MasterPageZoneId { get; set; }
    }
}
