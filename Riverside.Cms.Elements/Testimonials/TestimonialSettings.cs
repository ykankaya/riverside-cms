using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Testimonials
{
    /// <summary>
    /// Determines what is displayed in a testimonial element.
    /// </summary>
    public class TestimonialSettings : ElementSettings
    {
        /// <summary>
        /// Display name is a header that is rendered at the top of the testimonial element. Can be left empty if a header is not required.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// If set, preamble text rendered above testimonial information. Can be empty if not required.
        /// </summary>
        public string Preamble { get; set; }

        /// <summary>
        /// List of testimonial comments.
        /// </summary>
        public List<TestimonialComment> Comments { get; set; }
    }
}
