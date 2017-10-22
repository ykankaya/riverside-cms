using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Interfaces for services that manage master pages.
    /// </summary>
    public class MasterPageService : IMasterPageService
    {
        // Member variables
        private IElementService _elementService;
        private IMasterPageRepository _masterPageRepository;
        private IMasterPageValidator _masterPageValidator;
        private IPageRepository _pageRepository;
        private IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="elementService">Used to retrieve, update and delete elements.</param>
        /// <param name="masterPageRepository">Master page repository.</param>
        /// <param name="masterPageValidator">Master page validator.</param>
        /// <param name="pageRepository">Page repository.</param>
        /// <param name="unitOfWorkFactory">For the creation of units of work to ensure transactional integrity.</param>
        public MasterPageService(IElementService elementService, IMasterPageRepository masterPageRepository, IMasterPageValidator masterPageValidator, IPageRepository pageRepository, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _elementService = elementService;
            _masterPageRepository = masterPageRepository;
            _masterPageValidator = masterPageValidator;
            _pageRepository = pageRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Searches master pages.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        public ISearchResult<MasterPage> Search(long tenantId, ISearchParameters parameters, IUnitOfWork unitOfWork = null)
        {
            if (parameters.Search == null)
                parameters.Search = string.Empty;
            else
                parameters.Search = parameters.Search.Trim();
            if (parameters.PageSize == 0)
                parameters.PageSize = 10;
            return _masterPageRepository.Search(tenantId, parameters, unitOfWork);
        }

        /// <summary>
        /// Lists a website's master pages that can be created.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of master pages that can be created.</returns>
        public IEnumerable<MasterPage> ListCreatable(long tenantId, IUnitOfWork unitOfWork = null)
        {
            return _masterPageRepository.ListCreatable(tenantId, unitOfWork);
        }

        /// <summary>
        /// Creates a new master page.
        /// </summary>
        /// <param name="masterPage">Contains details of new master page to create.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated master page identifier.</returns>
        public long Create(MasterPage masterPage, IUnitOfWork unitOfWork = null)
        {
            return _masterPageRepository.Create(masterPage, unitOfWork);
        }

        /// <summary>
        /// Gets master page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="masterPageId">Identifies the master page to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Master page details (or null if master page not found).</returns>
        public MasterPage Read(long tenantId, long masterPageId, IUnitOfWork unitOfWork = null)
        {
            return _masterPageRepository.Read(tenantId, masterPageId, unitOfWork);
        }

        /// <summary>
        /// Gets master page zone details.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="masterPageId">Identifies the master page whose zone is returned.</param>
        /// <param name="masterPageZoneId">Identifies the master page zone to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Master page zone details (or null if master page zone not found).</returns>
        public MasterPageZone ReadZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null)
        {
            return _masterPageRepository.ReadZone(tenantId, masterPageId, masterPageZoneId, unitOfWork);
        }

        /// <summary>
        /// Gets admin master page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Master page details (or null if master page not found).</returns>
        public MasterPage ReadAdministration(long tenantId, IUnitOfWork unitOfWork = null)
        {
            return _masterPageRepository.ReadAdministration(tenantId, unitOfWork);
        }

        /// <summary>
        /// Ensure correct sort orders of zone elements. Ensure element types only specified for zones that are configurable.
        /// </summary>
        /// <param name="masterPageZone">Master page zone.</param>
        private void PrepareMasterPageZone(MasterPageZone masterPageZone)
        {
            // Only populate element types if master page zone admin type configurable
            if (masterPageZone.AdminType != MasterPageZoneAdminType.Configurable)
                masterPageZone.MasterPageZoneElementTypes.Clear();

            // Ensure correct zone element sort orders
            for (int zoneElementIndex = 0; zoneElementIndex < masterPageZone.MasterPageZoneElements.Count; zoneElementIndex++)
            {
                MasterPageZoneElement zoneElement = masterPageZone.MasterPageZoneElements[zoneElementIndex];
                zoneElement.SortOrder = zoneElementIndex;
                if (string.IsNullOrWhiteSpace(zoneElement.BeginRender))
                    zoneElement.BeginRender = null;
                if (string.IsNullOrWhiteSpace(zoneElement.EndRender))
                    zoneElement.EndRender = null;
            }
        }

        /// <summary>
        /// Update sort orders and set default values for new zones.
        /// </summary>
        /// <param name="masterPageZones">List of master page zones.</param>
        private void PrepareMasterPageZones(List<MasterPageZone> masterPageZones)
        {
            UpdateMasterPageZoneSortOrders(masterPageZones);
            foreach (MasterPageZone zone in masterPageZones)
            {
                if (zone.MasterPageZoneId == 0)
                {
                    // Set default values for zones that will be created
                    zone.AdminType = MasterPageZoneAdminType.Static;
                    zone.ContentType = MasterPageZoneContentType.Standard;
                    zone.BeginRender = null;
                    zone.EndRender = null;
                }
            }
        }

        /// <summary>
        /// Sets master page zone sort order according to position within collection.
        /// </summary>
        /// <param name="masterPageZones">Collection of master page zones.</param>
        private void UpdateMasterPageZoneSortOrders(List<MasterPageZone> masterPageZones)
        {
            for (int index = 0; index < masterPageZones.Count; index++)
                masterPageZones[index].SortOrder = index;
        }

        /// <summary>
        /// Empty image related fields if master page does not have image.
        /// </summary>
        /// <param name="masterPage">Master page.</param>
        private void PrepareMasterPage(MasterPage masterPage)
        {
            // If master page does not have image, set all image related fields null
            if (!masterPage.HasImage)
            {
                masterPage.ImageMinWidth = null;
                masterPage.ImageMinHeight = null;
                masterPage.ThumbnailImageWidth = null;
                masterPage.ThumbnailImageHeight = null;
                masterPage.ThumbnailImageResizeMode = null;
                masterPage.PreviewImageWidth = null;
                masterPage.PreviewImageHeight = null;
                masterPage.PreviewImageResizeMode = null;
            }
        }

        /// <summary>
        /// Updates master page.
        /// </summary>
        /// <param name="masterPage">The updated master page.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(MasterPage masterPage, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Do the page update
            try
            {
                // Prepare master page for update (e.g. set correct sort orders)
                PrepareMasterPage(masterPage);

                // Do the update
                _masterPageRepository.Update(masterPage, unitOfWork ?? localUnitOfWork);

                // Set specified master page as admin and ensure all other master pages belonging to the same tenant do not have master page set as admin.
                // There can only be 1 admin master page.
                if (masterPage.Administration)
                    _masterPageRepository.SetAdminMasterPage(masterPage.TenantId, masterPage.MasterPageId, unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
            }
            catch (ValidationErrorException)
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }

        /// <summary>
        /// Returns whether or not an element is in-use. An element is considered in-use if it exists on a page or master page. 
        /// </summary>
        /// <param name="tenantId">Tenant identifying a website.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>True if element on master page or page, false if not.</returns>
        private bool ElementInUse(long tenantId, long elementId, IUnitOfWork unitOfWork)
        {
            int count = _pageRepository.GetElementPageCount(tenantId, elementId, unitOfWork);
            if (count == 0)
                count = _masterPageRepository.GetElementMasterPageCount(tenantId, elementId, unitOfWork);
            return (count > 0);
        }

        /// <summary>
        /// Get elements that are found in the existing master page zone, but not in the updated master page zone. Following update of master page zone,
        /// these are the elements that may be deleted if they are no longer in-use.
        /// </summary>
        /// <param name="existingMasterPageZone">Existing master page zone.</param>
        /// <param name="masterPageZone">New master page zone.</param>
        /// <returns>Elements that may be redundant following update of master page zone.</returns>
        private HashSet<long> GetRemovedElementsForStaticOrConfigurableMasterPageZone(MasterPageZone existingMasterPageZone, MasterPageZone masterPageZone)
        {
            // Get identifiers of elements in master page zone as it currently stands
            HashSet<long> existingElements = new HashSet<long>();
            foreach (MasterPageZoneElement masterPageZoneElement in existingMasterPageZone.MasterPageZoneElements)
            {
                if (!existingElements.Contains(masterPageZoneElement.ElementId))
                    existingElements.Add(masterPageZoneElement.ElementId);
            }

            // Get identifiers of elements in updated master page zone
            HashSet<long> updatedElements = new HashSet<long>();
            foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
            {
                if (masterPageZoneElement.ElementId != 0 && !updatedElements.Contains(masterPageZoneElement.ElementId))
                    updatedElements.Add(masterPageZoneElement.ElementId);
            }

            // Get identifiers of elements that are no longer referenced by master page zone
            HashSet<long> removedElements = new HashSet<long>();
            foreach (long elementId in existingElements)
            {
                if (!updatedElements.Contains(elementId))
                    removedElements.Add(elementId);
            }

            // Return result
            return removedElements;
        }

        /// <summary>
        /// Gets identifiers of master page zone elements that are no longer required following update of editable master page zone.
        /// </summary>
        /// <param name="existingMasterPageZone">Existing master page zone details.</param>
        /// <param name="masterPageZone">New master page zone.</param>
        /// <returns>Master page zone elements that are redundant following update of master page zone.</returns>
        private HashSet<long> GetRemovedMasterPageZoneElementIds(MasterPageZone existingMasterPageZone, MasterPageZone masterPageZone)
        {
            // Get master page zone element identifiers in master page zone as it currently stands
            HashSet<long> existingMasterPageZoneElementIds = new HashSet<long>();
            foreach (MasterPageZoneElement masterPageZoneElement in existingMasterPageZone.MasterPageZoneElements)
            {
                if (!existingMasterPageZoneElementIds.Contains(masterPageZoneElement.MasterPageZoneElementId))
                    existingMasterPageZoneElementIds.Add(masterPageZoneElement.MasterPageZoneElementId);
            }

            // Get master page zone element identifiers in updated master page zone
            HashSet<long> updatedMasterPageZoneElementIds = new HashSet<long>();
            foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
            {
                if (!updatedMasterPageZoneElementIds.Contains(masterPageZoneElement.MasterPageZoneElementId))
                    updatedMasterPageZoneElementIds.Add(masterPageZoneElement.MasterPageZoneElementId);
            }

            // Get identifiers of master page zone elements that are no longer referenced by master page zone
            HashSet<long> removedMasterPageZoneElementIds = new HashSet<long>();
            foreach (long masterPageZoneElementId in existingMasterPageZoneElementIds)
            {
                if (!updatedMasterPageZoneElementIds.Contains(masterPageZoneElementId))
                    removedMasterPageZoneElementIds.Add(masterPageZoneElementId);
            }

            // Return result
            return removedMasterPageZoneElementIds;
        }

        /// <summary>
        /// Gets identifiers of master page zone elements that are new.
        /// </summary>
        /// <param name="existingMasterPageZone">Existing master page zone details.</param>
        /// <param name="masterPageZone">New master page zone.</param>
        /// <returns>Master page zone elements that exist in updated master page zone, but not in the old master page zone.</returns>
        private Dictionary<long, MasterPageZoneElement> GetNewMasterPageZoneElementIds(MasterPageZone existingMasterPageZone, MasterPageZone masterPageZone)
        {
            // Get master page zone element identifiers in master page zone as it was previously defined
            Dictionary<long, MasterPageZoneElement> existingMasterPageZoneElementIds = new Dictionary<long, MasterPageZoneElement>();
            foreach (MasterPageZoneElement masterPageZoneElement in existingMasterPageZone.MasterPageZoneElements)
            {
                if (!existingMasterPageZoneElementIds.ContainsKey(masterPageZoneElement.MasterPageZoneElementId))
                    existingMasterPageZoneElementIds.Add(masterPageZoneElement.MasterPageZoneElementId, masterPageZoneElement);
            }

            // Get master page zone element identifiers in new master page zone
            Dictionary<long, MasterPageZoneElement> updatedMasterPageZoneElementIds = new Dictionary<long, MasterPageZoneElement>();
            foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
            {
                if (!updatedMasterPageZoneElementIds.ContainsKey(masterPageZoneElement.MasterPageZoneElementId))
                    updatedMasterPageZoneElementIds.Add(masterPageZoneElement.MasterPageZoneElementId, masterPageZoneElement);
            }

            // Get identifiers of master page zone elements that are new
            Dictionary<long, MasterPageZoneElement> newMasterPageZoneElementIds = new Dictionary<long, MasterPageZoneElement>();
            foreach (KeyValuePair<long, MasterPageZoneElement> kvp in updatedMasterPageZoneElementIds)
            {
                if (!existingMasterPageZoneElementIds.ContainsKey(kvp.Key))
                    newMasterPageZoneElementIds.Add(kvp.Key, kvp.Value);
            }

            // Return result
            return newMasterPageZoneElementIds;
        }

        /// <summary>
        /// Gets identifiers of elements that are no longer required following update of editable master page zone.
        /// </summary>
        /// <param name="existingMasterPageZone">Existing master page zone details.</param>
        /// <param name="masterPageZone">New master page zone.</param>
        /// <param name="removedMasterPageZoneElementIds">Master page zone element identifiers that are no longer required following update of editable master page zone.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Elements that may be redundant following update of master page zone.</returns>
        private HashSet<long> GetRemovedElementsForEditableMasterPageZone(MasterPageZone existingMasterPageZone, MasterPageZone masterPageZone, HashSet<long> removedMasterPageZoneElementIds, IUnitOfWork unitOfWork)
        {
            // The potential list of elements to remove will include any elements referenced directly by master page zone element.
            // These elements can be retrieved by calling the equivalent method for static master page zones
            HashSet<long> removedElements = GetRemovedElementsForStaticOrConfigurableMasterPageZone(existingMasterPageZone, masterPageZone);

            // Finally, we need to get a list of the elements associated with the removed master page zone element identifiers
            List<long> elementIds = null;
            if (removedMasterPageZoneElementIds.Count == 0)
                elementIds = new List<long>();
            else
                elementIds = _masterPageRepository.ListPageElementsByMasterPageZoneElementIds(masterPageZone.TenantId, masterPageZone.MasterPageId, masterPageZone.MasterPageZoneId, removedMasterPageZoneElementIds.ToList(), unitOfWork);

            // Add these to the list of potentially redundant elements and we are done
            foreach (long elementId in elementIds)
                if (!removedElements.Contains(elementId))
                    removedElements.Add(elementId);

            // Return result
            return removedElements;
        }

        /// <summary>
        /// Deletes elements that are no longer in-use (i.e. not referenced by a master page or a page).
        /// </summary>
        /// <param name="tenantId">Identifies website.</param>
        /// <param name="elements">Element identifiers.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        private void DeleteElementsThatAreNoLongerInUse(long tenantId, HashSet<long> elements, IUnitOfWork unitOfWork)
        {
            foreach (long elementId in elements)
            {
                if (!ElementInUse(tenantId, elementId, unitOfWork))
                    _elementService.Delete(tenantId, elementId, unitOfWork);
            }
        }

        /// <summary>
        /// Deal with change of master page zone admin type from static to editable or configurable.
        /// </summary>
        /// <param name="existingMasterPageZone">Existing master page zone.</param>
        /// <param name="masterPageZone">Updated master page zone.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        private void ChangeAdminTypeFromStatic(MasterPageZone existingMasterPageZone, MasterPageZone masterPageZone, IUnitOfWork unitOfWork)
        {
            // Create page zones required by editable master page zones, that would not have existed for a static master page zone
            List<PageZone> pageZones = new List<PageZone>();
            List<Page> pages = _pageRepository.ListPagesByMasterPage(existingMasterPageZone.TenantId, existingMasterPageZone.MasterPageId, unitOfWork);
            foreach (Page page in pages)
            {
                pageZones.Add(new PageZone
                {
                    TenantId = existingMasterPageZone.TenantId,
                    PageId = page.PageId,
                    MasterPageId = existingMasterPageZone.MasterPageId,
                    MasterPageZoneId = existingMasterPageZone.MasterPageZoneId
                });
            }
            if (pageZones.Count > 0)
            {
                // Create page zones
                _pageRepository.CreatePageZones(existingMasterPageZone.TenantId, pageZones, unitOfWork);

                // Now get back created page zones, so that newly allocated page zone identifiers are populated
                pageZones = _pageRepository.ListPageZonesByMasterPageZoneId(existingMasterPageZone.TenantId, existingMasterPageZone.MasterPageId, existingMasterPageZone.MasterPageZoneId, unitOfWork);
            }

            // Create page zone elements required by editable master page zones, that would not have existed for a static master page zone
            List<PageZoneElement> pageZoneElements = new List<PageZoneElement>();
            foreach (PageZone pageZone in pageZones)
            {
                foreach (MasterPageZoneElement masterPageZoneElement in existingMasterPageZone.MasterPageZoneElements)
                {
                    long elementId = _elementService.Copy(existingMasterPageZone.TenantId, masterPageZoneElement.Element.ElementId, existingMasterPageZone.TenantId, masterPageZoneElement.Element.ElementTypeId, unitOfWork);
                    PageZoneElement pageZoneElement = new PageZoneElement
                    {
                        TenantId = existingMasterPageZone.TenantId,
                        PageId = pageZone.PageId,
                        PageZoneId = pageZone.PageZoneId,
                        ElementId = elementId
                    };
                    if (masterPageZone.AdminType == MasterPageZoneAdminType.Editable)
                    {
                        pageZoneElement.MasterPageId = masterPageZoneElement.MasterPageId;
                        pageZoneElement.MasterPageZoneId = masterPageZoneElement.MasterPageZoneId;
                        pageZoneElement.MasterPageZoneElementId = masterPageZoneElement.MasterPageZoneElementId;
                    }
                    if (masterPageZone.AdminType == MasterPageZoneAdminType.Configurable)
                    {
                        pageZoneElement.SortOrder = masterPageZoneElement.SortOrder;
                    }
                    pageZoneElements.Add(pageZoneElement);
                }
            }
            if (pageZoneElements.Count > 0)
                _pageRepository.CreatePageZoneElements(existingMasterPageZone.TenantId, pageZoneElements, unitOfWork);
        }

        /// <summary>
        /// Deal with change of master page zone admin type from editable or configurable to static.
        /// </summary>
        /// <param name="existingMasterPageZone">Existing master page zone.</param>
        /// <param name="masterPageZone">Updated master page zone.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        private void ChangeAdminTypeToStatic(MasterPageZone existingMasterPageZone, MasterPageZone masterPageZone, IUnitOfWork unitOfWork)
        {
            // Delete page zone elements that were required by editable or configurable master page zones, but no longer required for static master page zone
            List<long> elementIds = _masterPageRepository.ListPageElementsByMasterPageZone(existingMasterPageZone.TenantId, existingMasterPageZone.MasterPageId, existingMasterPageZone.MasterPageZoneId, unitOfWork);
            _pageRepository.DeletePageZoneElementsByMasterPageZone(existingMasterPageZone.TenantId, existingMasterPageZone.MasterPageId, existingMasterPageZone.MasterPageZoneId, unitOfWork);
            HashSet<long> removedElements = new HashSet<long>();
            foreach (long elementId in elementIds)
                removedElements.Add(elementId);
            if (removedElements.Count > 0)
                DeleteElementsThatAreNoLongerInUse(existingMasterPageZone.TenantId, removedElements, unitOfWork);

            // Delete page zones that were required by editable or configurable master page zones, but no longer required for static master page zone
            _pageRepository.DeletePageZonesByMasterPageZone(existingMasterPageZone.TenantId, existingMasterPageZone.MasterPageId, existingMasterPageZone.MasterPageZoneId, unitOfWork);
        }

        /// <summary>
        /// Deal with change of master page zone admin type from editable to configurable.
        /// </summary>
        /// <param name="existingMasterPageZone">Existing master page zone.</param>
        /// <param name="masterPageZone">Updated master page zone.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        private void ChangeAdminTypeFromEditableToConfigurable(MasterPageZone existingMasterPageZone, MasterPageZone masterPageZone, IUnitOfWork unitOfWork)
        {
            _pageRepository.UpdatePageZoneElementsForConfigurableAdminType(existingMasterPageZone.TenantId, existingMasterPageZone.MasterPageId, existingMasterPageZone.MasterPageZoneId, unitOfWork);
        }

        /// <summary>
        /// Deal with change of master page zone admin type from configurable to editable.
        /// </summary>
        /// <param name="existingMasterPageZone">Existing master page zone.</param>
        /// <param name="masterPageZone">Updated master page zone.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        private void ChangeAdminTypeFromConfigurableToEditable(MasterPageZone existingMasterPageZone, MasterPageZone masterPageZone, IUnitOfWork unitOfWork)
        {
            ChangeAdminTypeToStatic(existingMasterPageZone, masterPageZone, unitOfWork);
            ChangeAdminTypeFromStatic(existingMasterPageZone, masterPageZone, unitOfWork);
        }

        /// <summary>
        /// Process an admin type change.
        /// </summary>
        /// <param name="existingMasterPageZone">Existing master page zone.</param>
        /// <param name="masterPageZone">Updated master page zone.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        private void ProcessAdminTypeChange(MasterPageZone existingMasterPageZone, MasterPageZone masterPageZone, IUnitOfWork unitOfWork)
        {
            if (existingMasterPageZone.AdminType == MasterPageZoneAdminType.Static)
                ChangeAdminTypeFromStatic(existingMasterPageZone, masterPageZone, unitOfWork);
            else if (masterPageZone.AdminType == MasterPageZoneAdminType.Static)
                ChangeAdminTypeToStatic(existingMasterPageZone, masterPageZone, unitOfWork);
            else if (existingMasterPageZone.AdminType == MasterPageZoneAdminType.Editable && masterPageZone.AdminType == MasterPageZoneAdminType.Configurable)
                ChangeAdminTypeFromEditableToConfigurable(existingMasterPageZone, masterPageZone, unitOfWork);
            else if (existingMasterPageZone.AdminType == MasterPageZoneAdminType.Configurable && masterPageZone.AdminType == MasterPageZoneAdminType.Editable)
                ChangeAdminTypeFromConfigurableToEditable(existingMasterPageZone, masterPageZone, unitOfWork);
        }

        /// <summary>
        /// Creates any new elements that are required.
        /// </summary>
        /// <param name="masterPageZone">The updated master page zone.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        private void CreateNewElements(MasterPageZone masterPageZone, IUnitOfWork unitOfWork)
        {
            foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
            {
                if (masterPageZoneElement.ElementId == 0)
                {
                    IElementSettings element = _elementService.New(masterPageZone.TenantId, masterPageZoneElement.Element.ElementTypeId);
                    element.Name = masterPageZoneElement.Element.Name.Trim();
                    long elementId = _elementService.Create(element, unitOfWork);
                    masterPageZoneElement.ElementId = elementId;
                    masterPageZoneElement.Element.ElementId = elementId;
                }
            }
        }

        /// <summary>
        /// Updates single zone within a master page.
        /// </summary>
        /// <param name="masterPageZone">The updated master page zone.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdateZone(MasterPageZone masterPageZone, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Do the page update
            try
            {
                // Perform validation
                _masterPageValidator.ValidateUpdateZone(masterPageZone);

                // Prepare master page zone for update (e.g. set correct sort orders etc)
                PrepareMasterPageZone(masterPageZone);

                // Get zone as it currently stands
                MasterPageZone existingMasterPageZone = ReadZone(masterPageZone.TenantId, masterPageZone.MasterPageId, masterPageZone.MasterPageZoneId, unitOfWork ?? localUnitOfWork);

                // Check if master page zone admin type has changed?
                if (existingMasterPageZone.AdminType != masterPageZone.AdminType)
                    ProcessAdminTypeChange(existingMasterPageZone, masterPageZone, unitOfWork ?? localUnitOfWork);

                // Action performed depends on admin type of master zone
                HashSet<long> removedElements = null;
                if (masterPageZone.AdminType == MasterPageZoneAdminType.Static || masterPageZone.AdminType == MasterPageZoneAdminType.Configurable)
                {
                    // Get elements that may be removed
                    removedElements = GetRemovedElementsForStaticOrConfigurableMasterPageZone(existingMasterPageZone, masterPageZone);
                }
                else if (masterPageZone.AdminType == MasterPageZoneAdminType.Editable)
                {
                    // Get elements that may be removed and master page zone element identifiers that will be removed
                    HashSet<long> removedMasterPageZoneElementIds = GetRemovedMasterPageZoneElementIds(existingMasterPageZone, masterPageZone);
                    removedElements = GetRemovedElementsForEditableMasterPageZone(existingMasterPageZone, masterPageZone, removedMasterPageZoneElementIds, unitOfWork ?? localUnitOfWork);

                    // Finally, remove page zone elements that reference the master page zone element identifiers that will be removed
                    if (removedMasterPageZoneElementIds.Count > 0)
                        _masterPageRepository.DeletePageZoneElementsByMasterPageZoneElementIds(masterPageZone.TenantId, masterPageZone.MasterPageId, masterPageZone.MasterPageZoneId, removedMasterPageZoneElementIds.ToList(), unitOfWork ?? localUnitOfWork);
                }

                // Create any new elements
                CreateNewElements(masterPageZone, unitOfWork ?? localUnitOfWork);

                // Do the update
                _masterPageRepository.UpdateZone(masterPageZone, unitOfWork ?? localUnitOfWork);

                // Post update actions
                if (masterPageZone.AdminType == MasterPageZoneAdminType.Editable)
                {
                    // Get all of the page zones associated with the updated master page zone
                    List<PageZone> pageZones = _pageRepository.ListPageZonesByMasterPageZoneId(masterPageZone.TenantId, masterPageZone.MasterPageId, masterPageZone.MasterPageZoneId, unitOfWork ?? localUnitOfWork);

                    // Get identifiers of master page zone elements that are newly created
                    MasterPageZone newMasterPageZone = ReadZone(masterPageZone.TenantId, masterPageZone.MasterPageId, masterPageZone.MasterPageZoneId, unitOfWork ?? localUnitOfWork);
                    Dictionary<long, MasterPageZoneElement> newMasterPageZoneElements = GetNewMasterPageZoneElementIds(existingMasterPageZone, newMasterPageZone);

                    // Construct new page zone elements (including copies of elements specified in new master page zone elements)
                    List<PageZoneElement> pageZoneElements = new List<PageZoneElement>();
                    foreach (KeyValuePair<long, MasterPageZoneElement> kvp in newMasterPageZoneElements)
                    {
                        foreach (PageZone pageZone in pageZones)
                        {
                            long elementId = _elementService.Copy(kvp.Value.TenantId, kvp.Value.ElementId, kvp.Value.TenantId, kvp.Value.Element.ElementTypeId, unitOfWork ?? localUnitOfWork);
                            PageZoneElement pageZoneElement = new PageZoneElement
                            {
                                TenantId = pageZone.TenantId,
                                PageId = pageZone.PageId,
                                PageZoneId = pageZone.PageZoneId,
                                MasterPageId = kvp.Value.MasterPageId,
                                MasterPageZoneId = kvp.Value.MasterPageZoneId,
                                MasterPageZoneElementId = kvp.Value.MasterPageZoneElementId,
                                SortOrder = null,
                                ElementId = elementId
                            };
                            pageZoneElements.Add(pageZoneElement);
                        }
                    }

                    // Create page zone elements
                    if (pageZoneElements.Count > 0)
                        _pageRepository.CreatePageZoneElements(masterPageZone.TenantId, pageZoneElements, unitOfWork ?? localUnitOfWork);
                }

                // Remove elements if they are no longer in-use
                if (removedElements != null)
                    DeleteElementsThatAreNoLongerInUse(masterPageZone.TenantId, removedElements, unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
            }
            catch (ValidationErrorException)
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }

        /// <summary>
        /// Master page zones found in existing master page zones, but not in updated master page zones have been removed.
        /// </summary>
        /// <param name="existingMasterPageZones">Existing (old) master page zones.</param>
        /// <param name="masterPageZones">Updated (new) master page zones.</param>
        /// <returns>List of master page zones that have been removed.</returns>
        private List<MasterPageZone> GetRemovedMasterPageZones(List<MasterPageZone> existingMasterPageZones, List<MasterPageZone> masterPageZones)
        {
            // Get existing master page zones from updated zones list
            Dictionary<long, MasterPageZone> dict = new Dictionary<long, MasterPageZone>();
            foreach (MasterPageZone masterPageZone in masterPageZones)
                if (masterPageZone.MasterPageZoneId != 0) // Do not record master page zones that we are about to create
                    dict.Add(masterPageZone.MasterPageZoneId, masterPageZone);

            // Zones from list of existing master page zones that do not exist in the updated list of master page zones are to be deleted
            List<MasterPageZone> removedMasterPageZones = new List<MasterPageZone>();
            foreach (MasterPageZone masterPageZone in existingMasterPageZones)
                if (!dict.ContainsKey(masterPageZone.MasterPageZoneId))
                    removedMasterPageZones.Add(masterPageZone);
            return removedMasterPageZones;
        }

        /// <summary>
        /// From list of removed master page zones, gets identifiers of all elements that should be removed.
        /// </summary>
        /// <param name="removedMasterPageZones">List of removed master page zones.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Elements that may be deleted if they are no longer in-use following zone removal.</returns>
        public HashSet<long> GetRemovedElements(List<MasterPageZone> removedMasterPageZones, IUnitOfWork unitOfWork)
        {
            HashSet<long> removedElements = new HashSet<long>();
            foreach (MasterPageZone masterPageZone in removedMasterPageZones)
            {
                // Get elements that are directly associated with master page zone elements in deleted master page zone
                foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
                    if (!removedElements.Contains(masterPageZoneElement.ElementId))
                        removedElements.Add(masterPageZoneElement.ElementId);

                // Get elements that are on page zones associated with master page zone that is to be deleted
                List<long> elementIds = _masterPageRepository.ListPageElementsByMasterPageZone(masterPageZone.TenantId, masterPageZone.MasterPageId, masterPageZone.MasterPageZoneId, unitOfWork);
                foreach (long elementId in elementIds)
                    if (!removedElements.Contains(elementId))
                        removedElements.Add(elementId);
            }
            return removedElements;
        }

        /// <summary>
        /// Deletes page zone elements and page zones that are associated with specified master page zone.
        /// </summary>
        /// <param name="masterPageZone">Master page zone that is to be removed.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        private void DeleteZone(MasterPageZone masterPageZone, IUnitOfWork unitOfWork)
        {
            _masterPageRepository.DeletePageZoneElementsByMasterPageZone(masterPageZone.TenantId, masterPageZone.MasterPageId, masterPageZone.MasterPageZoneId, unitOfWork);
            _masterPageRepository.DeletePageZonesByMasterPageZone(masterPageZone.TenantId, masterPageZone.MasterPageId, masterPageZone.MasterPageZoneId, unitOfWork);
        }

        /// <summary>
        /// Adds, removes and re-orders master page zones.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="masterPageId">Identifier of master page whose zones are added, removed or re-ordered.</param>
        /// <param name="masterPageZones">Updated list of master page zones.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdateZones(long tenantId, long masterPageId, List<MasterPageZone> masterPageZones, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Do the page update
            try
            {
                // Update sort orders and set default values for new zones
                PrepareMasterPageZones(masterPageZones);

                // Get master page as it currently stands
                MasterPage existingMasterPage = Read(tenantId, masterPageId, unitOfWork ?? localUnitOfWork);

                // Get any zones that have been deleted
                List<MasterPageZone> removedMasterPageZones = GetRemovedMasterPageZones(existingMasterPage.MasterPageZones, masterPageZones);
                HashSet<long> removedElements = GetRemovedElements(removedMasterPageZones, unitOfWork ?? localUnitOfWork);
                foreach (MasterPageZone masterPageZone in removedMasterPageZones)
                    DeleteZone(masterPageZone, unitOfWork ?? localUnitOfWork);

                // Do the update
                _masterPageRepository.UpdateZones(tenantId, masterPageId, masterPageZones, unitOfWork ?? localUnitOfWork);

                // Remove elements if they are no longer in-use
                DeleteElementsThatAreNoLongerInUse(tenantId, removedElements, unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
            }
            catch (ValidationErrorException)
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }

        /// <summary>
        /// Get elements for master pages belonging to the specified tenant.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of elements.</returns>
        public IEnumerable<MasterPage> ListElementsByMasterPage(long tenantId, IUnitOfWork unitOfWork = null)
        {
            return _masterPageRepository.ListElementsByMasterPage(tenantId, unitOfWork);
        }
    }
}
