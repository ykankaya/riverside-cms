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
    /// Contains page information.
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Identifies website that this page belongs to.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// The page identifier.
        /// </summary>
        public long PageId { get; set; }

        /// <summary>
        /// The master page that this page is based on.
        /// </summary>
        public long MasterPageId { get; set; }

        /// <summary>
        /// The parent page identifier. Can be null if this page is the home (aka root) page of a website.
        /// </summary>
        public long? ParentPageId { get; set; }

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
        /// Date and time page created.
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "CreatedLabel")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Date and time page last updated (is initially set to the created date).
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "UpdatedLabel")]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Occurred date (for example, if page corresponds to an event).
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "OccurredLabel")]
        public DateTime? Occurred { get; set; }

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

        /// <summary>
        /// The zones that make up this page.
        /// </summary>
        public List<PageZone> PageZones { get; set; }

        /// <summary>
        /// The parent page.
        /// </summary>
        public Page ParentPage { get; set; }

        /// <summary>
        /// Child pages.
        /// </summary>
        public List<Page> ChildPages { get; set; }

        /// <summary>
        /// Tags associated with this page.
        /// </summary>
        public IList<Tag> Tags { get; set; }
    }
}
