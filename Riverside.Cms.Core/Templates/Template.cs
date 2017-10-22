using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Webs;
using Riverside.Utilities.Drawing;

namespace Riverside.Cms.Core.Templates
{
    /// <summary>
    /// Determines how a website is initially constructed.
    /// </summary>
    public class Template
    {
        /// <summary>
        /// The tenant uniquely identifying this template.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// Template name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Template description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Set true if user registration will allowed by websites created with this template. 
        /// </summary>
        public bool CreateUserEnabled { get; set; }

        /// <summary>
        /// Boolean indicates whether users can have profile / avatar images.
        /// </summary>
        public bool UserHasImage { get; set; }

        /// <summary>
        /// User profile / avatar image thumbnail image width (null if user images not allowed).
        /// </summary>
        public int? UserThumbnailImageWidth { get; set; }

        /// <summary>
        /// User profile / avatar image thumbnail image height (null if user images not allowed).
        /// </summary>
        public int? UserThumbnailImageHeight { get; set; }

        /// <summary>
        /// Determines how user thumbnail images generated (null if user images not allowed).
        /// </summary>
        public ResizeMode? UserThumbnailImageResizeMode { get; set; }

        /// <summary>
        /// User profile / avatar image preview image width (null if user images not allowed).
        /// </summary>
        public int? UserPreviewImageWidth { get; set; }

        /// <summary>
        /// User profile / avatar image preview image height (null if user images not allowed).
        /// </summary>
        public int? UserPreviewImageHeight { get; set; }

        /// <summary>
        /// Determines how user preview images generated (null if user images not allowed).
        /// </summary>
        public ResizeMode? UserPreviewImageResizeMode { get; set; }

        /// <summary>
        /// The minimum allowed width for uploaded user images.
        /// </summary>
        public int? UserImageMinWidth { get; set; }

        /// <summary>
        /// The minimum allowed height for uploaded user images.
        /// </summary>
        public int? UserImageMinHeight { get; set; }

        /// <summary>
        /// Hierarchy of template pages determining newly created website structure.
        /// </summary>
        public TemplatePage Page { get; set; }
    }
}
