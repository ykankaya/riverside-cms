using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Validates file uploads.
    /// </summary>
    public class UploadValidator : IUploadValidator
    {
        // Member variables
        private IImageAnalysisService _imageAnalysisService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="imageAnalysisService">Image analysis service.</param>
        public UploadValidator(IImageAnalysisService imageAnalysisService)
        {
            _imageAnalysisService = imageAnalysisService;
        }

        /// <summary>
        /// Checks that content type is specified and valid.
        /// </summary>
        /// <param name="model">Create upload details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateCreateContentType(CreateUploadModel model, string keyPrefix = null)
        {
            if (string.IsNullOrWhiteSpace(model.ContentType))
                throw new ValidationErrorException(new ValidationError(UploadPropertyNames.ContentType, UploadResource.ContentTypeRequiredMessage, keyPrefix));
        }

        /// <summary>
        /// Validates information supplied to create a new upload.
        /// </summary>
        /// <param name="model">Create upload details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateCreateUpload(CreateUploadModel model, string keyPrefix = null)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ValidationErrorException(new ValidationError(UploadPropertyNames.Name, UploadResource.NameRequiredMessage, keyPrefix));
            if (model.Content == null)
                throw new ValidationErrorException(new ValidationError(UploadPropertyNames.Content, UploadResource.ContentRequiredMessage, keyPrefix));
        }

        /// <summary>
        /// Validates information supplied to create a new image.
        /// </summary>
        /// <param name="model">Create image details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        /// <returns>Width and height of image.</returns>
        public Size? ValidateCreateImage(CreateUploadModel model, string keyPrefix)
        {
            // Perform generic upload checks
            ValidateCreateUpload(model, keyPrefix);

            // Check that upload is image
            Size? size = _imageAnalysisService.GetImageDimensions(model.Content);
            if (size == null)
                throw new ValidationErrorException(new ValidationError(UploadPropertyNames.Content, UploadResource.ImageInvalidMessage, keyPrefix));

            // Return dimensions of image
            return size;
        }
    }
}
