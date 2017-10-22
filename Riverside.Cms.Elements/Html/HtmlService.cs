using System;
using System.Collections.Generic;
using System.Linq;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Elements.Resources;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Html
{
    public class HtmlService : IAdvancedElementService, IUploadElementService
    {
        private IHtmlRepository _htmlRepository;
        private IHtmlValidator _htmlValidator;
        private IImageAnalysisService _imageAnalysisService;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IUploadService _uploadService;

        public HtmlService(IHtmlRepository htmlRepository, IHtmlValidator htmlValidator, IImageAnalysisService imageAnalysisService, IUnitOfWorkFactory unitOfWorkFactory, IUploadService uploadService)
        {
            _htmlRepository = htmlRepository;
            _htmlValidator = htmlValidator;
            _imageAnalysisService = imageAnalysisService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _uploadService = uploadService;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("c92ee4c4-b133-44cc-8322-640e99c334dc");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new HtmlSettings { TenantId = tenantId, ElementTypeId = ElementTypeId, Html = ElementResource.HtmlDefaultHtml };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<HtmlSettings, HtmlContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _htmlRepository.Create((HtmlSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _htmlRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _htmlRepository.Read((HtmlSettings)settings, unitOfWork);
        }

        private List<HtmlUpload> GetHtmlUploads(HtmlSettings settings)
        {
            // Construct empty list of uploads
            List<HtmlUpload> uploads = new List<HtmlUpload>();

            // Nothing to do if no HTML
            if (string.IsNullOrWhiteSpace(settings.Html))
                return uploads;

            // Search for strings like "/elements/{elementid}/uploads/{uploadid}?t={ticks}" and extract all uploadids.
            string html = settings.Html;
            const string beginText = "/elements/";
            const string endText = "\"";
            const int uploadIdIndex = 4;
            bool finished = false;
            while (!finished)
            {
                int uploadBeginIndex = html.IndexOf(beginText);
                finished = uploadBeginIndex < 0;
                if (finished)
                    break;
                int uploadEndIndex = html.IndexOf(endText, uploadBeginIndex + beginText.Length);
                finished = uploadEndIndex < 0;
                if (finished)
                    break;
                string url = html.Substring(uploadBeginIndex, uploadEndIndex - uploadBeginIndex);
                string[] urlParts = url.Split('/');
                string uploadIdText = urlParts[uploadIdIndex];
                int queryIndex = uploadIdText.IndexOf("?");
                if (queryIndex >= 0)
                    uploadIdText = uploadIdText.Substring(0, queryIndex);
                long htmlUploadId = Convert.ToInt64(uploadIdText);
                uploads.Add(new HtmlUpload { TenantId = settings.TenantId, ElementId = settings.ElementId, HtmlUploadId = htmlUploadId });
                html = html.Substring(uploadEndIndex + 1);
            }

            // Return list of uploads
            return uploads;
        }

        private bool HtmlUploadIsImage(HtmlUpload htmlUpload)
        {
            return htmlUpload.ImageTenantId.HasValue && htmlUpload.ThumbnailImageUploadId.HasValue && htmlUpload.PreviewImageUploadId.HasValue && htmlUpload.ImageUploadId.HasValue;
        }

        private bool HtmlUploadIsUpload(HtmlUpload htmlUpload)
        {
            return htmlUpload.UploadTenantId.HasValue && htmlUpload.UploadId.HasValue;
        }

        private void DeleteUploads(List<HtmlUpload> htmlUploads, IUnitOfWork unitOfWork)
        {
            foreach (HtmlUpload htmlUpload in htmlUploads)
            {
                if (HtmlUploadIsImage(htmlUpload))
                {
                    _uploadService.Delete(htmlUpload.ImageTenantId.Value, htmlUpload.ThumbnailImageUploadId.Value, GetHtmlUploadStorageHierarchy(htmlUpload.ElementId), unitOfWork);
                    _uploadService.Delete(htmlUpload.ImageTenantId.Value, htmlUpload.PreviewImageUploadId.Value, GetHtmlUploadStorageHierarchy(htmlUpload.ElementId), unitOfWork);
                    _uploadService.Delete(htmlUpload.ImageTenantId.Value, htmlUpload.ImageUploadId.Value, GetHtmlUploadStorageHierarchy(htmlUpload.ElementId), unitOfWork);
                }
                if (HtmlUploadIsUpload(htmlUpload))
                {
                    _uploadService.Delete(htmlUpload.UploadTenantId.Value, htmlUpload.UploadId.Value, GetHtmlUploadStorageHierarchy(htmlUpload.ElementId), unitOfWork);
                }
            }
        }

        private string SanitizeHtml(string html)
        {
            // Ensure no null values
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            // Fix incorrect relative URLs
            html = html.Replace("../../../../elements", "/elements");
            html = html.Replace("../../../../pages", "/pages");

            // Return sanitized HTML
            return html;
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            // Get HTML uploads that are being referenced by updated HTML
            HtmlSettings htmlSettings = (HtmlSettings)settings;
            htmlSettings.Html = SanitizeHtml(htmlSettings.Html);
            htmlSettings.Uploads = GetHtmlUploads(htmlSettings);

            // Get previous HTML settings
            HtmlSettings previousHtmlSettings = (HtmlSettings)New(settings.TenantId);
            previousHtmlSettings.ElementId = settings.ElementId;
            _htmlRepository.Read(previousHtmlSettings, unitOfWork);

            // Get HTML uploads that are no longer used
            List<HtmlUpload> uploadsToDelete = new List<HtmlUpload>();
            Dictionary<long, HtmlUpload> uploadsById = htmlSettings.Uploads.GroupBy(u => u.HtmlUploadId).ToDictionary(u => u.Key, u => u.First());
            foreach (HtmlUpload previousHtmlUpload in previousHtmlSettings.Uploads)
            {
                if (!uploadsById.ContainsKey(previousHtmlUpload.HtmlUploadId))
                    uploadsToDelete.Add(previousHtmlUpload);
            }

            // Update HTML element
            _htmlRepository.Update(htmlSettings, unitOfWork);

            // Delete HTML uploads that are no longer used
            DeleteUploads(uploadsToDelete, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            // Get HTML settings so that we know what uploads to delete
            HtmlSettings htmlSettings = (HtmlSettings)New(tenantId);
            htmlSettings.ElementId = elementId;
            _htmlRepository.Read(htmlSettings, unitOfWork);

            // Delete HTML and upload records
            _htmlRepository.Delete(tenantId, elementId, unitOfWork);

            // Delete uploads
            DeleteUploads(htmlSettings.Uploads, unitOfWork);
        }

        private string FormatHtml(string html)
        {
            return html.Replace("%YEAR%", DateTime.UtcNow.Year.ToString());
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            // Construct element content
            HtmlContent htmlContent = new HtmlContent();
            htmlContent.PartialViewName = "Html";

            // Populate element content according to element settings
            HtmlSettings htmlSettings = (HtmlSettings)settings;
            htmlContent.FormattedHtml = FormatHtml(htmlSettings.Html);

            // Return resulting element content
            return htmlContent;
        }

        public long PrepareImages(long tenantId, long elementId, CreateUploadModel model, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Begin work
            try
            {
                // Check that uploaded content is valid image
                System.Drawing.Size imageSize = _htmlValidator.ValidatePrepareImages(tenantId, elementId, model);

                // Get HTML settings (does not have to be within unit of work)
                HtmlSettings htmlSettings = (HtmlSettings)New(tenantId);
                htmlSettings.ElementId = elementId;
                Read(htmlSettings);

                // Create thumbnail model
                byte[] thumbnailContent = model.Content;
                if (imageSize.Width > htmlSettings.ThumbnailImageWidth || imageSize.Height > htmlSettings.ThumbnailImageHeight)
                {
                    ResizeInfo thumbnailResizeInfo = new ResizeInfo
                    {
                        Width = htmlSettings.ThumbnailImageWidth,
                        Height = htmlSettings.ThumbnailImageHeight,
                        ResizeMode = htmlSettings.ThumbnailImageResizeMode
                    };
                    thumbnailContent = _imageAnalysisService.ResizeImage(model.Content, thumbnailResizeInfo);
                }
                CreateUploadModel thumbnailModel = new CreateUploadModel
                {
                    Content = thumbnailContent,
                    ContentType = model.ContentType,
                    Name = model.Name,
                    TenantId = model.TenantId
                };

                // Create preview model
                byte[] previewContent = model.Content;
                if (imageSize.Width > htmlSettings.PreviewImageWidth || imageSize.Height > htmlSettings.PreviewImageHeight)
                {
                    ResizeInfo previewResizeInfo = new ResizeInfo
                    {
                        Width = htmlSettings.PreviewImageWidth,
                        Height = htmlSettings.PreviewImageHeight,
                        ResizeMode = htmlSettings.PreviewImageResizeMode
                    };
                    previewContent = _imageAnalysisService.ResizeImage(model.Content, previewResizeInfo);
                }
                CreateUploadModel previewModel = new CreateUploadModel
                {
                    Content = previewContent,
                    ContentType = model.ContentType,
                    Name = model.Name,
                    TenantId = model.TenantId
                };

                // Create uncommitted uploads for thumbnail, preview and original image
                long thumbnailImageUploadId = _uploadService.Create(thumbnailModel, unitOfWork ?? localUnitOfWork);
                long previewImageUploadId = _uploadService.Create(previewModel, unitOfWork ?? localUnitOfWork);
                long imageUploadId = _uploadService.Create(model, unitOfWork ?? localUnitOfWork);

                // Commit uploads
                _uploadService.Commit(tenantId, thumbnailImageUploadId, GetHtmlUploadStorageHierarchy(elementId), unitOfWork ?? localUnitOfWork);
                _uploadService.Commit(tenantId, previewImageUploadId, GetHtmlUploadStorageHierarchy(elementId), unitOfWork ?? localUnitOfWork);
                _uploadService.Commit(tenantId, imageUploadId, GetHtmlUploadStorageHierarchy(elementId), unitOfWork ?? localUnitOfWork);

                // Create HTML image, recording upload IDs of newly created images
                HtmlUpload upload = new HtmlUpload
                {
                    TenantId = tenantId,
                    ElementId = elementId,
                    ImageTenantId = tenantId,
                    ThumbnailImageUploadId = thumbnailImageUploadId,
                    PreviewImageUploadId = previewImageUploadId,
                    ImageUploadId = imageUploadId
                };
                long htmlImageId = _htmlRepository.CreateUpload(upload, unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place and return result
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return htmlImageId;
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

        public ImageUploadIds PrepareImages2(long tenantId, long elementId, CreateUploadModel model, IUnitOfWork unitOfWork = null)
        {
            throw new NotImplementedException();
        }

        private List<string> GetHtmlUploadStorageHierarchy(long elementId)
        {
            return new List<string> { "elements", "html", elementId.ToString() };
        }

        public Upload ReadUpload(long tenantId, long elementId, long uploadId, string format, IUnitOfWork unitOfWork = null)
        {
            HtmlUpload htmlUpload = _htmlRepository.ReadUpload(tenantId, elementId, uploadId, unitOfWork);
            if (htmlUpload == null)
                return null;

            bool isImage = htmlUpload.ImageTenantId.HasValue && htmlUpload.ThumbnailImageUploadId.HasValue &&
                htmlUpload.PreviewImageUploadId.HasValue && htmlUpload.ImageUploadId.HasValue;
            bool isUpload = htmlUpload.UploadTenantId.HasValue && htmlUpload.UploadId.HasValue;
            if ((!isImage && !isUpload) || (isImage && isUpload))
                return null;

            if (isImage)
            {
                switch (format)
                {
                    case "preview":
                        return (Image)_uploadService.Read(htmlUpload.ImageTenantId.Value, htmlUpload.PreviewImageUploadId.Value, GetHtmlUploadStorageHierarchy(elementId), unitOfWork);

                    case "thumbnail":
                        return (Image)_uploadService.Read(htmlUpload.ImageTenantId.Value, htmlUpload.ThumbnailImageUploadId.Value, GetHtmlUploadStorageHierarchy(elementId), unitOfWork);

                    default:
                        return null;
                }
            }

            return _uploadService.Read(htmlUpload.UploadTenantId.Value, htmlUpload.UploadId.Value, GetHtmlUploadStorageHierarchy(elementId), unitOfWork);
        }
    }
}
