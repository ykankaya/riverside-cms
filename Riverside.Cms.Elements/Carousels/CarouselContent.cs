using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;

namespace Riverside.Cms.Elements.Carousels
{
    public class CarouselContent : ElementContent
    {
        public List<Page> Pages { get; set; }
        public IDictionary<object, object> Items { get; set; }
    }
}
