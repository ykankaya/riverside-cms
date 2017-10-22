using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Elements.Resources;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Albums
{
    public class AlbumValidator : IAlbumValidator
    {
        private IImageAnalysisService _imageAnalysisService;
        private IUploadService _uploadService;

        private const int ImageMinWidth = 1200;
        private const int ImageMinHeight = 675;

        public AlbumValidator(IImageAnalysisService imageAnalysisService, IUploadService uploadService)
        {
            _imageAnalysisService = imageAnalysisService;
            _uploadService = uploadService;
        }

        public void ValidatePrepareImages(long tenantId, long elementId, CreateUploadModel model, string keyPrefix = null)
        {
            // Check that content type identifies an image
            UploadType uploadType = _uploadService.GetUploadType(model.ContentType);
            if (uploadType != UploadType.Image)
                throw new ValidationErrorException(new ValidationError(AlbumPropertyNames.Image, ElementResource.AlbumImageInvalidMessage, keyPrefix));

            // Check that supplied upload is an image
            Size? size = _imageAnalysisService.GetImageDimensions(model.Content);
            if (size == null)
                throw new ValidationErrorException(new ValidationError(AlbumPropertyNames.Image, ElementResource.AlbumImageInvalidMessage, keyPrefix));

            // Check image dimension constraints (minimum width and height)
            if (size.Value.Width < ImageMinWidth || size.Value.Height < ImageMinHeight)
                throw new ValidationErrorException(new ValidationError(AlbumPropertyNames.Image, string.Format(ElementResource.AlbumImageDimensionsInvalidMessage, ImageMinWidth, ImageMinHeight), keyPrefix));
        }

        public void ValidatePhoto(AlbumPhoto photo, string keyPrefix = null)
        {
            // Check that image specified
            if (photo.ImageTenantId == 0 || photo.ThumbnailImageUploadId == 0 || photo.PreviewImageUploadId == 0 || photo.ImageUploadId == 0)
                throw new ValidationErrorException(new ValidationError(AlbumPropertyNames.Image, ElementResource.AlbumImageRequiredMessage, keyPrefix));
        }
    }
}
