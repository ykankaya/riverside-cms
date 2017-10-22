using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Albums
{
    public class AlbumSettings : ElementSettings
    {
        public string DisplayName { get; set; }
        public List<AlbumPhoto> Photos { get; set; }
    }
}
