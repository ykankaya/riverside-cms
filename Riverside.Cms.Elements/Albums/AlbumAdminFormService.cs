using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Validation;
using Riverside.UI.Forms;
using Riverside.Cms.Elements.Resources;

namespace Riverside.Cms.Elements.Albums
{
    public class AlbumAdminFormService : IFormService, IFormUploadService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IAlbumValidator _albumValidator;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;

        public AlbumAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IAlbumValidator albumValidator, IElementFactory elementFactory, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _albumValidator = albumValidator;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
        }

        public Guid FormId { get { return new Guid("b539d2a4-52ae-40d5-b366-e42447b93d15"); } }

        private AlbumPhotoViewModel GetPhotoViewModel(AlbumPhoto photo, bool uncommittedPhotoImages)
        {
            // TODO: Use proper URL routing here, do not hard wire these URLs.
            string imageUrl = null;
            if (uncommittedPhotoImages)
                imageUrl = string.Format("/uploads/{0}?t={0}", photo.ThumbnailImageUploadId);
            else
                imageUrl = string.Format("/elements/{0}/uploads/{1}?format=thumbnail&t={1}", photo.ElementId, photo.AlbumPhotoId);
            AlbumPhotoViewModel photoViewModel = new AlbumPhotoViewModel {
                AlbumPhotoId           = photo.AlbumPhotoId.ToString(),
                ThumbnailImageUploadId = photo.ThumbnailImageUploadId.ToString(),
                PreviewImageUploadId   = photo.PreviewImageUploadId.ToString(),
                ImageUploadId          = photo.ImageUploadId.ToString(),
                Name                   = photo.Name,
                Description            = photo.Description,
                ImageUrl               = imageUrl
            };
            return photoViewModel;
        }

        private Form GetAlbumForm(string context, long elementId)
        {
            // Get current album settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            AlbumSettings albumSettings = (AlbumSettings)elementService.New(_authenticationService.TenantId);
            albumSettings.ElementId = elementId;
            elementService.Read(albumSettings);

            // Get album photo view models
            List<AlbumPhotoViewModel> photoViewModels = new List<AlbumPhotoViewModel>();
            foreach (AlbumPhoto photo in albumSettings.Photos)
            {
                AlbumPhotoViewModel photoViewModel = new AlbumPhotoViewModel {
                    AlbumPhotoId         = photo.AlbumPhotoId.ToString(),
                    PreviewImageUploadId = photo.PreviewImageUploadId.ToString(),
                    Name                 = photo.Name,
                    Description          = photo.Description,
                    ImageUrl             = string.Format("/elements/{0}/uploads/{1}?format=preview&t={1}", photo.ElementId, photo.AlbumPhotoId)
                };
                photoViewModels.Add(photoViewModel);
            }
            string data = JsonConvert.SerializeObject(photoViewModels);

            // Return form with data
            return new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, Data = data };
        }

        private Form GetPhotosForm(string context, long elementId)
        {
            // Get current album settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            AlbumSettings albumSettings = (AlbumSettings)elementService.New(_authenticationService.TenantId);
            albumSettings.ElementId = elementId;
            elementService.Read(albumSettings);

            // Get album photo view models
            List<AlbumPhotoViewModel> photoViewModels = new List<AlbumPhotoViewModel>();
            foreach (AlbumPhoto photo in albumSettings.Photos)
                photoViewModels.Add(GetPhotoViewModel(photo, false));
            string data = JsonConvert.SerializeObject(photoViewModels);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, Data = data };
            form.Fields.Add("displayName", new TextField {
                Name                  = "displayName",
                Label                 = ElementResource.AlbumDisplayNameLabel,
                MaxLength             = AlbumLengths.DisplayNameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.AlbumDisplayNameMaxLengthMessage, "displayName", AlbumLengths.DisplayNameMaxLength),
                Value                 = albumSettings.DisplayName
            });
            form.SubmitLabel = ElementResource.AlbumButtonLabel;

            // Return result
            return form;
        }

        private Form GetPhotoForm(string context, long elementId)
        {
            // Construct form
            Dictionary<string, string> labels = new Dictionary<string, string>();
            labels.Add("update", ElementResource.AlbumUpdatePhotoButtonLabel);
            labels.Add("create", ElementResource.AlbumCreatePhotoButtonLabel);
            string data = JsonConvert.SerializeObject(labels);
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, Data = data };
            form.Fields.Add("name", new TextField {
                Name                  = "name",
                Label                 = ElementResource.AlbumPhotoNameLabel,
                MaxLength             = AlbumLengths.NameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.AlbumPhotoNameMaxLengthMessage, "name", AlbumLengths.NameMaxLength)
            });
            form.Fields.Add("description", new MultiLineTextField {
                Name  = "description",
                Label = ElementResource.AlbumPhotoDescriptionLabel,
                Rows  = 4
            });
            form.Fields.Add("upload", new UploadField {
                Name  = "upload",
                Label = ElementResource.AlbumPhotoUploadLabel
            });

            // Return result
            return form;
        }

        public Form GetForm(string context)
        {
            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            string action = parts[0];

            // Allow through "album" actions
            if (action == "album")
                return GetAlbumForm(context, elementId);

            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Perform the correct action based on form context
            switch (action)
            {
                case "photo":
                    return GetPhotoForm(context, elementId);

                case "photos":
                    return GetPhotosForm(context, elementId);
            }

            // Return nothing
            return null;
        }

        private void PostPhotosForm(Form form, long pageId, long elementId)
        {
            // Get tenant ID
            long tenantId = _authenticationService.TenantId;

            // Get element service
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

            // Get updated album settings
            AlbumSettings albumSettings = (AlbumSettings)elementService.New(_authenticationService.TenantId);
            albumSettings.ElementId = elementId;
            albumSettings.DisplayName = string.IsNullOrWhiteSpace(((TextField)form.Fields["displayName"]).Value) ? null : ((TextField)form.Fields["displayName"]).Value;
            albumSettings.Photos = new List<AlbumPhoto>();
            List<AlbumPhotoViewModel> photoViewModels = JsonConvert.DeserializeObject<List<AlbumPhotoViewModel>>(form.Data);
            for (int index = 0; index < photoViewModels.Count; index++)
            {
                AlbumPhotoViewModel photoViewModel = photoViewModels[index];
                albumSettings.Photos.Add(new AlbumPhoto {
                    AlbumPhotoId           = Convert.ToInt64(photoViewModel.AlbumPhotoId),
                    Description            = photoViewModel.Description,
                    ElementId              = elementId,
                    ImageTenantId          = tenantId,
                    ImageUploadId          = Convert.ToInt64(photoViewModel.ImageUploadId),
                    Name                   = photoViewModel.Name,
                    PreviewImageUploadId   = Convert.ToInt64(photoViewModel.PreviewImageUploadId),
                    SortOrder              = index,
                    TenantId               = tenantId,
                    ThumbnailImageUploadId = Convert.ToInt64(photoViewModel.ThumbnailImageUploadId)
                });
            }

            // Perform the update
            elementService.Update(albumSettings);
        }

        private string PostPhotoForm(Form form, long tenantId, long pageId, long elementId)
        {
            // Get album photo details
            long albumPhotoId = Convert.ToInt64(form.Data);
            string uploadIds = ((UploadField)form.Fields["upload"]).Value;
            AlbumPhoto photo = new AlbumPhoto {
                TenantId     = tenantId,
                ElementId    = elementId,
                AlbumPhotoId = albumPhotoId,
                Name         = ((TextField)form.Fields["name"]).Value,
                Description  = ((MultiLineTextField)form.Fields["description"]).Value
            };
            if (!string.IsNullOrWhiteSpace(uploadIds))
            {
                string[] uploadParts = uploadIds.Split('|');
                photo.ImageTenantId = tenantId;
                photo.ThumbnailImageUploadId = Convert.ToInt64(uploadParts[0]);
                photo.PreviewImageUploadId   = Convert.ToInt64(uploadParts[1]);
                photo.ImageUploadId          = Convert.ToInt64(uploadParts[2]);
            }

            // Validate supplied data
            _albumValidator.ValidatePhoto(photo);

            // Determine whether or not there are uncommitted photo images
            bool uncommittedPhotoImages = true;
            if (albumPhotoId > 0)
            {
                IAlbumService albumService = (IAlbumService)_elementFactory.GetElementService(FormId);
                AlbumPhoto currentPhoto = albumService.ReadPhoto(tenantId, elementId, albumPhotoId);
                uncommittedPhotoImages = currentPhoto.ImageUploadId != photo.ImageUploadId;
            }

            // Get photo view model and return it's JSON representation as form result status
            AlbumPhotoViewModel photoViewModel = GetPhotoViewModel(photo, uncommittedPhotoImages);
            return JsonConvert.SerializeObject(photoViewModel);
        }

        private FormResult PostCreateUpload(long elementId, string name, string contentType, byte[] content)
        {
            // Get tenant identifier, website identifier
            long tenantId = _authenticationService.TenantId;

            // Get upload model
            CreateUploadModel model = new CreateUploadModel {
                Content     = content,
                ContentType = contentType,
                Name        = name,
                TenantId    = tenantId
            };

            // Create uploads, ready to be assigned to photo when form submitted
            IUploadElementService albumService = (IUploadElementService)_elementFactory.GetElementService(FormId);
            ImageUploadIds uploadIds = albumService .PrepareImages2(tenantId, elementId, model);

            // Return form result
            string status = string.Format("{0}|{1}|{2}", uploadIds.ThumbnailImageUploadId, uploadIds.PreviewImageUploadId, uploadIds.ImageUploadId);
            return _formHelperService.GetFormResult(status);
        }

        public FormResult PostUpload(string id, string context, string name, string contentType, byte[] content)
        {
            try
            {
                // Check permissions
                _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

                // The form result
                FormResult formResult = null;

                // Split context into different parts
                string[] parts = context.Split('|');
                long pageId = Convert.ToInt64(parts[1]);
                long elementId = Convert.ToInt64(parts[2]);
                string action = parts[0];

                // Perform the correct action based on form context
                switch (action)
                {
                    case "photo":
                        formResult = PostCreateUpload(elementId, name, contentType, content);
                        break;
                }

                // Return result
                return formResult;
            }
            catch (ValidationErrorException ex)
            {
                // Return form result containing errors
                return _formHelperService.GetFormResultWithValidationErrors(ex.Errors);
            }
            catch (Exception)
            {
                // Return form result containing unexpected error message
                return _formHelperService.GetFormResultWithErrorMessage(ApplicationResource.UnexpectedErrorMessage);
            }
        }

        public FormResult PostForm(Form form)
        {
            try
            {
                // Check permissions
                _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

                // Get page and element identifiers, plus admin action
                string[] parts = form.Context.Split('|');
                long pageId = Convert.ToInt64(parts[1]);
                long elementId = Convert.ToInt64(parts[2]);
                string action = parts[0];
                long tenantId = _authenticationService.TenantId;

                // Switch on action
                string status = null;
                switch (action)
                {
                    case "photo":
                        status = PostPhotoForm(form, tenantId, pageId, elementId);
                        break;

                    case "photos":
                        PostPhotosForm(form, pageId, elementId);
                        break;
                }

                // Return form result with no errors
                return _formHelperService.GetFormResult(status);
            }
            catch (ValidationErrorException ex)
            {
                // Return form result containing errors
                return _formHelperService.GetFormResultWithValidationErrors(ex.Errors);
            }
            catch (Exception)
            {
                // Return form result containing unexpected error message
                return _formHelperService.GetFormResultWithErrorMessage(ApplicationResource.UnexpectedErrorMessage);
            }
        }
    }
}
