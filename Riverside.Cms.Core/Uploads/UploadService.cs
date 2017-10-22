using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Storage;
using Drawing = System.Drawing;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Service for managing uploads.
    /// </summary>
    public class UploadService : IUploadService
    {
        // Member variables
        private IImageAnalysisService _imageAnalysisService;
        private IStorageService _storageService;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IUploadRepository _uploadRepository;
        private IUploadValidator _uploadValidator;

        /// <summary>
        /// Constructors sets dependent components.
        /// </summary>
        /// <param name="imageAnalysisService">Image analysis service.</param>
        /// <param name="storageService">Storage service.</param>
        /// <param name="uploadRepository">Upload repository.</param>
        /// <param name="unitOfWorkFactory">Unit of work factory.</param>
        /// <param name="uploadValidator">Validates upload actions.</param>
        public UploadService(IImageAnalysisService imageAnalysisService, IStorageService storageService, IUploadRepository uploadRepository, IUnitOfWorkFactory unitOfWorkFactory, IUploadValidator uploadValidator)
        {
            _imageAnalysisService = imageAnalysisService;
            _storageService = storageService;
            _uploadRepository = uploadRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _uploadValidator = uploadValidator;
        }

        /// <summary>
        /// From content type, determine the more generic upload type.
        /// </summary>
        /// <param name="contentType">Content type (e.g. "image/png").</param>
        /// <returns>Upload type.</returns>
        public UploadType GetUploadType(string contentType)
        {
            contentType = contentType.ToLower();
            switch (contentType)
            {
                case ContentTypes.Gif:
                case ContentTypes.Jpeg:
                case ContentTypes.Png:
                    return UploadType.Image;

                default:
                    return UploadType.Upload;
            }
        }

        /// <summary>
        /// Creates a generic upload.
        /// </summary>
        /// <param name="model">The upload to create.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Upload populated with newly allocated upload identifier.</returns>
        private Upload CreateUpload(CreateUploadModel model, IUnitOfWork unitOfWork)
        {
            // Validate create upload model
            _uploadValidator.ValidateCreateUpload(model);

            // Construct upload object
            DateTime now = DateTime.UtcNow;
            Upload upload = new Upload
            {
                TenantId = model.TenantId,
                Created = now,
                Updated = now,
                Name = model.Name.Trim(),
                Content = model.Content,
                ContentType = model.ContentType.Trim().ToLower(),
                Size = model.Content.Length,
                UploadType = UploadType.Upload,
                Committed = false
            };

            // Create upload and record newly allocated upload identifier
            upload.UploadId = _uploadRepository.CreateUpload(upload, unitOfWork);

            // Return upload object
            return upload;
        }

        /// <summary>
        /// Creates an image.
        /// </summary>
        /// <param name="model">The upload to create.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Image populated with newly allocated upload identifier.</returns>
        private Image CreateImage(CreateUploadModel model, IUnitOfWork unitOfWork)
        {
            // Validate create upload model
            Drawing.Size? size = _uploadValidator.ValidateCreateImage(model);

            // Construct image object
            DateTime now = DateTime.UtcNow;
            Image image = new Image
            {
                TenantId = model.TenantId,
                Created = now,
                Updated = now,
                Name = model.Name.Trim(),
                Content = model.Content,
                ContentType = model.ContentType.Trim().ToLower(),
                Size = model.Content.Length,
                UploadType = UploadType.Image,
                Committed = false,
                Width = size.Value.Width,
                Height = size.Value.Height
            };

            // Create image and record newly allocated upload identifier
            image.UploadId = _uploadRepository.CreateImage(image, unitOfWork);

            // Return image object
            return image;
        }

        /// <summary>
        /// Returns storage hierarchy used for uncommitted uploads.
        /// </summary>
        /// <returns>Uncomitted storage hierarchy.</returns>
        public List<string> GetUncommittedStorageHierarchy()
        {
            return new List<string> { "uncommitted" };
        }

        /// <summary>
        /// Creates a new upload with uncommitted state.
        /// </summary>
        /// <param name="model">The upload to create.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated upload identfier.</returns>
        public long Create(CreateUploadModel model, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Try to create upload
            try
            {
                // The first thing we need to check is that content type is specified
                _uploadValidator.ValidateCreateContentType(model);

                // Get upload type, which will determine how upload is stored
                UploadType uploadType = GetUploadType(model.ContentType);

                // Construct upload given upload type and content
                Upload upload = null;
                switch (uploadType)
                {
                    case UploadType.Image:
                        upload = CreateImage(model, unitOfWork ?? localUnitOfWork);
                        break;

                    case UploadType.Upload:
                        upload = CreateUpload(model, unitOfWork ?? localUnitOfWork);
                        break;
                }

                // Create upload in storage
                _storageService.Create(upload, GetUncommittedStorageHierarchy(), unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place, then return newly allocated upload identifier
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return upload.UploadId;
            }
            catch
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw;
            }
            finally
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Dispose();
            }
        }

        /// <summary>
        /// Commits an upload.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="uploadId">Upload identifier.</param>
        /// <param name="storageHierarchy">New location of upload, once committed. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Commit(long tenantId, long uploadId, List<string> storageHierarchy, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Try to commit upload
            try
            {
                // Get location of uncommitted uploads
                List<string> uncommittedStorageHierarchy = GetUncommittedStorageHierarchy();

                // Get upload
                Upload upload = Read(tenantId, uploadId, uncommittedStorageHierarchy, unitOfWork ?? localUnitOfWork);

                // Set committed true
                upload.Committed = true;
                upload.Updated = DateTime.UtcNow;

                // Update upload so that committed is true
                switch (upload.UploadType)
                {
                    case UploadType.Upload:
                        _uploadRepository.UpdateUpload(upload, unitOfWork ?? localUnitOfWork);
                        break;

                    case UploadType.Image:
                        _uploadRepository.UpdateImage((Image)upload, unitOfWork ?? localUnitOfWork);
                        break;
                }

                // Create copy of upload with new storage hierarchy
                _storageService.Create(upload, storageHierarchy, unitOfWork ?? localUnitOfWork);

                // Delete uncommitted upload from storage
                _storageService.Delete(upload, uncommittedStorageHierarchy, unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place, then return newly allocated upload identifier
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
            }
            catch
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw;
            }
            finally
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Dispose();
            }
        }

        /// <summary>
        /// Retrieves upload content.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="uploadId">Upload identifer.</param>
        /// <param name="storageHierarchy">Location of upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Upload content.</returns>
        public Upload Read(long tenantId, long uploadId, List<string> storageHierarchy, IUnitOfWork unitOfWork = null)
        {
            // Get main upload details from upload repository
            Upload upload = _uploadRepository.Read(tenantId, uploadId, unitOfWork);

            // Get upload content from underlying storage and populate upload object
            UploadContent uploadContent = _storageService.Read(upload, storageHierarchy, unitOfWork);
            upload.Content = uploadContent.Content;
            upload.ContentType = uploadContent.ContentType;

            // Return upload
            return upload;
        }

        /// <summary>
        /// Deletes an upload.
        /// </summary>
        /// <param name="teantId">Tenant identifier.</param>
        /// <param name="uploadId">Upload identifier.</param>
        /// <param name="storageHierarchy">Location of upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, long uploadId, List<string> storageHierarchy, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Try to delete upload
            try
            {
                // Get main upload details from upload repository
                Upload upload = _uploadRepository.Read(tenantId, uploadId, unitOfWork ?? localUnitOfWork);

                // Delete record of upload
                _uploadRepository.Delete(tenantId, uploadId, unitOfWork ?? localUnitOfWork);

                // Delete upload in storage
                _storageService.Delete(upload, storageHierarchy, unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
            }
            catch
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw;
            }
            finally
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Dispose();
            }
        }
    }
}
