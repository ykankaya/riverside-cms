using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forums
{
    /// <summary>
    /// Contains user information, including alias and optionally details of user thumbnail photo.
    /// </summary>
    public class ForumUser
    {
        /// <summary>
        /// Alias of user who started thread.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Height of user thumbnail photo in pixels (or null if user has not uploaded a photo).
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Width of user thumbnail photo in pixels (or null if user has not uploaded a photo).
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// The date and time when user uploaded a photo (or null if user has not uploaded a photo).
        /// </summary>
        public DateTime? Uploaded { get; set; }
    }
}
