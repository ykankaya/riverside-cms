using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Interfaces for repositories that manage pages.
    /// </summary>
    public interface IPageRepository
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
        /// <param name="tenantId">Identifies website.</param>
        /// <param name="names">List of tag names.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of tags, identified by name.</returns>
        List<Tag> ListNamedTags(long tenantId, List<string> names, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// List pages by master page.
        /// </summary>
        /// <param name="tenantId">Identifies website.</param>
        /// <param name="masterPageId">Master page whose pages are returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>The pages associated with a master page.</returns>
        List<Page> ListPagesByMasterPage(long tenantId, long masterPageId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name="page">New page details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated page identifier.</returns>
        long Create(Page page, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Sets page tags.
        /// </summary>
        /// <param name="page">Contains information required to update page tags.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void UpdateTags(Page page, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that pages belong to.</param>
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
        /// Updates a page (not including zones and elements).
        /// </summary>
        /// <param name="page">Updated page details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Update(Page page, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Retrieves count of the number of times an element occurs on a page.
        /// </summary>
        /// <param name="tenantId">Tenant identifies website whose pages are searched.</param>
        /// <param name="elementId">The element whose count is returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element count.</returns>
        int GetElementPageCount(long tenantId, long elementId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates a zone's elements.
        /// </summary>
        /// <param name="tenantId">The tenant identifying website that page belongs to.</param>
        /// <param name="pageId">The page that zone belongs to.</param>
        /// <param name="pageZoneId">Identifies zone within page that is being updated.</param>
        /// <param name="pageZoneElements">Updated list of page zone elements.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void UpdatePageZoneElements(long tenantId, long pageId, long pageZoneId, List<PageZoneElement> pageZoneElements, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Get all of the page zones associated with a master page zone.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of page zones associated with the given master page zone.</returns>
        List<PageZone> ListPageZonesByMasterPageZoneId(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Creates multiple page zone elements.
        /// </summary>
        /// <param name="tenantId">Identifies website where page zone elements created.</param>
        /// <param name="pageZoneElements">List of page zone elements to create.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void CreatePageZoneElements(long tenantId, List<PageZoneElement> pageZoneElements, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Creates multiple page zones.
        /// </summary>
        /// <param name="tenantId">Identifies website where page zones created.</param>
        /// <param name="pageZones">List of page zones to create.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void CreatePageZones(long tenantId, List<PageZone> pageZones, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes page zone elements associated with a master page zone.
        /// </summary>
        /// <param name="tenantId">Identifies website whose page zone elements are deleted.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void DeletePageZoneElementsByMasterPageZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes page zones associated with a master page zone.
        /// </summary>
        /// <param name="tenantId">Identifies website whose page zone elements are deleted.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void DeletePageZonesByMasterPageZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Converts page zone elements from format that is compatible with editable master page zone to format that is compatible with configurable master page zone.
        /// For page zone elements associated with a given master page zone, sets master page details NULL and updates sort order so NOT NULL.
        /// </summary>
        /// <param name="tenantId">Identifies website whose page zone elements are updated.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void UpdatePageZoneElementsForConfigurableAdminType(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null);
    }
}
