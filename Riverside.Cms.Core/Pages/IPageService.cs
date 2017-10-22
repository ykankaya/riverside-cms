using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Uploads;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Interfaces for services that manage pages.
    /// </summary>
    public interface IPageService
    {
        /// <summary>
        /// Searches pages.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are searched.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        ISearchResult<Page> Search(long tenantId, ISearchParameters parameters, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Returns all folders where a page with the specified master can be created.
        /// </summary>
        /// <param name="masterPage">The master page.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of folders where pages can be created.</returns>
        List<Page> ListMasterPageParentPages(MasterPage masterPage,  IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Retrieves list of pages.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are listed.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="pageId">Identifies the parent page whose child pages are returned (set NULL to include root folder of website).</param>
        /// <param name="sortBy">The sort order of pages returned.</param>
        /// <param name="sortAsc">True to sort ascending, false to sort descending.</param>
        /// <param name="recursive">Set true to get all child pages, false if only direct descendants are required.</param>
        /// <param name="pageType">The type of page listed (document or folder).</param>
        /// <param name="loadTags">Indicates whether tags should be loaded for retrieved pages.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        ISearchResult<Page> List(long tenantId, ISearchParameters parameters, long? pageId, PageSortBy sortBy, bool sortAsc, bool recursive, PageType pageType, bool loadTags, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Retrieves list of pages filtered by tags.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are listed.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="tags">Tags used to filter pages.</param>
        /// <param name="pageId">Identifies the parent page whose child pages are returned (set NULL to include root folder of website).</param>
        /// <param name="sortBy">The sort order of pages returned.</param>
        /// <param name="sortAsc">True to sort ascending, false to sort descending.</param>
        /// <param name="recursive">Set true to get all child pages, false if only direct descendants are required.</param>
        /// <param name="pageType">The type of page listed (document or folder).</param>
        /// <param name="loadTags">Indicates whether tags should be loaded for retrieved pages.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        ISearchResult<Page> ListTagged(long tenantId, ISearchParameters parameters, IList<Tag> tags, long? pageId, PageSortBy sortBy, bool sortAsc, bool recursive, PageType pageType, bool loadTags, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Lists tags associated with page.
        /// </summary>
        /// <param name="tenantId">Identifies website whose page tags are listed.</param>
        /// <param name="pageId">Identifies the parent page whose child page tags are returned (set NULL to include root folder of website).</param>
        /// <param name="recursive">Set true to get tags of all child pages, false if only tags of direct descendants are required.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of tag counts.</returns>
        List<TagCount> ListTags(long tenantId, long? pageId, bool recursive, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Lists tags associated with page that are related to an existing list of tags.
        /// </summary>
        /// <param name="tenantId">Identifies website whose page tags are listed.</param>
        /// <param name="pageId">Identifies the parent page whose child page tags are returned (set NULL to include root folder of website).</param>
        /// <param name="tags">Tags must be related to these tags.</param>
        /// <param name="recursive">Set true to get tags of all child pages, false if only tags of direct descendants are required.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of tag counts.</returns>
        List<TagCount> ListTaggedPageTags(long tenantId, long? pageId, IList<Tag> tags, bool recursive, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets tags with specified names. The number of tags returned may not match the number of names specified, if tags do not exist for some names.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="names">List of tag names.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of tags, identified by name.</returns>
        List<Tag> ListNamedTags(long tenantId, List<string> names, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name="tenantId">The website that created page will belong to.</param>
        /// <param name="parentPageId">Parent page (can be null if page has no parent - i.e. is home page, or if can be determined).</param>
        /// <param name="masterpageId">The master page that newly created page is based on.</param>
        /// <param name="pageInfo">If specified, used to override master page settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated page identifier.</returns>
        long Create(long tenantId, long? parentPageId, long masterPageId, PageInfo pageInfo, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that page belongs to.</param>
        /// <param name="pageId">Identifies the page to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Page details (or null if page not found).</returns>
        Page Read(long tenantId, long pageId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets home page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that home page belongs to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Home page details (or null if home page not found).</returns>
        Page ReadHome(long tenantId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets page hierarchy (only primary page information is returned by this call, no zones or elements).
        /// </summary>
        /// <param name="tenantId">Website that page belongs to.</param>
        /// <param name="pageId">The page whose hierarchy is returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Page hierarchy, with parent pages populated (or null if page not found).</returns>
        Page ReadHierarchy(long tenantId, long pageId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets thumbnail image associated with page (null if no thumbnail image).
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="pageId">Page identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Thumbnail image.</returns>
        Image ReadThumbnailImage(long tenantId, long pageId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets preview image associated with page (null if no preview image).
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="pageId">Page identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Preview image.</returns>
        Image ReadPreviewImage(long tenantId, long pageId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets storage hierarchy for page images.
        /// </summary>
        /// <returns>Storage hierarchy.</returns>
        List<string> GetPageImageStorageHierarchy();

        /// <summary>
        /// From a single uploaded file, creates thumbnail, preview and original images in underlying storage that may be associated with a page.
        /// </summary>
        /// <param name="tenantId">Website that page belongs to.</param>
        /// <param name="masterPageId">Master page containing image upload rules.</param>
        /// <param name="model">Image upload.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Identifiers of newly created thumbnail, preview and source images.</returns>
        ImageUploadIds PrepareImages(long tenantId, long masterPageId, CreateUploadModel model, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates a page (not including zones and elements).
        /// </summary>
        /// <param name="page">Updated page details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Update(Page page, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates a page zone. Creates new elements and removes old elements. Re-orders existing elements.
        /// </summary>
        /// <param name="tenantId">Website that page belongs to.</param>
        /// <param name="pageId">Page identifier.</param>
        /// <param name="pageZoneId">Page zone identifier.</param>
        /// <param name="pageZoneElements">New page zone contents.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void UpdateZone(long tenantId, long pageId, long pageZoneId, List<PageZoneElementInfo> pageZoneElements, IUnitOfWork unitOfWork = null);
    }
}
