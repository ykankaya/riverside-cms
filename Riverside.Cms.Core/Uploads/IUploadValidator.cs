using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Validates upload actions.
    /// </summary>
    public interface IUploadValidator
    {
        /// <summary>
        /// Checks that content type is specified and valid.
        /// </summary>
        /// <param name="model">Create upload details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        void ValidateCreateContentType(CreateUploadModel model, string keyPrefix = null);

        /// <summary>
        /// Validates information supplied to create a new upload.
        /// </summary>
        /// <param name="model">Create upload details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        void ValidateCreateUpload(CreateUploadModel model, string keyPrefix = null);

        /// <summary>
        /// Validates information supplied to create a new image.
        /// </summary>
        /// <param name="model">Create image details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        /// <returns>Width and height of image.</returns>
        Size? ValidateCreateImage(CreateUploadModel model, string keyPrefix = null);
    }
}
