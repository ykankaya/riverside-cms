using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Carousels
{
    public class CarouselSettings : ElementSettings
    {
        public List<CarouselSlide> Slides { get; set; }
    }
}
