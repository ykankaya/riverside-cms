using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Webs
{
    /// <summary>
    /// Contains top level website information.
    /// </summary>
    public class Web
    {
        /// <summary>
        /// The tenant that identifies this website.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// Website name.
        /// </summary>
        [Display(ResourceType = typeof(WebResource), Name = "NameLabel")]
        [Required(ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "NameRequiredMessage")]
        [StringLength(WebLengths.NameMaxLength, ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "NameMaxLengthMessage")]
        [RegularExpression(RegularExpression.Trimmed, ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "NameInvalidMessage")]
        public string Name { get; set; }

        /// <summary>
        /// Set true if user registration is allowed by website. 
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
        /// The domains associated with a website.
        /// </summary>
        public List<Domain> Domains { get; set; }

        /// <summary>
        /// Name of font option override.
        /// </summary>
        public string FontOption { get; set; }

        /// <summary>
        /// Name of colour option override.
        /// </summary>
        public string ColourOption { get; set; }

        /// <summary>
        /// Returns string representation of this object.
        /// </summary>
        /// <returns>Website name.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
