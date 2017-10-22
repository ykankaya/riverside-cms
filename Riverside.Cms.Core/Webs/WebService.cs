using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Templates;
using Riverside.Cms.Core.Tenants;
using Riverside.Cms.Core.Webs;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Webs
{
    /// <summary>
    /// Service for web management.
    /// </summary>
    public class WebService : IWebService
    {
        // Member variables
        private IDomainRepository _domainRepository;
        private IElementService _elementService;
        private IMasterPageRepository _masterPageRepository;
        private IPageService _pageService;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private ITemplateRepository _templateRepository;
        private ITenantRepository _tenantRepository;
        private IWebRepository _webRepository;
        private IWebValidator _webValidator;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="domainRepository">Domain repository.</param>
        /// <param name="elementService">Element service.</param>
        /// <param name="masterPageRepository">Master page repository.</param>
        /// <param name="pageService">Page service.</param>
        /// <param name="unitOfWorkFactory">Unit of work factory.</param>
        /// <param name="templateRepository">Template repository.</param>
        /// <param name="tenantRepository">Used to get new tenant identifier when website created.</param>
        /// <param name="webRepository">Web repository.</param>
        /// <param name="webValidator">Validates create, update and delete actions.</param>
        public WebService(IDomainRepository domainRepository, IElementService elementService, IMasterPageRepository masterPageRepository, IPageService pageService, IUnitOfWorkFactory unitOfWorkFactory, ITemplateRepository templateRepository, ITenantRepository tenantRepository, IWebRepository webRepository, IWebValidator webValidator)
        {
            _domainRepository = domainRepository;
            _elementService = elementService;
            _masterPageRepository = masterPageRepository;
            _pageService = pageService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _templateRepository = templateRepository;
            _tenantRepository = tenantRepository;
            _webRepository = webRepository;
            _webValidator = webValidator;
        }

        /// <summary>
        /// Searches websites.
        /// </summary>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        public ISearchResult<Web> Search(ISearchParameters parameters, IUnitOfWork unitOfWork = null)
        {
            if (parameters.Search == null)
                parameters.Search = string.Empty;
            else
                parameters.Search = parameters.Search.Trim();
            if (parameters.PageSize == 0)
                parameters.PageSize = 10;
            return _webRepository.Search(parameters, unitOfWork);
        }

        /// <summary>
        /// Gets master page from a template page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="page">The page that is about to be created. Contains hierarchy of pages created so far.</param>
        /// <param name="templatePage">Template page.</param>
        /// <param name="templateElements">Maintains dictionary of template elements and their corresponding master element copies.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        private MasterPage GetMasterPageFromTemplatePage(long tenantId, Page page, TemplatePage templatePage, Dictionary<ElementKeyValue, ElementKeyValue> templateElements, IUnitOfWork unitOfWork)
        {
            // Get ancestor page ID?
            long? ancestorPageId = null;
            if (templatePage.AncestorPageLevel != null)
            {
                PageLevel ancestorPageLevel = templatePage.AncestorPageLevel.Value;
                int numericPageLevel = (int)ancestorPageLevel;
                while (numericPageLevel > 0)
                {
                    ancestorPageId = page.ParentPageId;
                    page = page.ParentPage;
                    numericPageLevel--;
                }
            }

            // Create master page from template page
            MasterPage masterPage = new MasterPage
            {
                Administration = templatePage.Administration,
                AncestorPageId = ancestorPageId,
                AncestorPageLevel = templatePage.AncestorPageLevel,
                Creatable = templatePage.Creatable,
                Deletable = templatePage.Deletable,
                HasOccurred = templatePage.HasOccurred,
                HasImage = templatePage.HasImage,
                ThumbnailImageWidth = templatePage.ThumbnailImageWidth,
                ThumbnailImageHeight = templatePage.ThumbnailImageHeight,
                ThumbnailImageResizeMode = templatePage.ThumbnailImageResizeMode,
                PreviewImageWidth = templatePage.PreviewImageWidth,
                PreviewImageHeight = templatePage.PreviewImageHeight,
                PreviewImageResizeMode = templatePage.PreviewImageResizeMode,
                ImageMinWidth = templatePage.ImageMinWidth,
                ImageMinHeight = templatePage.ImageMinHeight,
                Name = templatePage.Name,
                PageName = templatePage.PageName,
                PageDescription = templatePage.PageDescription,
                PageType = templatePage.PageType,
                Taggable = templatePage.Taggable,
                TenantId = tenantId,
                BeginRender = templatePage.BeginRender,
                EndRender = templatePage.EndRender,
                MasterPageZones = new List<MasterPageZone>()
            };

            // Create master page zones from template page zones
            foreach (TemplatePageZone templatePageZone in templatePage.TemplatePageZones)
            {
                // Create master page zone
                MasterPageZone masterPageZone = new MasterPageZone
                {
                    Name = templatePageZone.Name,
                    AdminType = templatePageZone.AdminType,
                    ContentType = templatePageZone.ContentType,
                    TenantId = tenantId,
                    BeginRender = templatePageZone.BeginRender,
                    EndRender = templatePageZone.EndRender,
                    SortOrder = templatePageZone.SortOrder,
                    MasterPageZoneElementTypes = new List<MasterPageZoneElementType>(),
                    MasterPageZoneElements = new List<MasterPageZoneElement>()
                };
                masterPage.MasterPageZones.Add(masterPageZone);

                // Create master page zone element types from template page zone element types
                for (int index = 0; index < templatePageZone.TemplatePageZoneElementTypes.Count; index++)
                {
                    TemplatePageZoneElementType templatePageZoneElementType = templatePageZone.TemplatePageZoneElementTypes[index];
                    masterPageZone.MasterPageZoneElementTypes.Add(new MasterPageZoneElementType
                    {
                        TenantId = tenantId,
                        ElementTypeId = templatePageZoneElementType.ElementTypeId
                    });
                }

                // Create master page zone elements from template page zone elements
                for (int index = 0; index < templatePageZone.TemplatePageZoneElements.Count; index++)
                {
                    // Template elements are copied to create master elements
                    TemplatePageZoneElement templatePageZoneElement = templatePageZone.TemplatePageZoneElements[index];
                    ElementKeyValue templateElementKeyValue = new ElementKeyValue
                    {
                        ElementTypeId = templatePageZoneElement.ElementTypeId,
                        ElementId = templatePageZoneElement.ElementId
                    };

                    // Where template elements are re-used, so master element copies should be re-used
                    if (!templateElements.ContainsKey(templateElementKeyValue))
                    {
                        long masterElementId = _elementService.Copy(templatePage.TenantId, templateElementKeyValue.ElementId, tenantId, templateElementKeyValue.ElementTypeId, unitOfWork);
                        templateElements.Add(templateElementKeyValue, new ElementKeyValue { ElementTypeId = templateElementKeyValue.ElementTypeId, ElementId = masterElementId });
                    }

                    // Get master element key value
                    ElementKeyValue masterElementKeyValue = templateElements[templateElementKeyValue];
                    masterPageZone.MasterPageZoneElements.Add(new MasterPageZoneElement
                    {
                        ElementId = masterElementKeyValue.ElementId,
                        Element = new ElementSettings { TenantId = tenantId, ElementId = masterElementKeyValue.ElementId, ElementTypeId = masterElementKeyValue.ElementTypeId },
                        SortOrder = index,
                        TenantId = tenantId,
                        Parent = masterPageZone,
                        BeginRender = templatePageZoneElement.BeginRender,
                        EndRender = templatePageZoneElement.EndRender
                    });
                }
            }

            // Return the result
            return masterPage;
        }

        /// <summary>
        /// Recursive method builds website master pages and pages.
        /// </summary>
        /// <param name="tenantId">Identifies website being built.</param>
        /// <param name="page">Used to store identifier of page that will be created.</param>
        /// <param name="templatePage">Template page.</param>
        /// <param name="templateElements">Maintains dictionary of template elements and their corresponding master element copies.</param>
        /// <param name="navigationPages">List of pages that should be shown on site navigation.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        private void BuildWebsite(long tenantId, Page page, TemplatePage templatePage, Dictionary<ElementKeyValue, ElementKeyValue> templateElements, List<Page> navigationPages, IUnitOfWork unitOfWork)
        {
            // Get master page from template page
            MasterPage masterPage = GetMasterPageFromTemplatePage(tenantId, page, templatePage, templateElements, unitOfWork);

            // Create master page
            long masterPageId = _masterPageRepository.Create(masterPage, unitOfWork);

            // We do not create page instances of administration master pages - these are created on the fly as needed
            if (!masterPage.Administration)
            {
                // Create page based on master page 
                page.PageId = _pageService.Create(tenantId, page.ParentPageId, masterPageId, null, unitOfWork);
                page.Name = masterPage.PageName;
                if (templatePage.ShowOnNavigation)
                    navigationPages.Add(page);
            }

            // Recursively create child pages
            foreach (TemplatePage childTemplatePage in templatePage.ChildTemplatePages)
            {
                Page childPage = new Page { ParentPageId = page.PageId, ParentPage = page, TenantId = tenantId };
                BuildWebsite(tenantId, childPage, childTemplatePage, templateElements, navigationPages, unitOfWork);
            }
        }

        /// <summary>
        /// Create a new website from a template.
        /// </summary>
        /// <param name="templateTenantId">Identifies the template that will be used to create website.</param>
        /// <param name="web">New website details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated website identifier.</returns>
        public long Create(long templateTenantId, Web web, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one so that website creation tasks can all be rolled back in the case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            try
            {
                // Check that website (and domain) do not already exist and can be created
                _webValidator.ValidateCreate(web);

                // Get template that determines initial website structure
                Template template = _templateRepository.Read(templateTenantId, true, unitOfWork ?? localUnitOfWork);

                // Get tenant identifier for new website
                DateTime now = DateTime.UtcNow;
                Tenant tenant = new Tenant { Created = now, Updated = now };
                web.TenantId = _tenantRepository.Create(tenant, unitOfWork ?? localUnitOfWork);

                // Populate web from template
                web.CreateUserEnabled = template.CreateUserEnabled;
                web.UserHasImage = template.UserHasImage;
                web.UserThumbnailImageWidth = template.UserThumbnailImageWidth;
                web.UserThumbnailImageHeight = template.UserThumbnailImageHeight;
                web.UserThumbnailImageResizeMode = template.UserThumbnailImageResizeMode;
                web.UserPreviewImageWidth = template.UserPreviewImageWidth;
                web.UserPreviewImageHeight = template.UserPreviewImageHeight;
                web.UserPreviewImageResizeMode = template.UserPreviewImageResizeMode;
                web.UserImageMinWidth = template.UserImageMinWidth;
                web.UserImageMinHeight = template.UserImageMinHeight;

                // Create web
                _webRepository.Create(web, unitOfWork ?? localUnitOfWork);

                // Create domain
                web.Domains[0].TenantId = web.TenantId;
                long domainId = _domainRepository.Create(web.Domains[0], unitOfWork ?? localUnitOfWork);

                // Build website based on template
                Dictionary<ElementKeyValue, ElementKeyValue> templateElements = new Dictionary<ElementKeyValue, ElementKeyValue>();
                List<Page> navigationPages = new List<Page>();
                Page page = new Page { TenantId = web.TenantId };
                BuildWebsite(web.TenantId, page, template.Page, templateElements, navigationPages, unitOfWork ?? localUnitOfWork);

                // Update navigation elements with navigation pages
                _elementService.AddNavigationPages(web.TenantId, templateElements.Select(kvp => kvp.Value).ToList(), navigationPages, unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place, then return newly allocated website identifier
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return web.TenantId;
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
            finally
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Dispose();
            }
        }

        /// <summary>
        /// Gets website details.
        /// </summary>
        /// <param name="tenantId">Identifies website whose details are returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Website details (or null if website not found).</returns>
        public Web Read(long tenantId, IUnitOfWork unitOfWork = null)
        {
            return _webRepository.Read(tenantId, unitOfWork);
        }

        /// <summary>
        /// Updates a website's details.
        /// </summary>
        /// <param name="web">Updated website details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(Web web, IUnitOfWork unitOfWork = null)
        {
            try
            {
                // Perform validation
                _webValidator.ValidateUpdate(web);

                // Update website details
                _webRepository.Update(web, unitOfWork);
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
        /// Update theme options.
        /// </summary>
        /// <param name="tenantId">Identifies website whose theme options are updated.</param>
        /// <param name="fontOption">Font option.</param>
        /// <param name="colourOption">Colour option.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdateThemeOptions(long tenantId, string fontOption, string colourOption, IUnitOfWork unitOfWork = null)
        {
            if (fontOption == string.Empty)
                fontOption = null;
            if (colourOption == string.Empty)
                colourOption = null;
            _webRepository.UpdateThemeOptions(tenantId, fontOption, colourOption, unitOfWork);
        }

        /// <summary>
        /// Deletes a website.
        /// </summary>
        /// <param name="tenantId">Identifies the website to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one so that website and domain deletion tasks can both be rolled back in the case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            try
            {
                // Delete domains and website
                _domainRepository.DeleteByTenant(tenantId, unitOfWork ?? localUnitOfWork);
                _webRepository.Delete(tenantId, unitOfWork ?? localUnitOfWork);

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
    }
}
