using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Holds master page details.
    /// </summary>
    public class MasterPage
    {
        /// <summary>
        /// Identifies website that this master page belongs to.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// Unique master page identifier.
        /// </summary>
        public long MasterPageId { get; set; }

        /// <summary>
        /// Master page name.
        /// </summary>
        [Display(ResourceType = typeof(MasterPageResource), Name = "NameLabel")]
        [Required(ErrorMessageResourceType = typeof(MasterPageResource), ErrorMessageResourceName = "NameRequiredMessage")]
        [StringLength(MasterPageLengths.NameMaxLength, ErrorMessageResourceType = typeof(MasterPageResource), ErrorMessageResourceName = "NameMaxLengthMessage")]
        [RegularExpression(RegularExpression.Trimmed, ErrorMessageResourceType = typeof(MasterPageResource), ErrorMessageResourceName = "NameInvalidMessage")]
        public string Name { get; set; }

        /// <summary>
        /// The name that will be given to newly created pages that are based on this master page.
        /// </summary>
        public string PageName { get; set; }

        /// <summary>
        /// The description that will be given to newly created pages that are based on this master page.
        /// </summary>
        public string PageDescription { get; set; }

        /// <summary>
        /// Determines an ancestor of pages created with this master page (can be null if no ancestor information set, i.e. page with this master can be created anywhere).
        /// </summary>
        [Display(ResourceType = typeof(MasterPageResource), Name = "AncestorPageIdLabel")]
        public long? AncestorPageId { get; set; }

        /// <summary>
        /// Determines number of levels between ancestor and pages created with this master page (can be null if no ancestor information set, i.e. page with this master can be created anywhere).
        /// </summary>
        [Display(ResourceType = typeof(MasterPageResource), Name = "AncestorPageLevelLabel")]
        public PageLevel? AncestorPageLevel { get; set; }

        /// <summary>
        /// The type of page that is created by this master page.
        /// </summary>
        [Display(ResourceType = typeof(MasterPageResource), Name = "PageTypeLabel")]
        [Required(ErrorMessageResourceType = typeof(MasterPageResource), ErrorMessageResourceName = "PageTypeRequiredMessage")]
        public PageType PageType { get; set; }

        /// <summary>
        /// Boolean indicates whether page created with this master page has an occurred date (i.e. page corresponds to an event that has a date and time).
        /// </summary>
        [Display(ResourceType = typeof(MasterPageResource), Name = "HasOccurredLabel")]
        public bool HasOccurred { get; set; }

        /// <summary>
        /// Boolean indicates whether page created with this master page can have an image.
        /// </summary>
        public bool HasImage { get; set; }

        /// <summary>
        /// Width of thumbnail image that page created with this master page has (null if master page does not allow images).
        /// </summary>
        public int? ThumbnailImageWidth { get; set; }

        /// <summary>
        /// Height of thumbnail image that page created with this master page has (null if master page does not allow images).
        /// </summary>
        public int? ThumbnailImageHeight { get; set; }

        /// <summary>
        /// Determines how thumbnail images generated (null if master page does not allow images).
        /// </summary>
        public ResizeMode? ThumbnailImageResizeMode { get; set; }

        /// <summary>
        /// Width of preview image that page created with this master page has (null if master page does not allow images).
        /// </summary>
        public int? PreviewImageWidth { get; set; }

        /// <summary>
        /// Height of preview image that page created with this master page has (null if master page does not allow images).
        /// </summary>
        public int? PreviewImageHeight { get; set; }

        /// <summary>
        /// Determines how preview images generated (null if master page does not allow images).
        /// </summary>
        public ResizeMode? PreviewImageResizeMode { get; set; }

        /// <summary>
        /// The minimum allowed width of an uploaded page image (null if master page does not allow images).
        /// </summary>
        public int? ImageMinWidth { get; set; }

        /// <summary>
        /// The minimum allowed height of an uploaded page image (null if master page does not allow images).
        /// </summary>
        public int? ImageMinHeight { get; set; }

        /// <summary>
        /// Indicates whether pages with this master page can be created.
        /// </summary>
        public bool Creatable { get; set; }

        /// <summary>
        /// Indicates whether pages created with this master page can be deleted.
        /// </summary>
        public bool Deletable { get; set; }

        /// <summary>
        /// Indicates whether pages created with this master page can be tagged.
        /// </summary>
        public bool Taggable { get; set; }

        /// <summary>
        /// True if this master page is the one master page determining layout when administering the CMS.
        /// </summary>
        public bool Administration { get; set; }

        /// <summary>
        /// HTML that is rendered just after start of master page.
        /// </summary>
        public string BeginRender { get; set; }

        /// <summary>
        /// HTML that is rendered just before end of master page.
        /// </summary>
        public string EndRender { get; set; }

        /// <summary>
        /// The master page zones that make up this master page.
        /// </summary>
        public List<MasterPageZone> MasterPageZones { get; set; }
    }
}
