using System.Drawing;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Elements.Resources;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Html
{
    public class HtmlValidator : IHtmlValidator
    {
        private IImageAnalysisService _imageAnalysisService;
        private IUploadService _uploadService;

        public HtmlValidator(IImageAnalysisService imageAnalysisService, IUploadService uploadService)
        {
            _imageAnalysisService = imageAnalysisService;
            _uploadService = uploadService;
        }

        public Size ValidatePrepareImages(long tenantId, long masterPageId, CreateUploadModel model, string keyPrefix = null)
        {
            // Check that content type identifies an image
            UploadType uploadType = _uploadService.GetUploadType(model.ContentType);
            if (uploadType != UploadType.Image)
                throw new ValidationErrorException(new ValidationError(HtmlPropertyNames.Image, ElementResource.HtmlImageInvalidMessage, keyPrefix));

            // Check that supplied upload is an image
            Size? size = _imageAnalysisService.GetImageDimensions(model.Content);
            if (size == null)
                throw new ValidationErrorException(new ValidationError(HtmlPropertyNames.Image, ElementResource.HtmlImageInvalidMessage, keyPrefix));

            // Return result
            return size.Value;
        }
    }
}
