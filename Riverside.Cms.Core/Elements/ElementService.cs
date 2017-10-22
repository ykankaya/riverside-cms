using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Service for managing elements.
    /// </summary>
    public class ElementService : IElementService
    {
        // Member variables
        private IElementRepository  _elementRepository;
        private IElementFactory     _elementFactory;
        private IUnitOfWorkFactory  _unitOfWorkFactory;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="elementRepository">Element repository.</param>
        /// <param name="elementFactory">Element factory.</param>
        /// <param name="unitOfWorkFactory">Unit of work factory.</param>
        public ElementService(IElementRepository elementRepository, IElementFactory elementFactory, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _elementRepository = elementRepository;
            _elementFactory    = elementFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Creates a new instance of a type of element settings.
        /// </summary>
        /// <param name="tenantId">Identifies the tenant that newly created element settings belong to.</param>
        /// <param name="elementTypeId">The type of the element whose instance is created and returned.</param>
        /// <returns>Newly created element settings instance.</returns>
        public IElementSettings New(long tenantId, Guid elementTypeId)
        {
            return  _elementFactory.GetElementService(elementTypeId).New(tenantId);
        }

        /// <summary>
        /// Creates and returns strongly typed element info instance, populated with supplied element settings and content.
        /// </summary>
        /// <param name="settings">Element settings.</param>
        /// <param name="content">Element content.</param>
        /// <returns>An element info object containing settings and content.</returns>
        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return _elementFactory.GetElementService(settings.ElementTypeId).NewInfo(settings, content);
        }

        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <param name="settings">New element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated element identifier.</returns>
        public long Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            try
            {
                IAdvancedElementService customElementService = (IAdvancedElementService)_elementFactory.GetElementService(settings.ElementTypeId);
                ICustomElementValidator customElementValidator = _elementFactory.GetElementValidator(settings.ElementTypeId);
                IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
                try
                {
                    if (customElementValidator != null)
                        customElementValidator.ValidateCreate(settings, unitOfWork ?? localUnitOfWork);
                    settings.ElementId = _elementRepository.Create(settings, unitOfWork ?? localUnitOfWork);
                    if (customElementService != null)
                        customElementService.Create(settings, unitOfWork ?? localUnitOfWork);
                    if (localUnitOfWork != null)
                        localUnitOfWork.Commit();
                    return settings.ElementId;
                }
                catch
                {
                    if (localUnitOfWork != null)
                        localUnitOfWork.Rollback();
                    throw;
                }
                finally
                {
                    if (localUnitOfWork != null)
                        localUnitOfWork.Dispose();
                }
            }
            catch (ValidationErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }

        /// <summary>
        /// Creates new element based on existing element.
        /// </summary>
        /// <param name="sourceTenantId">The source tenant.</param>
        /// <param name="sourceElementId">Identifies element instance to copy.</param>
        /// <param name="destTenantId">The destination tenant.</param>
        /// <param name="sourceElementTypeId">The type of the element that is copied (destination will be same type).</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated element identifier.</returns>
        public long Copy(long sourceTenantId, long sourceElementId, long destTenantId, Guid sourceElementTypeId, IUnitOfWork unitOfWork = null)
        {
            try
            {
                IAdvancedElementService customElementService = (IAdvancedElementService)_elementFactory.GetElementService(sourceElementTypeId);
                IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
                try
                {
                    long destElementId = _elementRepository.Copy(sourceTenantId, sourceElementId, destTenantId, unitOfWork ?? localUnitOfWork);
                    customElementService.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork ?? localUnitOfWork);
                    if (localUnitOfWork != null)
                        localUnitOfWork.Commit();
                    return destElementId;
                }
                catch
                {
                    if (localUnitOfWork != null)
                        localUnitOfWork.Rollback();
                    throw;
                }
                finally
                {
                    if (localUnitOfWork != null)
                        localUnitOfWork.Dispose();
                }
            }
            catch (ValidationErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }

        /// <summary>
        /// Gets element details.
        /// </summary>
        /// <param name="tenantId">Identifies the tenant that element belongs to.</param>
        /// <param name="elementTypeId">The type of the element.</param>
        /// <param name="elementId">Identifies the element to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element settings (or null if element not found).</returns>
        public IElementSettings Read(long tenantId, Guid elementTypeId, long elementId, IUnitOfWork unitOfWork = null)
        {
            // Construct instance of element settings
            IBasicElementService customElementService = _elementFactory.GetElementService(elementTypeId);
            IElementSettings settings = customElementService.New(tenantId);

            // Populate element ID
            settings.ElementId = elementId;

            // Populate for advanced elements
            if (customElementService is IAdvancedElementService)
            {
                // Populate element with common settings
                _elementRepository.Read(settings, unitOfWork);

                // Populate with custom settings
                ((IAdvancedElementService)customElementService).Read(settings, unitOfWork);
            }

            // Return fully populated element
            return settings;
        }

        /// <summary>
        /// Reads primary element details.
        /// </summary>
        /// <param name="tenantId">Tenant that element associated with.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="loadCustomSettings">Set false to retrieve only those settings common to all elements. Set true to retrieve settings common to all elements and element specific settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element setings (null if element not found).</returns>
        public IElementSettings Read(long tenantId, long elementId, bool loadCustomSettings, IUnitOfWork unitOfWork = null)
        {
            // Get core element details
            IElementSettings elementSettings = _elementRepository.Read(tenantId, elementId, unitOfWork);
            if (elementSettings == null)
                return null;

            // Now we know the type of element requested, we can get custom element settings
            IBasicElementService customElementService = _elementFactory.GetElementService(elementSettings.ElementTypeId);
            IElementSettings customElementSettings = customElementService.New(elementSettings.TenantId);
            customElementSettings.ElementId = elementSettings.ElementId;
            customElementSettings.Name = elementSettings.Name;

            // If "advanced" element, populate custom settings
            if (loadCustomSettings && customElementService is IAdvancedElementService)
                ((IAdvancedElementService)customElementService).Read(customElementSettings, unitOfWork);

            // Return fully populated element settings
            return customElementSettings;
        }

        /// <summary>
        /// Updates an element's details.
        /// </summary>
        /// <param name="settings">Updated element details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            try
            {
                IAdvancedElementService customElementService = (IAdvancedElementService)_elementFactory.GetElementService(settings.ElementTypeId);
                ICustomElementValidator customElementValidator = _elementFactory.GetElementValidator(settings.ElementTypeId);
                if (customElementValidator != null)
                    customElementValidator.ValidateUpdate(settings, unitOfWork);
                if (customElementService != null)
                    customElementService.Update(settings, unitOfWork);
            }
            catch (ValidationErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }

        /// <summary>
        /// Deletes an element.
        /// </summary>
        /// <param name="tenantId">The tenant that element to delete belongs to.</param>
        /// <param name="elementId">Identifies the element to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            IElementSettings elementSettings = Read(tenantId, elementId, false, unitOfWork);
            if (elementSettings != null)
                Delete(tenantId, elementSettings.ElementTypeId, elementId, unitOfWork);
        }

        /// <summary>
        /// Deletes an element.
        /// </summary>
        /// <param name="tenantId">The tenant that element to delete belongs to.</param>
        /// <param name="elementTypeId">Identifies the type of element to delete.</param>
        /// <param name="elementId">Identifies the element to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, Guid elementTypeId, long elementId, IUnitOfWork unitOfWork = null)
        {
            try
            {
                IAdvancedElementService customElementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
                ICustomElementValidator customElementValidator = _elementFactory.GetElementValidator(elementTypeId);
                IUnitOfWork localUnitOfWork = (unitOfWork == null && customElementService != null) ? _unitOfWorkFactory.CreateUnitOfWork() : null;
                try
                {
                    if (customElementValidator != null)
                        customElementValidator.ValidateDelete(tenantId, elementTypeId, elementId, unitOfWork ?? localUnitOfWork);
                    if (customElementService != null)
                        customElementService.Delete(tenantId, elementId, unitOfWork ?? localUnitOfWork);
                    _elementRepository.Delete(tenantId, elementId, unitOfWork ?? localUnitOfWork);
                    if (localUnitOfWork != null)
                        localUnitOfWork.Commit();
                }
                catch
                {
                    if (localUnitOfWork != null)
                        localUnitOfWork.Rollback();
                    throw;
                }
                finally
                {
                    if (localUnitOfWork != null)
                        localUnitOfWork.Dispose();
                }
            }
            catch (ValidationErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }

        /// <summary>
        /// Retrieves dynamic element content.
        /// </summary>
        /// <param name="settings">Contains element settings.</param>
        /// <param name="pageContext">Page context.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element content.</returns>
        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            IBasicElementService customElementService = _elementFactory.GetElementService(settings.ElementTypeId);
            return customElementService.GetContent(settings, pageContext, unitOfWork);
        }

        /// <summary>
        /// For the list of supplied element key values, this method works out which elements are to do with site navigation and then
        /// registers the navigation pages with those elements. In this way, elements such as nav bars can be populated with pages.
        /// </summary>
        /// <param name="tenantId">The tenant whose navigation elements are updated.</param>
        /// <param name="elementKeyValues">List of element key values.</param>
        /// <param name="navigationPages">Pages that are to be added to site navigation elements.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void AddNavigationPages(long tenantId, List<ElementKeyValue> elementKeyValues, List<Page> navigationPages, IUnitOfWork unitOfWork = null)
        {
            foreach (ElementKeyValue elementKeyValue in elementKeyValues)
            {
                IBasicElementService customElementService = _elementFactory.GetElementService(elementKeyValue.ElementTypeId);
                if (customElementService is INavigationElementService)
                    ((INavigationElementService)customElementService).AddNavigationPages(tenantId, elementKeyValue.ElementId, navigationPages, unitOfWork);
            }
        }

        /// <summary>
        /// Gets all registered element types. 
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of element types.</returns>
        public IEnumerable<ElementType> ListTypes(IUnitOfWork unitOfWork = null)
        {
            return _elementRepository.ListTypes(unitOfWork);
        }

        /// <summary>
        /// Get list of all elements belonging to a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of elements.</returns>
        public IEnumerable<IElementSettings> ListElements(long tenantId, IUnitOfWork unitOfWork = null)
        {
            return _elementRepository.ListElements(tenantId, unitOfWork);
        }
    }
}
