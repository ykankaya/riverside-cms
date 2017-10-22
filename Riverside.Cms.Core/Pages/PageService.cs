using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Webs;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// A service for managing pages.
    /// </summary>
    public class PageService : IPageService
    {
        // Member variables
        private IElementService _elementService;
        private IImageAnalysisService _imageAnalysisService;
        private IMasterPageRepository _masterPageRepository;
        private IMasterPageService _masterPageService;
        private IPageRepository _pageRepository;
        private IPageValidator _pageValidator;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IUploadService _uploadService;

        // Constants
        private const int MaxTagSizes = 10;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="elementService">For element interactions.</param>
        /// <param name="imageAnalysisService">Used to retrieve image properties from uploaded files and resize images.</param>
        /// <param name="masterPageRepository">Master page repository.</param>
        /// <param name="masterPageService">Master page service.</param>
        /// <param name="pageRepository">Page repository.</param>
        /// <param name="pageValidator">Page validator.</param>
        /// <param name="unitOfWorkFactory">Unit of work factory.</param>
        /// <param name="uploadService">For the management of uploads.</param>
        public PageService(IElementService elementService, IImageAnalysisService imageAnalysisService, IMasterPageRepository masterPageRepository, IMasterPageService masterPageService, IPageRepository pageRepository, IPageValidator pageValidator, IUnitOfWorkFactory unitOfWorkFactory, IUploadService uploadService)
        {
            _elementService = elementService;
            _imageAnalysisService = imageAnalysisService;
            _masterPageRepository = masterPageRepository;
            _masterPageService = masterPageService;
            _pageRepository = pageRepository;
            _pageValidator = pageValidator;
            _unitOfWorkFactory = unitOfWorkFactory;
            _uploadService = uploadService;
        }

        /// <summary>
        /// Searches pages.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are searched.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        public ISearchResult<Page> Search(long tenantId, ISearchParameters parameters, IUnitOfWork unitOfWork = null)
        {
            if (parameters.Search == null)
                parameters.Search = string.Empty;
            else
                parameters.Search = parameters.Search.Trim();
            if (parameters.PageSize == 0)
                parameters.PageSize = 10;
            return _pageRepository.Search(tenantId, parameters, unitOfWork);
        }

        /// <summary>
        /// Returns count of pages from specified page to root.
        /// </summary>
        /// <param name="page">The page whose count is returned.</param>
        /// <returns>Count of pages from specified page to root.</returns>
        private int GetPageDepth(Page page)
        {
            int count = 0;
            while (page != null)
            {
                page = page.ParentPage;
                count++;
            }
            return count;
        }

        /// <summary>
        /// Sets parent page references for the supplied enumeration of pages.
        /// </summary>
        /// <param name="pages">Enumeration of pages whose parent page property may be updated.</param>
        private void ConnectParentPages(IEnumerable<Page> pages)
        {
            Dictionary<long, Page> pagesById = new Dictionary<long, Page>();
            foreach (Page page in pages)
                pagesById.Add(page.PageId, page);
            foreach (Page page in pages)
                if (page.ParentPageId.HasValue && pagesById.ContainsKey(page.ParentPageId.Value))
                    page.ParentPage = pagesById[page.ParentPageId.Value];
        }

        /// <summary>
        /// Retrieves only those ancestor folders that match the ancestor conditions in a master page.
        /// </summary>
        /// <param name="masterPage">The master page (ancestor page ID and ancestor page level must both be specified).</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of folders where a page with the specified master can be creted.</returns>
        private List<Page> GetAncestorPages(MasterPage masterPage, IUnitOfWork unitOfWork)
        {
            // The page list that we will return and other variables
            long ancestorPageId = masterPage.AncestorPageId.Value;
            PageLevel ancestorPageLevel = masterPage.AncestorPageLevel.Value;
            int integerPageLevel = (int)ancestorPageLevel;
            List<Page> pages = new List<Page>();

            // List of pages is determined by ancestor page level
            switch (ancestorPageLevel)
            {
                case PageLevel.Parent:  // The straightforward case, parent must be ancestor page ID
                    pages.Add(Read(masterPage.TenantId, ancestorPageId, unitOfWork));
                    break;

                default: // For all other cases, we must return only those ancestor pages with the correct distance from the ancestor page specified
                    ISearchParameters parameters = new SearchParameters { PageIndex = 0, PageSize = 1000 }; // TODO: Need way to return all pages, not have some max bound upper limit
                    ISearchResult<Page> result = List(masterPage.TenantId, parameters, ancestorPageId, PageSortBy.Name, true, true, PageType.Folder, false, unitOfWork);
                    ConnectParentPages(result.Items);
                    foreach (Page page in result.Items)
                    {
                        if (GetPageDepth(page) == (integerPageLevel - 1))
                            pages.Add(page);
                    }
                    break;
            }

            // Return the result
            return pages;
        }

        /// <summary>
        /// Returns all folders where a page with the specified master can be created.
        /// </summary>
        /// <param name="masterPage">The master page.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of folders where pages can be created.</returns>
        public List<Page> ListMasterPageParentPages(MasterPage masterPage, IUnitOfWork unitOfWork = null)
        {
            // The list of parent folders
            List<Page> parentPages = null;

            // If no ancestor page or level specified, then any folder is valid
            if (masterPage.AncestorPageId == null || masterPage.AncestorPageLevel == null)
            {
                ISearchParameters parameters = new SearchParameters { PageIndex = 0, PageSize = 1000 }; // TODO: Need way to return all pages, not have some max bound upper limit
                ISearchResult<Page> result = List(masterPage.TenantId, parameters, null, PageSortBy.Name, true, true, PageType.Folder, false, unitOfWork);
                parentPages = result.Items.ToList();
            }

            // If ancestor page and level specified, must restrict choice of available folders
            if (masterPage.AncestorPageId.HasValue && masterPage.AncestorPageLevel.HasValue)
            {
                parentPages = GetAncestorPages(masterPage, unitOfWork);
            }

            // Return result
            return parentPages;
        }

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
        public ISearchResult<Page> List(long tenantId, ISearchParameters parameters, long? pageId, PageSortBy sortBy, bool sortAsc, bool recursive, PageType pageType, bool loadTags, IUnitOfWork unitOfWork = null)
        {
            return _pageRepository.List(tenantId, parameters, pageId, sortBy, sortAsc, recursive, pageType, loadTags, unitOfWork);
        }

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
        public ISearchResult<Page> ListTagged(long tenantId, ISearchParameters parameters, IList<Tag> tags, long? pageId, PageSortBy sortBy, bool sortAsc, bool recursive, PageType pageType, bool loadTags, IUnitOfWork unitOfWork = null)
        {
            return _pageRepository.ListTagged(tenantId, parameters, tags, pageId, sortBy, sortAsc, recursive, pageType, loadTags, unitOfWork);
        }

        /// <summary>
        /// Populates tag counts with a tag size between 1 and 11, indicating size of count relative to other tags.
        /// </summary>
        /// <param name="tagCounts">Tag counts.</param>
        private void CalculateTagSizes(List<TagCount> tagCounts)
        {
            // Now work out relative sizes of tags
            Dictionary<int, List<TagCount>> tagsByCount = new Dictionary<int, List<TagCount>>();
            foreach (TagCount tagCount in tagCounts)
            {
                if (!tagsByCount.ContainsKey(tagCount.Count))
                    tagsByCount.Add(tagCount.Count, new List<TagCount>());
                tagsByCount[tagCount.Count].Add(tagCount);
            }

            // Multiple counts?
            if (tagsByCount.Count > 1)
            {
                List<int> counts = tagsByCount.Select(kvp => kvp.Key).OrderBy(key => key).ToList();
                double sizeStep = (double)MaxTagSizes / (double)(counts.Count - 1);
                for (int index = 0; index < counts.Count; index++)
                {
                    List<TagCount> tagCountsForCount = tagsByCount[counts[index]];
                    foreach (TagCount tagCount in tagCountsForCount)
                        tagCount.Size = (int)Math.Round(((double)index) * sizeStep) + 1;
                }
            }

            // Single count
            if (tagsByCount.Count == 1)
            {
                List<TagCount> tagCountsForCount = tagsByCount.First().Value;
                foreach (TagCount tagCount in tagCountsForCount)
                    tagCount.Size = (int)Math.Round((double)MaxTagSizes / 2.0) + 1;
            }
        }

        /// <summary>
        /// Lists tags associated with page.
        /// </summary>
        /// <param name="tenantId">Identifies website whose page tags are listed.</param>
        /// <param name="pageId">Identifies the parent page whose child page tags are returned (set NULL to include root folder of website).</param>
        /// <param name="recursive">Set true to get tags of all child pages, false if only tags of direct descendants are required.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of tag counts.</returns>
        public List<TagCount> ListTags(long tenantId, long? pageId, bool recursive, IUnitOfWork unitOfWork = null)
        {
            // Get tag counts
            List<TagCount> tagCounts = _pageRepository.ListTags(tenantId, pageId, recursive, unitOfWork);

            // Calculate relative size of each tag
            CalculateTagSizes(tagCounts);

            // Return result
            return tagCounts;
        }

        /// <summary>
        /// Lists tags associated with page that are related to an existing list of tags.
        /// </summary>
        /// <param name="tenantId">Identifies website whose page tags are listed.</param>
        /// <param name="pageId">Identifies the parent page whose child page tags are returned (set NULL to include root folder of website).</param>
        /// <param name="tags">Tags must be related to these tags.</param>
        /// <param name="recursive">Set true to get tags of all child pages, false if only tags of direct descendants are required.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of tag counts.</returns>
        public List<TagCount> ListTaggedPageTags(long tenantId, long? pageId, IList<Tag> tags, bool recursive, IUnitOfWork unitOfWork = null)
        {
            // Get tag counts
            List<TagCount> tagCounts = _pageRepository.ListTaggedPageTags(tenantId, pageId, tags, recursive, unitOfWork);

            // Calculate relative size of each tag
            CalculateTagSizes(tagCounts);

            // Return result
            return tagCounts;
        }

        /// <summary>
        /// Gets tags with specified names. The number of tags returned may not match the number of names specified, if tags do not exist for some names.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="names">List of tag names.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of tags, identified by name.</returns>
        public List<Tag> ListNamedTags(long tenantId, List<string> names, IUnitOfWork unitOfWork = null)
        {
            return _pageRepository.ListNamedTags(tenantId, names, unitOfWork);
        }

        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name="tenantId">The website that created page will belong to.</param>
        /// <param name="parentPageId">Parent page (can be null if page has no parent - i.e. is home page, or if can be determined).</param>
        /// <param name="masterpageId">The master page that newly created page is based on.</param>
        /// <param name="pageInfo">If specified, used to override master page settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated page identifier.</returns>
        public long Create(long tenantId, long? parentPageId, long masterPageId, PageInfo pageInfo, IUnitOfWork unitOfWork = null)
        {
            // Multiple actions are performed during page creation, so we need a unit of work to perform rollback if any failures occur
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            try
            {
                // Get master page that will determine how new page is created
                MasterPage masterPage = _masterPageRepository.Read(tenantId, masterPageId, unitOfWork ?? localUnitOfWork);

                // Get valid parent pages
                List<Page> parentPages = null;
                if (masterPage.AncestorPageId.HasValue && masterPage.AncestorPageLevel.HasValue)
                    parentPages = ListMasterPageParentPages(masterPage, unitOfWork ?? localUnitOfWork);

                // Validate the page create request (including checking that all or none of the image upload properties are specified)
                _pageValidator.ValidateCreate(tenantId, parentPageId, pageInfo, parentPages);

                // Set parent page identifier?
                if (parentPageId == null && masterPage.AncestorPageId.HasValue && masterPage.AncestorPageLevel.HasValue)
                    parentPageId = parentPages[0].PageId;

                // Construct new page based on master page definition
                DateTime now = DateTime.UtcNow;
                Page page = new Page
                {
                    Created = now,
                    Description = pageInfo == null ? masterPage.PageDescription : pageInfo.Description,
                    MasterPageId = masterPageId,
                    Name = pageInfo == null ? masterPage.PageName : pageInfo.Name,
                    Occurred = masterPage.HasOccurred ? DateTime.UtcNow.Date : (DateTime?)null,
                    ParentPageId = parentPageId,
                    Updated = now,
                    TenantId = tenantId,
                    PageZones = new List<PageZone>(),
                    Tags = pageInfo == null ? new List<Tag>() : pageInfo.Tags,
                    ImageTenantId = pageInfo == null ? null : pageInfo.ImageTenantId,
                    ThumbnailImageUploadId = pageInfo == null ? null : pageInfo.ThumbnailImageUploadId,
                    PreviewImageUploadId = pageInfo == null ? null : pageInfo.PreviewImageUploadId,
                    ImageUploadId = pageInfo == null ? null : pageInfo.ImageUploadId
                };

                // Construct page zones
                foreach (MasterPageZone masterPageZone in masterPage.MasterPageZones)
                {
                    if (masterPageZone.AdminType != MasterPageZoneAdminType.Static)
                    {
                        PageZone pageZone = new PageZone
                        {
                            TenantId = tenantId,
                            MasterPageId = masterPageId,
                            MasterPageZoneId = masterPageZone.MasterPageZoneId,
                            PageZoneElements = new List<PageZoneElement>()
                        };
                        foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
                        {
                            long elementId = _elementService.Copy(tenantId, masterPageZoneElement.ElementId, tenantId, masterPageZoneElement.Element.ElementTypeId, unitOfWork ?? localUnitOfWork);
                            PageZoneElement pageZoneElement = new PageZoneElement
                            {
                                TenantId = tenantId,
                                ElementTypeId = masterPageZoneElement.Element.ElementTypeId,
                                ElementId = elementId,
                                Parent = pageZone
                            };
                            if (masterPageZone.AdminType == MasterPageZoneAdminType.Configurable)
                            {
                                pageZoneElement.SortOrder = masterPageZoneElement.SortOrder;
                            }
                            if (masterPageZone.AdminType == MasterPageZoneAdminType.Editable)
                            {
                                pageZoneElement.MasterPageId = masterPageId;
                                pageZoneElement.MasterPageZoneId = masterPageZone.MasterPageZoneId;
                                pageZoneElement.MasterPageZoneElementId = masterPageZoneElement.MasterPageZoneElementId;
                            }
                            pageZone.PageZoneElements.Add(pageZoneElement);
                        }
                        page.PageZones.Add(pageZone);
                    }
                }

                // Commit page images?
                if (page.ImageUploadId.HasValue)
                {
                    _uploadService.Commit(page.ImageTenantId.Value, page.ThumbnailImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                    _uploadService.Commit(page.ImageTenantId.Value, page.PreviewImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                    _uploadService.Commit(page.ImageTenantId.Value, page.ImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                }

                // Create page and return newly allocated page identifier
                page.PageId = _pageRepository.Create(page, unitOfWork ?? localUnitOfWork);

                // Update page tags?
                if (masterPage.Taggable)
                    _pageRepository.UpdateTags(page, unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place, then return newly allocated page identifier
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return page.PageId;
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
        /// Gets page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that page belongs to.</param>
        /// <param name="pageId">Identifies the page to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Page details (or null if page not found).</returns>
        public Page Read(long tenantId, long pageId, IUnitOfWork unitOfWork = null)
        {
            return _pageRepository.Read(tenantId, pageId, unitOfWork);
        }

        /// <summary>
        /// Gets home page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that home page belongs to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Home page details (or null if home page not found).</returns
        public Page ReadHome(long tenantId, IUnitOfWork unitOfWork = null)
        {
            return _pageRepository.ReadHome(tenantId, unitOfWork);
        }

        /// <summary>
        /// Gets page hierarchy (only primary page information is returned by this call, no zones or elements).
        /// </summary>
        /// <param name="tenantId">Website that page belongs to.</param>
        /// <param name="pageId">The page whose hierarchy is returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Page hierarchy, with parent pages populated (or null if page not found).</returns>
        public Page ReadHierarchy(long tenantId, long pageId, IUnitOfWork unitOfWork = null)
        {
            return _pageRepository.ReadHierarchy(tenantId, pageId, unitOfWork);
        }

        /// <summary>
        /// Gets thumbnail image associated with page (null if no thumbnail image).
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="pageId">Page identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Thumbnail image.</returns>
        public Image ReadThumbnailImage(long tenantId, long pageId, IUnitOfWork unitOfWork = null)
        {
            Image image = null;
            Page page = Read(tenantId, pageId, unitOfWork);
            if (page.ThumbnailImageUploadId.HasValue)
                image = (Image)_uploadService.Read(page.ImageTenantId.Value, page.ThumbnailImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork);
            return image;
        }

        /// <summary>
        /// Gets preview image associated with page (null if no preview image).
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="pageId">Page identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Preview image.</returns>
        public Image ReadPreviewImage(long tenantId, long pageId, IUnitOfWork unitOfWork = null)
        {
            Image image = null;
            Page page = Read(tenantId, pageId, unitOfWork);
            if (page.PreviewImageUploadId.HasValue)
                image = (Image)_uploadService.Read(page.ImageTenantId.Value, page.PreviewImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork);
            return image;
        }

        /// <summary>
        /// From a single uploaded file, creates thumbnail, preview and original images in underlying storage that may be associated with a page.
        /// </summary>
        /// <param name="tenantId">Website that page belongs to.</param>
        /// <param name="masterPageId">Master page containing image upload rules.</param>
        /// <param name="model">Image upload.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Identifiers of newly created thumbnail, preview and source images.</returns>
        public ImageUploadIds PrepareImages(long tenantId, long masterPageId, CreateUploadModel model, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Begin work
            try
            {
                // Check that master page allows page images and that uploaded content is valid image
                ValidatePrepareImagesResult result = _pageValidator.ValidatePrepareImages(tenantId, masterPageId, model);

                // Create thumbnail model
                ResizeInfo thumbnailResizeInfo = new ResizeInfo
                {
                    Width = result.MasterPage.ThumbnailImageWidth.Value,
                    Height = result.MasterPage.ThumbnailImageHeight.Value,
                    ResizeMode = result.MasterPage.ThumbnailImageResizeMode.Value
                };
                byte[] thumbnailContent = _imageAnalysisService.ResizeImage(model.Content, thumbnailResizeInfo);
                CreateUploadModel thumbnailModel = new CreateUploadModel
                {
                    Content = thumbnailContent,
                    ContentType = model.ContentType,
                    Name = model.Name,
                    TenantId = model.TenantId
                };

                // Create preview model
                ResizeInfo previewResizeInfo = new ResizeInfo
                {
                    Width = result.MasterPage.PreviewImageWidth.Value,
                    Height = result.MasterPage.PreviewImageHeight.Value,
                    ResizeMode = result.MasterPage.PreviewImageResizeMode.Value
                };
                byte[] previewContent = _imageAnalysisService.ResizeImage(model.Content, previewResizeInfo);
                CreateUploadModel previewModel = new CreateUploadModel
                {
                    Content = previewContent,
                    ContentType = model.ContentType,
                    Name = model.Name,
                    TenantId = model.TenantId
                };

                // Create uploads for thumbnail, preview and original image
                long thumbnailImageUploadId = _uploadService.Create(thumbnailModel, unitOfWork ?? localUnitOfWork);
                long previewImageUploadId = _uploadService.Create(previewModel, unitOfWork ?? localUnitOfWork);
                long imageUploadId = _uploadService.Create(model, unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place and return result
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return new ImageUploadIds { ThumbnailImageUploadId = thumbnailImageUploadId, PreviewImageUploadId = previewImageUploadId, ImageUploadId = imageUploadId };
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
        /// Gets storage hierarchy for page images.
        /// </summary>
        /// <returns>Storage hierarchy.</returns>
        public List<string> GetPageImageStorageHierarchy()
        {
            return new List<string> { "pages", "images" };
        }

        /// <summary>
        /// Updates a page (not including zones and elements).
        /// </summary>
        /// <param name="page">Updated page details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(Page page, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Do the page update
            try
            {
                // Set updated date to now
                page.Updated = DateTime.UtcNow;

                // Perform validation (including checking that all or none of the image upload properties are specified)
                _pageValidator.ValidateUpdate(page);

                // Get current page
                Page currentPage = _pageRepository.Read(page.TenantId, page.PageId, unitOfWork ?? localUnitOfWork);

                // Master page determines what can and can't be updated
                MasterPage masterPage = _masterPageRepository.Read(page.TenantId, page.MasterPageId, unitOfWork ?? localUnitOfWork);

                // Commit page images?
                if (page.ImageUploadId.HasValue && currentPage.ImageUploadId != page.ImageUploadId)
                {
                    _uploadService.Commit(page.ImageTenantId.Value, page.ThumbnailImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                    _uploadService.Commit(page.ImageTenantId.Value, page.PreviewImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                    _uploadService.Commit(page.ImageTenantId.Value, page.ImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                }

                // Update page details
                _pageRepository.Update(page, unitOfWork ?? localUnitOfWork);

                // Update page tags?
                if (masterPage.Taggable)
                    _pageRepository.UpdateTags(page, unitOfWork ?? localUnitOfWork);

                // Delete old page images?
                if (currentPage.ImageUploadId.HasValue && currentPage.ImageUploadId != page.ImageUploadId)
                {
                    _uploadService.Delete(currentPage.ImageTenantId.Value, currentPage.ThumbnailImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                    _uploadService.Delete(currentPage.ImageTenantId.Value, currentPage.PreviewImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                    _uploadService.Delete(currentPage.ImageTenantId.Value, currentPage.ImageUploadId.Value, GetPageImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                }

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
        /// Updates a configurable page zone. Creates new elements and removes old elements. Re-orders existing elements.
        /// </summary>
        /// <param name="tenantId">Website that page belongs to.</param>
        /// <param name="pageId">Page identifier.</param>
        /// <param name="pageZoneId">Page zone identifier.</param>
        /// <param name="pageZoneElements">New page zone contents.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdateZone(long tenantId, long pageId, long pageZoneId, List<PageZoneElementInfo> pageZoneElements, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Do the page update
            try
            {
                // Get page, master page and page zone that we administering
                Page page = Read(tenantId, pageId, unitOfWork ?? localUnitOfWork);
                MasterPage masterPage = _masterPageService.Read(tenantId, page.MasterPageId, unitOfWork ?? localUnitOfWork);
                PageZone pageZone = page.PageZones.Where(z => z.PageZoneId == pageZoneId).FirstOrDefault();

                // Perform validation
                _pageValidator.ValidateUpdateZone(masterPage, page, pageZoneId, pageZoneElements);

                // Construct new list of page zone elements
                List<PageZoneElement> newPageZoneElements = new List<PageZoneElement>();
                foreach (PageZoneElementInfo info in pageZoneElements)
                {
                    // Create new page zone element?
                    if (info.PageZoneElementId == 0)
                    {
                        IElementSettings element = _elementService.New(tenantId, info.ElementTypeId);
                        element.Name = info.Name;
                        long elementId = _elementService.Create(element, unitOfWork ?? localUnitOfWork);
                        newPageZoneElements.Add(new PageZoneElement
                        {
                            TenantId = tenantId,
                            PageId = pageId,
                            PageZoneId = pageZoneId,
                            PageZoneElementId = 0,
                            SortOrder = info.SortOrder,
                            ElementTypeId = info.ElementTypeId,
                            ElementId = elementId,
                            Element = element
                        });
                    }

                    // Update existing page zone element?
                    if (info.PageZoneElementId != 0)
                    {
                        IElementSettings element = _elementService.New(tenantId, info.ElementTypeId);
                        element.Name = info.Name;
                        PageZoneElement pageZoneElement = pageZone.PageZoneElements.Where(e => e.PageZoneElementId == info.PageZoneElementId).First();
                        pageZoneElement.SortOrder = info.SortOrder;
                        pageZoneElement.Element = element;
                        newPageZoneElements.Add(pageZoneElement);
                    }
                }

                // Do the page zone element update
                _pageRepository.UpdatePageZoneElements(tenantId, pageId, pageZoneId, newPageZoneElements, unitOfWork ?? localUnitOfWork);

                // Get elements that might be removed
                List<ElementKeyValue> elementsToDelete = new List<ElementKeyValue>();
                foreach (PageZoneElement pageZoneElement in pageZone.PageZoneElements)
                {
                    PageZoneElementInfo pageZoneElementInfo = pageZoneElements.Where(e => e.PageZoneElementId == pageZoneElement.PageZoneElementId).FirstOrDefault();
                    if (pageZoneElementInfo == null)
                        elementsToDelete.Add(new ElementKeyValue { ElementId = pageZoneElement.ElementId, ElementTypeId = pageZoneElement.ElementTypeId });
                }

                // Delete elements if no longer in use
                foreach (ElementKeyValue elementKeyValue in elementsToDelete)
                {
                    if (!ElementInUse(tenantId, elementKeyValue.ElementId, unitOfWork ?? localUnitOfWork))
                        _elementService.Delete(tenantId, elementKeyValue.ElementTypeId, elementKeyValue.ElementId, unitOfWork ?? localUnitOfWork);
                }

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
