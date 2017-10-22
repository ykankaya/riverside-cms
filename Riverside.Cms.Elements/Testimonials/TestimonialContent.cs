using System;
using System.Collections.Generic;
using System.Text;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Testimonials
{
    public class TestimonialContent : ElementContent
    {
        public IDictionary<object, object> Items { get; set; }
    }
}
