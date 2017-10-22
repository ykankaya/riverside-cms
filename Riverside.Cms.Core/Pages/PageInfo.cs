using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Contains page information that can be updated.
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// Page name.
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "NameLabel")]
        [Required(ErrorMessageResourceType = typeof(PageResource), ErrorMessageResourceName = "NameRequiredMessage")]
        [StringLength(PageLengths.NameMaxLength, ErrorMessageResourceType = typeof(PageResource), ErrorMessageResourceName = "NameMaxLengthMessage")]
        [RegularExpression(RegularExpression.Trimmed, ErrorMessageResourceType = typeof(PageResource), ErrorMessageResourceName = "NameInvalidMessage")]
        public string Name { get; set; }

        /// <summary>
        /// Page description.
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "DescriptionLabel")]
        public string Description { get; set; }

        /// <summary>
        /// Page tags.
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "TagsLabel")]
        public IList<Tag> Tags { get; set; }

        /// <summary>
        /// The tenant determining image upload primary key (null if no page image).
        /// </summary>
        public long? ImageTenantId { get; set; }

        /// <summary>
        /// Page thumbnail image upload identifier (null if no thumbnail image).
        /// </summary>
        public long? ThumbnailImageUploadId { get; set; }

        /// <summary>
        /// Page preview image upload identifier (null if no preview image).
        /// </summary>
        public long? PreviewImageUploadId { get; set; }

        /// <summary>
        /// Page source image upload identifier (null if no image).
        /// </summary>
        public long? ImageUploadId { get; set; }
    }
}
