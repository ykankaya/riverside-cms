using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Core.Uploads;
using Riverside.UI.Web;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Carousels
{
    public class CarouselService : ICarouselService, IAdvancedElementService, IUploadElementService
    {
        // Member variables
        private ICarouselRepository _carouselRepository;
        private ICarouselValidator _carouselValidator;
        private IImageAnalysisService _imageAnalysisService;
        private IPageService _pageService;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IUploadService _uploadService;
        private IWebHelperService _webHelperService;

        // Constants
        private const int ThumbnailImageWidth = 150;
        private const int ThumbnailImageHeight = 150;
        private const ResizeMode ThumbnailImageResizeMode = ResizeMode.Crop;
        private const int PreviewImageWidth = 1366;
        private const int PreviewImageHeight = 600;
        private const ResizeMode PreviewImageResizeMode = ResizeMode.MaintainAspect;

        public CarouselService(ICarouselRepository carouselRepository, ICarouselValidator carouselValidator, IImageAnalysisService imageAnalysisService, IPageService pageService, IUnitOfWorkFactory unitOfWorkFactory, IUploadService uploadService, IWebHelperService webHelperService)
        {
            _carouselRepository = carouselRepository;
            _carouselValidator = carouselValidator;
            _imageAnalysisService = imageAnalysisService;
            _pageService = pageService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _uploadService = uploadService;
            _webHelperService = webHelperService;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("aacb11a0-5532-47cb-aab9-939cee3d5175");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new CarouselSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<CarouselSettings, CarouselContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _carouselRepository.Create((CarouselSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _carouselRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _carouselRepository.Read((CarouselSettings)settings, unitOfWork);
        }

        public CarouselSlide ReadSlide(long tenantId, long elementId, long carouselSlideId, IUnitOfWork unitOfWork = null)
        {
            return _carouselRepository.ReadSlide(tenantId, elementId, carouselSlideId, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Begin work
            try
            {
                // Validate slides
                CarouselSettings carouselSettings = (CarouselSettings)settings;
                foreach (CarouselSlide slide in carouselSettings.Slides)
                    _carouselValidator.ValidateSlide(slide);

                // Get current carousel settings
                CarouselSettings currentCarouselSettings = (CarouselSettings)New(settings.TenantId);
                currentCarouselSettings.ElementId = settings.ElementId;
                _carouselRepository.Read(currentCarouselSettings, unitOfWork ?? localUnitOfWork);

                // Get slides to delete (i.e. slides that were in current settings, but not in the new settings)
                // Get slides with updated images
                List<CarouselSlide> slidesToDelete = new List<CarouselSlide>();
                List<CarouselSlide> slidesWithUpdatedImages = new List<CarouselSlide>();
                List<CarouselSlide> currentSlidesWithUpdatedImages = new List<CarouselSlide>();
                Dictionary<long, CarouselSlide> slidesById = carouselSettings.Slides.Where(s => s.CarouselSlideId != 0).GroupBy(s => s.CarouselSlideId).ToDictionary(u => u.Key, u => u.First());
                foreach (CarouselSlide currentSlide in currentCarouselSettings.Slides)
                {
                    if (!slidesById.ContainsKey(currentSlide.CarouselSlideId))
                    {
                        slidesToDelete.Add(currentSlide);
                    }
                    else
                    {
                        CarouselSlide slide = slidesById[currentSlide.CarouselSlideId];
                        if (slide.ImageUploadId != currentSlide.ImageUploadId)
                        {
                            slidesWithUpdatedImages.Add(slide);
                            currentSlidesWithUpdatedImages.Add(currentSlide);
                        }
                    }
                }

                // Get new slides
                List<CarouselSlide> slidesToCreate = carouselSettings.Slides.Where(s => s.CarouselSlideId == 0).ToList();

                // Commit new images
                slidesToCreate.AddRange(slidesWithUpdatedImages);
                foreach (CarouselSlide slide in slidesToCreate)
                {
                    _uploadService.Commit(slide.ImageTenantId, slide.ThumbnailImageUploadId, GetCarouselSlideStorageHierarchy(slide.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Commit(slide.ImageTenantId, slide.PreviewImageUploadId, GetCarouselSlideStorageHierarchy(slide.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Commit(slide.ImageTenantId, slide.ImageUploadId, GetCarouselSlideStorageHierarchy(slide.ElementId), unitOfWork ?? localUnitOfWork);
                }

                // Update database
                _carouselRepository.Update((CarouselSettings)settings, unitOfWork ?? localUnitOfWork);

                // Delete uploads that are no longer required
                slidesToDelete.AddRange(currentSlidesWithUpdatedImages);
                foreach (CarouselSlide currentSlide in slidesToDelete)
                {
                    _uploadService.Delete(currentSlide.ImageTenantId, currentSlide.ThumbnailImageUploadId, GetCarouselSlideStorageHierarchy(currentSlide.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Delete(currentSlide.ImageTenantId, currentSlide.PreviewImageUploadId, GetCarouselSlideStorageHierarchy(currentSlide.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Delete(currentSlide.ImageTenantId, currentSlide.ImageUploadId, GetCarouselSlideStorageHierarchy(currentSlide.ElementId), unitOfWork ?? localUnitOfWork);
                }

                // Commit work if local unit of work in place and return result
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
            finally
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Dispose();
            }
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Begin work
            try
            {
                // Get carousel settings
                CarouselSettings carouselSettings = (CarouselSettings)New(tenantId);
                carouselSettings.ElementId = elementId;
                _carouselRepository.Read(carouselSettings, unitOfWork ?? localUnitOfWork);

                // Delete carousel from underlying storage
                _carouselRepository.Delete(tenantId, elementId, unitOfWork ?? localUnitOfWork);

                // Delete slide images
                foreach (CarouselSlide slide in carouselSettings.Slides)
                {
                    _uploadService.Delete(slide.ImageTenantId, slide.ThumbnailImageUploadId, GetCarouselSlideStorageHierarchy(slide.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Delete(slide.ImageTenantId, slide.PreviewImageUploadId, GetCarouselSlideStorageHierarchy(slide.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Delete(slide.ImageTenantId, slide.ImageUploadId, GetCarouselSlideStorageHierarchy(slide.ElementId), unitOfWork ?? localUnitOfWork);
                }

                // Commit work if local unit of work in place and return result
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
            finally
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Dispose();
            }
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            // Get carousel content
            IDictionary<object, object> items = _webHelperService.GetItems();
            CarouselContent content = new CarouselContent { PartialViewName = "Carousel", Pages = new List<Page>(), Items = items };

            // Populate pages
            foreach (CarouselSlide slide in ((CarouselSettings)settings).Slides)
            {
                Page page = null;
                if (slide.PageId.HasValue)
                    page = _pageService.Read(slide.TenantId, slide.PageId.Value, unitOfWork);
                content.Pages.Add(page);
            }

            // Return result
            return content;
        }

        private List<string> GetCarouselSlideStorageHierarchy(long elementId)
        {
            return new List<string> { "elements", "carousels", elementId.ToString() };
        }

        public Upload ReadUpload(long tenantId, long elementId, long uploadId, string format, IUnitOfWork unitOfWork = null)
        {
            CarouselSlide slide = _carouselRepository.ReadSlide(tenantId, elementId, uploadId, unitOfWork);
            if (slide == null)
                return null;
            switch (format)
            {
                case "preview":
                    return (Image)_uploadService.Read(slide.ImageTenantId, slide.PreviewImageUploadId, GetCarouselSlideStorageHierarchy(elementId), unitOfWork);

                case "thumbnail":
                    return (Image)_uploadService.Read(slide.ImageTenantId, slide.ThumbnailImageUploadId, GetCarouselSlideStorageHierarchy(elementId), unitOfWork);

                default:
                    return null;
            }
        }

        public long PrepareImages(long tenantId, long elementId, CreateUploadModel model, IUnitOfWork unitOfWork = null)
        {
            throw new NotImplementedException();
        }

        public ImageUploadIds PrepareImages2(long tenantId, long elementId, CreateUploadModel model, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Begin work
            try
            {
                // Check that uploaded content is valid image
                _carouselValidator.ValidatePrepareImages(tenantId, elementId, model);

                // Create thumbnail model
                ResizeInfo thumbnailResizeInfo = new ResizeInfo
                {
                    Width = ThumbnailImageWidth,
                    Height = ThumbnailImageHeight,
                    ResizeMode = ThumbnailImageResizeMode
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
                    Width = PreviewImageWidth,
                    Height = PreviewImageHeight,
                    ResizeMode = PreviewImageResizeMode
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
    }
}
