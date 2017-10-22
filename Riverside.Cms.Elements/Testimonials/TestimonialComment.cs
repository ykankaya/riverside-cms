using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Testimonials
{
    /// <summary>
    /// Describes a comment that can appear on a testimonial element.
    /// </summary>
    public class TestimonialComment
    {
        /// <summary>
        /// Identifies tenant that this comment is associated with.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// Identifies the testimonial element that this comment is associated with.
        /// </summary>
        public long ElementId { get; set; }

        /// <summary>
        /// Identifies a comment.
        /// </summary>
        public long TestimonialCommentId { get; set; }

        /// <summary>
        /// Identifies this comment's sort order relative to other comments.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// The actual comment (or testimonial).
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Name of person who made the comment (or gave the testimonial).
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Title of person who made the comment (or gave the testimonial).
        /// </summary>
        public string AuthorTitle { get; set; }

        /// <summary>
        /// The date when the comment (or testimonial) was left.
        /// </summary>
        public string CommentDate { get; set; }
    }
}
