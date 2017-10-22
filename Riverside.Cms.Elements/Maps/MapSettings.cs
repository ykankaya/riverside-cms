using Riverside.Cms.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Maps
{
    public class MapSettings : ElementSettings
    {
        public string DisplayName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}