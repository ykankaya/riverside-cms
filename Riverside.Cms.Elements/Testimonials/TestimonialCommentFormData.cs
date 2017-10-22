using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Riverside.Cms.Elements.Testimonials
{
    /// <summary>
    /// Data that is sent back in the testimonial comment form. Contains submit button labels for create and update actions, as well as the default values
    /// for testimonial comment when creating a new testimonial comment.
    /// </summary>
    public class TestimonialCommentFormData
    {
        /// <summary>
        /// Keys for labels are "create" and "update". Corresponding values are the text that appear on action buttons.
        /// </summary>
        [JsonProperty(PropertyName = "labels")]
        public Dictionary<string, string> Labels { get; set; }

        /// <summary>
        /// Testimonial comment that can be used to populate the initial create testimonial comment form.
        /// </summary>
        [JsonProperty(PropertyName = "testimonialComment")]
        public TestimonialComment TestimonialComment { get; set; }
    }
}
