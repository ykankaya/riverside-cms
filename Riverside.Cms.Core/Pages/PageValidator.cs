using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Provides validation for page actions.
    /// </summary>
    public class PageValidator : IPageValidator
    {
        // Member variables
        private IImageAnalysisService _imageAnalysisService;
        private IMasterPageRepository _masterPageRepository;
        private IModelValidator _modelValidator;
        private IPageRepository _pageRepository;
        private IUploadService _uploadService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="imageAnalysisService">Used to verify generic upload is an image.</param>
        /// <param name="masterPageRepository">Master page repository.</param>
        /// <param name="modelValidator">Model validator.</param>
        /// <param name="pageRepository">Page repository.</param>
        /// <param name="uploadService">Provides access to upload services.</param>
        public PageValidator(IImageAnalysisService imageAnalysisService, IMasterPageRepository masterPageRepository, IModelValidator modelValidator, IPageRepository pageRepository, IUploadService uploadService)
        {
            _imageAnalysisService = imageAnalysisService;
            _masterPageRepository = masterPageRepository;
            _modelValidator = modelValidator;
            _pageRepository = pageRepository;
            _uploadService = uploadService;
        }

        /// <summary>
        /// Validates create page model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="parentPageId">Parent page (can be null if page has no parent - i.e. is home page, or if can be determined).</param>
        /// <param name="pageInfo">If specified, used to override master page settings.</param>
        /// <param name="parentPages">If master has ancestor page specified, will contain list of valid parent pages.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateCreate(long tenantId, long? parentPageId, PageInfo pageInfo, List<Page> parentPages, string keyPrefix = null)
        {
            // Do stock model validation of page info, if it is specified
            if (pageInfo != null)
                _modelValidator.Validate(pageInfo, keyPrefix);

            // Image values must all be null or all specified
            if (pageInfo != null)
            {
                bool allHaveValue = pageInfo.ImageTenantId.HasValue && pageInfo.ThumbnailImageUploadId.HasValue && pageInfo.PreviewImageUploadId.HasValue && pageInfo.ImageUploadId.HasValue;
                bool noneHaveValue = !pageInfo.ImageTenantId.HasValue && !pageInfo.ThumbnailImageUploadId.HasValue && !pageInfo.PreviewImageUploadId.HasValue && !pageInfo.ImageUploadId.HasValue;
                if (!(allHaveValue || noneHaveValue))
                    throw new ValidationErrorException(new ValidationError(PagePropertyNames.Image, PageResource.ImageInvalidMessage, keyPrefix));
            }

            // If ancestor page specified, check that parent page is valid
            if (parentPages != null)
            {
                // If parent page not specified and valid parent page count does not equal one, then we can't automatically assign a parent page identifier
                // and this needs to be flagged as a validation error
                if (parentPageId == null & parentPages.Count != 1)
                    throw new ValidationErrorException(new ValidationError(PagePropertyNames.Parent, PageResource.ParentRequiredMessage, keyPrefix));

                // If parent page identifier specified, but it is not one of the valid parent pages, then throw a validation error
                if (parentPageId != null && parentPages.Where(p => p.PageId == parentPageId.Value).FirstOrDefault() == null)
                    throw new ValidationErrorException(new ValidationError(PagePropertyNames.Parent, PageResource.ParentInvalidMessage, keyPrefix));
            }
        }

        /// <summary>
        /// Validates update page model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="page">Updated page details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateUpdate(Page page, string keyPrefix = null)
        {
            // Do stock model validation
            _modelValidator.Validate(page, keyPrefix);

            // Image values must all be null or all specified
            bool allHaveValue = page.ImageTenantId.HasValue && page.ThumbnailImageUploadId.HasValue && page.PreviewImageUploadId.HasValue && page.ImageUploadId.HasValue;
            bool noneHaveValue = !page.ImageTenantId.HasValue && !page.ThumbnailImageUploadId.HasValue && !page.PreviewImageUploadId.HasValue && !page.ImageUploadId.HasValue;
            if (!(allHaveValue || noneHaveValue))
                throw new ValidationErrorException(new ValidationError(PagePropertyNames.Image, PageResource.ImageInvalidMessage, keyPrefix));
        }

        /// <summary>
        /// Validates page, master page and file upload details before images can be prepared for a page.
        /// </summary>
        /// <param name="tenantId">Website that page belongs to.</param>
        /// <param name="masterPageId">Master page containing image upload rules.</param>
        /// <param name="model">Uploaded image.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        /// <returns>Useful information retrieved during validation.</returns>
        public ValidatePrepareImagesResult ValidatePrepareImages(long tenantId, long masterPageId, CreateUploadModel model, string keyPrefix = null)
        {
            // Check that master page associated with page allows images
            MasterPage masterPage = _masterPageRepository.Read(tenantId, masterPageId);
            if (!masterPage.HasImage)
                throw new ValidationErrorException(new ValidationError(PagePropertyNames.Image, PageResource.ImageNotAllowedMessage, keyPrefix));

            // Check that content type identifies an image
            UploadType uploadType = _uploadService.GetUploadType(model.ContentType);
            if (uploadType != UploadType.Image)
                throw new ValidationErrorException(new ValidationError(PagePropertyNames.Image, PageResource.ImageInvalidMessage, keyPrefix));

            // Check that supplied upload is an image
            Size? size = _imageAnalysisService.GetImageDimensions(model.Content);
            if (size == null)
                throw new ValidationErrorException(new ValidationError(PagePropertyNames.Image, PageResource.ImageInvalidMessage, keyPrefix));

            // Check image dimension constraints (minimum width and height)
            if (size.Value.Width < masterPage.ImageMinWidth.Value || size.Value.Height < masterPage.ImageMinHeight.Value)
                throw new ValidationErrorException(new ValidationError(PagePropertyNames.Image, string.Format(PageResource.ImageDimensionsInvalidMessage, masterPage.ImageMinWidth.Value, masterPage.ImageMinHeight.Value), keyPrefix));

            // Return result
            return new ValidatePrepareImagesResult { MasterPage = masterPage, Size = size.Value };
        }

        /// <summary>
        /// Validates update of page zone.
        /// </summary>
        /// <param name="masterPage">Master page associated with page whose zone is being updated.</param>
        /// <param name="page">Page whose zone is being updated.</param>
        /// <param name="pageZoneId">Identifier of page zone being updated.</param>
        /// <param name="pageZoneElements">Determines the content in a page zone.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateUpdateZone(MasterPage masterPage, Page page, long pageZoneId, List<PageZoneElementInfo> pageZoneElements, string keyPrefix = null)
        {
            // Check that at least one item of content is specified
            if (pageZoneElements == null || pageZoneElements.Count == 0)
                throw new ValidationErrorException(new ValidationError(null, PageResource.PageZoneElementsRequiredMessage));

            // Check page zone exists
            PageZone pageZone = page.PageZones.Where(z => z.PageZoneId == pageZoneId).FirstOrDefault();
            if (pageZone == null)
                throw new ValidationErrorException(new ValidationError(null, PageResource.PageZoneInvalidMessage));

            // Check master page zone exists
            MasterPageZone masterPageZone = masterPage.MasterPageZones.Where(z => z.MasterPageZoneId == pageZone.MasterPageZoneId).FirstOrDefault();
            if (masterPageZone == null)
                throw new ValidationErrorException(new ValidationError(null, PageResource.PageZoneInvalidMessage));

            // Check that master page zone is configurable
            if (masterPageZone.AdminType != MasterPageZoneAdminType.Configurable)
                throw new ValidationErrorException(new ValidationError(null, PageResource.PageZoneInvalidMessage));

            // Check that element types in update request match allowed element types in zone configuration
            Dictionary<Guid, MasterPageZoneElementType> elementTypesById = masterPageZone.MasterPageZoneElementTypes.GroupBy(t => t.ElementTypeId).ToDictionary(t => t.Key, t => t.First());
            foreach (PageZoneElementInfo pageZoneElementInfo in pageZoneElements)
            {
                if (!elementTypesById.ContainsKey(pageZoneElementInfo.ElementTypeId))
                    throw new ValidationErrorException(new ValidationError(null, PageResource.PageZoneInvalidMessage));
            }

            // Check that element names are all specified
            foreach (PageZoneElementInfo pageZoneElementInfo in pageZoneElements)
            {
                if (string.IsNullOrWhiteSpace(pageZoneElementInfo.Name))
                    throw new ValidationErrorException(new ValidationError(null, PageResource.PageZoneElementNameRequiredMessage));
                if (pageZoneElementInfo.Name.Trim().Length > ElementLengths.NameMaxLength)
                    throw new ValidationErrorException(new ValidationError(null, string.Format(PageResource.PageZoneElementNameMaxLengthMessage, "name", ElementLengths.NameMaxLength)));
                pageZoneElementInfo.Name = pageZoneElementInfo.Name.Trim();
            }

            // Check that all page zone elements specified are found in page zone
            foreach (PageZoneElementInfo pageZoneElementInfo in pageZoneElements)
            {
                if (pageZoneElementInfo.PageZoneElementId != 0)
                {
                    PageZoneElement pageZoneElement = pageZone.PageZoneElements.Where(e => e.PageZoneElementId == pageZoneElementInfo.PageZoneElementId).FirstOrDefault();
                    if (pageZoneElement == null)
                        throw new ValidationErrorException(new ValidationError(null, PageResource.PageZoneElementInvalidMessage));
                }
            }
        }
    }
}