using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Interface for master page repositories.
    /// </summary>
    public interface IMasterPageRepository
    {
        /// <summary>
        /// Searches master pages.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        ISearchResult<MasterPage> Search(long tenantId, ISearchParameters parameters, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Lists a website's master pages that can be created.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of master pages that can be created.</returns>
        IEnumerable<MasterPage> ListCreatable(long tenantId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Creates a new master page.
        /// </summary>
        /// <param name="masterPage">New master page details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated master page identifier.</returns>
        long Create(MasterPage masterPage, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets master page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="masterPageId">Identifies the master page to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Master page details (or null if master page not found).</returns>
        MasterPage Read(long tenantId, long masterPageId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets master page zone details.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="masterPageId">Identifies the master page whose zone is returned.</param>
        /// <param name="masterPageZoneId">Identifies the master page zone to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Master page zone details (or null if master page zone not found).</returns>
        MasterPageZone ReadZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets admin master page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Master page details (or null if master page not found).</returns>
        MasterPage ReadAdministration(long tenantId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Retrieves count of the number of times an element occurs on a master page.
        /// </summary>
        /// <param name="tenantId">Tenant identifies website whose master pages are searched.</param>
        /// <param name="elementId">The element whose count is returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element count.</returns>
        int GetElementMasterPageCount(long tenantId, long elementId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates existing master page.
        /// </summary>
        /// <param name="masterPage">Updated master page details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Update(MasterPage masterPage, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates existing master page zone.
        /// </summary>
        /// <param name="masterPageZone">Updated master page zone details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void UpdateZone(MasterPageZone masterPageZone, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Adds, removes and re-orders master page zones.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="masterPageId">Identifier of master page whose zones are added, removed or re-ordered.</param>
        /// <param name="masterPageZones">Updated list of master page zones.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void UpdateZones(long tenantId, long masterPageId, List<MasterPageZone> masterPageZones, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Set specified master page as admin and ensure all other master pages belonging to the same tenant do not have master page set as admin.
        /// There can only be 1 admin master page.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="masterPageId">Identifier of master page to be made the admin master page.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void SetAdminMasterPage(long tenantId, long masterPageId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Retrieves distinct list of element identifiers that are associated with the supplied list of master page zone element identifiers.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="masterPageZoneElementIds">List of master page zone element identifiers, whose associated elements are returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Distinct list of element identifiers associated with supplied list of master page zone element identifiers.</returns>
        List<long> ListPageElementsByMasterPageZoneElementIds(long tenantId, long masterPageId, long masterPageZoneId, List<long> masterPageZoneElementIds, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Retrieves distinct list of element identifiers that are associated with a master page zone.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Distinct list of element identifiers associated with master page zone.</returns>
        List<long> ListPageElementsByMasterPageZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes page zone elements that are associated with list of master page zone element identifiers.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="masterPageZoneElementIds">List of master page zone element identifiers, identifying page zone elements to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void DeletePageZoneElementsByMasterPageZoneElementIds(long tenantId, long masterPageId, long masterPageZoneId, List<long> masterPageZoneElementIds, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes page zone elements associated with a master page zone.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void DeletePageZoneElementsByMasterPageZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes page zones associated with a master page zone.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void DeletePageZonesByMasterPageZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Get elements for master pages belonging to the specified tenant.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of elements.</returns>
        IEnumerable<MasterPage> ListElementsByMasterPage(long tenantId, IUnitOfWork unitOfWork = null);
    }
}
