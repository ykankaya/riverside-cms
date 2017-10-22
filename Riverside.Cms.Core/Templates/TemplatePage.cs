using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Drawing;

namespace Riverside.Cms.Core.Templates
{
    /// <summary>
    /// Template pages are used to model the initial structure of a website.
    /// </summary>
    public class TemplatePage
    {
        /// <summary>
        /// The template that this template page belongs to.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// Uniquely identifies this template page.
        /// </summary>
        public long TemplatePageId { get; set; }

        /// <summary>
        /// Name of master page that will be created by this template page.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name that will be given to newly created pages that are based on the master page created by this template.
        /// </summary>
        public string PageName { get; set; }

        /// <summary>
        /// The description that will be given to newly created pages that are based on the master page created by this template.
        /// </summary>
        public string PageDescription { get; set; }

        /// <summary>
        /// Master page type. 
        /// </summary>
        public PageType PageType { get; set; }

        /// <summary>
        /// Boolean indicates whether master page has an occurred date (i.e. page corresponds to an event that has a date and time).
        /// </summary>
        public bool HasOccurred { get; set; }

        /// <summary>
        /// Boolean indicates whether master page can have an image.
        /// </summary>
        public bool HasImage { get; set; }

        /// <summary>
        /// Master page thumbnail image width (null if master page does not allow images).
        /// </summary>
        public int? ThumbnailImageWidth { get; set; }

        /// <summary>
        /// Master page thumbnail image height (null if master page does not allow images).
        /// </summary>
        public int? ThumbnailImageHeight { get; set; }

        /// <summary>
        /// Determines how thumbnail images generated (null if master page does not allow images).
        /// </summary>
        public ResizeMode? ThumbnailImageResizeMode { get; set; }

        /// <summary>
        /// Master page preview image width (null if master page does not allow images).
        /// </summary>
        public int? PreviewImageWidth { get; set; }

        /// <summary>
        /// Master page preview image height (null if master page does not allow images).
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
        /// Determines number of levels between ancestor and pages created with master page (can be null if no ancestor information set, i.e. page with master can be created anywhere).
        /// </summary>
        public PageLevel? AncestorPageLevel { get; set; }

        /// <summary>
        /// Identifies the parent template page, if this template is a child.
        /// </summary>
        public long? ParentTemplatePageId { get; set; }

        /// <summary>
        /// Set true if this template page should appear on elements that implement site navigation.
        /// </summary>
        public bool ShowOnNavigation { get; set; }

        /// <summary>
        /// Indicates whether pages with this template / master page can be created.
        /// </summary>
        public bool Creatable { get; set; }

        /// <summary>
        /// Indicates whether pages created with this template / master page can be deleted.
        /// </summary>
        public bool Deletable { get; set; }

        /// <summary>
        /// Indicates whether pages created with this template / master page can be tagged.
        /// </summary>
        public bool Taggable { get; set; }

        /// <summary>
        /// True if master page created by this template page if for administering the CMS.
        /// </summary>
        public bool Administration { get; set; }

        /// <summary>
        /// HTML that is rendered just after start of template page.
        /// </summary>
        public string BeginRender { get; set; }

        /// <summary>
        /// HTML that is rendered just before end of template page.
        /// </summary>
        public string EndRender { get; set; }

        /// <summary>
        /// The template page zones that make up this template page.
        /// </summary>
        public List<TemplatePageZone> TemplatePageZones { get; set; }

        /// <summary>
        /// Child template pages.
        /// </summary>
        public List<TemplatePage> ChildTemplatePages { get; set; }
    }
}
