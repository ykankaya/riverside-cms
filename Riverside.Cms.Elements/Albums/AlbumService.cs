using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Albums
{
    public class AlbumService : IAlbumService, IAdvancedElementService, IUploadElementService
    {
        private IAlbumRepository _albumRepository;
        private IAlbumValidator _albumValidator;
        private IImageAnalysisService _imageAnalysisService;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IUploadService _uploadService;

        private const int ThumbnailImageWidth = 300;
        private const int ThumbnailImageHeight = 300;
        private const ResizeMode ThumbnailImageResizeMode = ResizeMode.Crop;
        private const int PreviewImageWidth = 1200;
        private const int PreviewImageHeight = 675;
        private const ResizeMode PreviewImageResizeMode = ResizeMode.MaintainAspect;

        public AlbumService(IAlbumRepository albumRepository, IAlbumValidator albumValidator, IImageAnalysisService imageAnalysisService, IUnitOfWorkFactory unitOfWorkFactory, IUploadService uploadService)
        {
            _albumRepository = albumRepository;
            _albumValidator = albumValidator;
            _imageAnalysisService = imageAnalysisService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _uploadService = uploadService;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("b539d2a4-52ae-40d5-b366-e42447b93d15");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new AlbumSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<AlbumSettings, ElementContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _albumRepository.Create((AlbumSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _albumRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _albumRepository.Read((AlbumSettings)settings, unitOfWork);
        }

        public AlbumPhoto ReadPhoto(long tenantId, long elementId, long albumPhotoId, IUnitOfWork unitOfWork = null)
        {
            return _albumRepository.ReadPhoto(tenantId, elementId, albumPhotoId, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Begin work
            try
            {
                // Validate slides
                AlbumSettings albumSettings = (AlbumSettings)settings;
                foreach (AlbumPhoto photo in albumSettings.Photos)
                    _albumValidator.ValidatePhoto(photo);

                // Get current album settings
                AlbumSettings currentAlbumSettings = (AlbumSettings)New(settings.TenantId);
                currentAlbumSettings.ElementId = settings.ElementId;
                _albumRepository.Read(currentAlbumSettings, unitOfWork ?? localUnitOfWork);

                // Get photos to delete (i.e. photos that were in current settings, but not in the new settings)
                // Get photos with updated images
                List<AlbumPhoto> photosToDelete = new List<AlbumPhoto>();
                List<AlbumPhoto> photosWithUpdatedImages = new List<AlbumPhoto>();
                List<AlbumPhoto> currentPhotosWithUpdatedImages = new List<AlbumPhoto>();
                Dictionary<long, AlbumPhoto> photosById = albumSettings.Photos.Where(p => p.AlbumPhotoId != 0).GroupBy(s => s.AlbumPhotoId).ToDictionary(u => u.Key, u => u.First());
                foreach (AlbumPhoto currentPhoto in currentAlbumSettings.Photos)
                {
                    if (!photosById.ContainsKey(currentPhoto.AlbumPhotoId))
                    {
                        photosToDelete.Add(currentPhoto);
                    }
                    else
                    {
                        AlbumPhoto photo = photosById[currentPhoto.AlbumPhotoId];
                        if (photo.ImageUploadId != currentPhoto.ImageUploadId)
                        {
                            photosWithUpdatedImages.Add(photo);
                            currentPhotosWithUpdatedImages.Add(currentPhoto);
                        }
                    }
                }

                // Get new photos
                List<AlbumPhoto> photosToCreate = albumSettings.Photos.Where(s => s.AlbumPhotoId == 0).ToList();

                // Commit new images
                photosToCreate.AddRange(photosWithUpdatedImages);
                foreach (AlbumPhoto photo in photosToCreate)
                {
                    _uploadService.Commit(photo.ImageTenantId, photo.ThumbnailImageUploadId, GetAlbumPhotoStorageHierarchy(photo.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Commit(photo.ImageTenantId, photo.PreviewImageUploadId, GetAlbumPhotoStorageHierarchy(photo.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Commit(photo.ImageTenantId, photo.ImageUploadId, GetAlbumPhotoStorageHierarchy(photo.ElementId), unitOfWork ?? localUnitOfWork);
                }

                // Update database
                _albumRepository.Update((AlbumSettings)settings, unitOfWork ?? localUnitOfWork);

                // Delete uploads that are no longer required
                photosToDelete.AddRange(currentPhotosWithUpdatedImages);
                foreach (AlbumPhoto currentPhoto in photosToDelete)
                {
                    _uploadService.Delete(currentPhoto.ImageTenantId, currentPhoto.ThumbnailImageUploadId, GetAlbumPhotoStorageHierarchy(currentPhoto.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Delete(currentPhoto.ImageTenantId, currentPhoto.PreviewImageUploadId, GetAlbumPhotoStorageHierarchy(currentPhoto.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Delete(currentPhoto.ImageTenantId, currentPhoto.ImageUploadId, GetAlbumPhotoStorageHierarchy(currentPhoto.ElementId), unitOfWork ?? localUnitOfWork);
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
                // Get album settings
                AlbumSettings albumSettings = (AlbumSettings)New(tenantId);
                albumSettings.ElementId = elementId;
                _albumRepository.Read(albumSettings, unitOfWork ?? localUnitOfWork);

                // Delete album from underlying storage
                _albumRepository.Delete(tenantId, elementId, unitOfWork ?? localUnitOfWork);

                // Delete photo images
                foreach (AlbumPhoto photo in albumSettings.Photos)
                {
                    _uploadService.Delete(photo.ImageTenantId, photo.ThumbnailImageUploadId, GetAlbumPhotoStorageHierarchy(photo.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Delete(photo.ImageTenantId, photo.PreviewImageUploadId, GetAlbumPhotoStorageHierarchy(photo.ElementId), unitOfWork ?? localUnitOfWork);
                    _uploadService.Delete(photo.ImageTenantId, photo.ImageUploadId, GetAlbumPhotoStorageHierarchy(photo.ElementId), unitOfWork ?? localUnitOfWork);
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
            // Get album content
            ElementContent content = new ElementContent { PartialViewName = "Album" };

            // Return result
            return content;
        }

        private List<string> GetAlbumPhotoStorageHierarchy(long elementId)
        {
            return new List<string> { "elements", "albums", elementId.ToString() };
        }

        public Upload ReadUpload(long tenantId, long elementId, long uploadId, string format, IUnitOfWork unitOfWork = null)
        {
            AlbumPhoto photo = _albumRepository.ReadPhoto(tenantId, elementId, uploadId, unitOfWork);
            if (photo == null)
                return null;
            switch (format)
            {
                case "preview":
                    return (Image)_uploadService.Read(photo.ImageTenantId, photo.PreviewImageUploadId, GetAlbumPhotoStorageHierarchy(elementId), unitOfWork);

                case "thumbnail":
                    return (Image)_uploadService.Read(photo.ImageTenantId, photo.ThumbnailImageUploadId, GetAlbumPhotoStorageHierarchy(elementId), unitOfWork);

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
                _albumValidator.ValidatePrepareImages(tenantId, elementId, model);

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
