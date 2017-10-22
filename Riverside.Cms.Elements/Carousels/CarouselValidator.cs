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

namespace Riverside.Cms.Elements.Carousels
{
    public class CarouselValidator : ICarouselValidator
    {
        // Member variables
        private IImageAnalysisService _imageAnalysisService;
        private IUploadService _uploadService;

        // Constants
        private const int ImageMinWidth = 1366;
        private const int ImageMinHeight = 600;

        public CarouselValidator(IImageAnalysisService imageAnalysisService, IUploadService uploadService)
        {
            _imageAnalysisService = imageAnalysisService;
            _uploadService = uploadService;
        }

        public void ValidatePrepareImages(long tenantId, long elementId, CreateUploadModel model, string keyPrefix = null)
        {
            // Check that content type identifies an image
            UploadType uploadType = _uploadService.GetUploadType(model.ContentType);
            if (uploadType != UploadType.Image)
                throw new ValidationErrorException(new ValidationError(CarouselPropertyNames.Image, ElementResource.CarouselImageInvalidMessage, keyPrefix));

            // Check that supplied upload is an image
            Size? size = _imageAnalysisService.GetImageDimensions(model.Content);
            if (size == null)
                throw new ValidationErrorException(new ValidationError(CarouselPropertyNames.Image, ElementResource.CarouselImageInvalidMessage, keyPrefix));

            // Check image dimension constraints (minimum width and height)
            if (size.Value.Width < ImageMinWidth || size.Value.Height < ImageMinHeight)
                throw new ValidationErrorException(new ValidationError(CarouselPropertyNames.Image, string.Format(ElementResource.CarouselImageDimensionsInvalidMessage, ImageMinWidth, ImageMinHeight), keyPrefix));
        }

        public void ValidateSlide(CarouselSlide slide, string keyPrefix = null)
        {
            // Check that image specified
            if (slide.ImageTenantId == 0 || slide.ThumbnailImageUploadId == 0 || slide.PreviewImageUploadId == 0 || slide.ImageUploadId == 0)
                throw new ValidationErrorException(new ValidationError(CarouselPropertyNames.Image, ElementResource.CarouselImageRequiredMessage, keyPrefix));

            //  If page text specified, then a page must also be specified
            if (!string.IsNullOrWhiteSpace(slide.PageText) && slide.PageId == null)
                throw new ValidationErrorException(new ValidationError(CarouselPropertyNames.PageText, ElementResource.CarouselPageRequiredMessage, keyPrefix));
        }
    }
}
