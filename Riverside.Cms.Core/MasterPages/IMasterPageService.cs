using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Interfaces for services that manage master pages.
    /// </summary>
    public interface IMasterPageService
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
        /// <param name="masterPage">Contains details of new master page to create.</param>
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
        /// Gets admin master page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Master page details (or null if master page not found).</returns>
        MasterPage ReadAdministration(long tenantId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates master page.
        /// </summary>
        /// <param name="masterPage">The updated master page.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Update(MasterPage masterPage, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates single zone within a master page.
        /// </summary>
        /// <param name="masterPageZone">The updated master page zone.</param>
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
        /// Get elements for master pages belonging to the specified tenant.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of elements.</returns>
        IEnumerable<MasterPage> ListElementsByMasterPage(long tenantId, IUnitOfWork unitOfWork = null);
    }
}
