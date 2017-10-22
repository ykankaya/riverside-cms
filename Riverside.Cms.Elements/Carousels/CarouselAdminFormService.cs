using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Uploads;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;
using Riverside.UI.Forms;
using Riverside.Cms.Elements.Resources;
using Riverside.Cms.Core.Resources;

namespace Riverside.Cms.Elements.Carousels
{
    public class CarouselAdminFormService : IFormService, IFormUploadService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private ICarouselValidator _carouselValidator;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;
        private IPageService _pageService;

        public CarouselAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, ICarouselValidator carouselValidator, IElementFactory elementFactory, IFormHelperService formHelperService, IPageService pageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _carouselValidator = carouselValidator;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
            _pageService = pageService;
        }

        public Guid FormId { get { return new Guid("aacb11a0-5532-47cb-aab9-939cee3d5175"); } }

        /// <summary>
        /// TODO: Code duplicated in NavBarFormService. Fix this.
        /// </summary>
        /// <returns>Collection of pages.</returns>
        private IEnumerable<Page> GetFoldersAndDocuments()
        {
            long tenantId = _authenticationService.TenantId;
            ISearchParameters searchParameters = new SearchParameters { PageIndex = 0, PageSize = 1000 }; // TODO: Need way to return all pages, not have some max bound upper limit
            ISearchResult<Page> folderResult = _pageService.List(tenantId, searchParameters, null, PageSortBy.Name, true, true, PageType.Folder, false);
            ISearchResult<Page> documentResult = _pageService.List(tenantId, searchParameters, null, PageSortBy.Name, true, true, PageType.Document, false);
            IEnumerable<Page> foldersAndDocuments = folderResult.Items.Concat(documentResult.Items).OrderBy(p => p.Name);
            return foldersAndDocuments;
        }

        /// <summary>
        /// TODO: Use proper URL routing here, do not hard wire these URLs.
        /// </summary>
        /// <param name="slide">Carousel slide.</param>
        /// <param name="uncommittedSlideImages">True if slide's image has changed and there are uncommitted image uploads, false if slide's image has not changed.</param>
        private CarouselSlideViewModel GetSlideViewModel(CarouselSlide slide, bool uncommittedSlideImages)
        {
            string imageUrl = null;
            if (uncommittedSlideImages)
                imageUrl = string.Format("/uploads/{0}?t={0}", slide.ThumbnailImageUploadId);
            else
                imageUrl = string.Format("/elements/{0}/uploads/{1}?format=thumbnail&t={1}", slide.ElementId, slide.CarouselSlideId);
            CarouselSlideViewModel slideViewModel = new CarouselSlideViewModel
            {
                CarouselSlideId = slide.CarouselSlideId.ToString(),
                ThumbnailImageUploadId = slide.ThumbnailImageUploadId.ToString(),
                PreviewImageUploadId = slide.PreviewImageUploadId.ToString(),
                ImageUploadId = slide.ImageUploadId.ToString(),
                Name = slide.Name,
                Description = slide.Description,
                PageId = slide.PageId == null ? string.Empty : slide.PageId.ToString(),
                PageText = slide.PageText,
                ImageUrl = imageUrl
            };
            if (slide.PageId.HasValue)
            {
                if (string.IsNullOrWhiteSpace(slide.PageText))
                    slideViewModel.ButtonText = _pageService.Read(slide.TenantId, slide.PageId.Value).Name;
                else
                    slideViewModel.ButtonText = slide.PageText;
                slideViewModel.ButtonUrl = "/pages/" + slide.PageId.Value;
            }
            return slideViewModel;
        }

        private Form GetSlidesForm(string context, long elementId)
        {
            // Get current carousel settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            CarouselSettings carouselSettings = (CarouselSettings)elementService.New(_authenticationService.TenantId);
            carouselSettings.ElementId = elementId;
            elementService.Read(carouselSettings);

            // Get carousel slide view models
            List<CarouselSlideViewModel> slideViewModels = new List<CarouselSlideViewModel>();
            foreach (CarouselSlide slide in carouselSettings.Slides)
                slideViewModels.Add(GetSlideViewModel(slide, false));
            string data = JsonConvert.SerializeObject(slideViewModels);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, Data = data };
            form.SubmitLabel = ElementResource.CarouselButtonLabel;

            // Return result
            return form;
        }

        private Form GetSlideForm(string context, long elementId)
        {
            // Construct form
            Dictionary<string, string> labels = new Dictionary<string, string>();
            labels.Add("update", ElementResource.CarouselUpdateSlideButtonLabel);
            labels.Add("create", ElementResource.CarouselCreateSlideButtonLabel);
            string data = JsonConvert.SerializeObject(labels);
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, Data = data };
            form.Fields.Add("name", new TextField
            {
                Name = "name",
                Label = ElementResource.CarouselSlideNameLabel,
                MaxLength = CarouselLengths.NameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.CarouselSlideNameMaxLengthMessage, "name", CarouselLengths.NameMaxLength)
            });
            form.Fields.Add("description", new MultiLineTextField
            {
                Name = "description",
                Label = ElementResource.CarouselSlideDescriptionLabel,
                Rows = 4
            });
            form.Fields.Add("upload", new UploadField
            {
                Name = "upload",
                Label = ElementResource.CarouselSlideUploadLabel
            });
            List<ListFieldItem<string>> items = new List<ListFieldItem<string>>();
            form.Fields.Add("page", new SelectListField<string>
            {
                Name = "page",
                Label = ElementResource.CarouselSlidePageLabel,
                Items = items,
                Value = string.Empty
            });
            IEnumerable<Page> foldersAndDocuments = GetFoldersAndDocuments();
            items.Add(new ListFieldItem<string> { Name = ElementResource.CarouselSlidePageDefaultOption, Value = string.Empty });
            foreach (Page page in foldersAndDocuments)
                items.Add(new ListFieldItem<string> { Name = page.Name, Value = page.PageId.ToString() });
            form.Fields.Add("pageText", new TextField
            {
                Name = "pageText",
                Label = ElementResource.CarouselSlidePageTextLabel,
                MaxLength = CarouselLengths.PageTextMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.CarouselSlidePageTextMaxLengthMessage, "pageText", CarouselLengths.PageTextMaxLength)
            });

            // Return result
            return form;
        }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            string action = parts[0];

            // Perform the correct action based on form context
            switch (action)
            {
                case "slide":
                    return GetSlideForm(context, elementId);

                case "slides":
                    return GetSlidesForm(context, elementId);
            }

            // Return nothing
            return null;
        }

        private void PostSlidesForm(Form form, long pageId, long elementId)
        {
            // Get tenant ID
            long tenantId = _authenticationService.TenantId;

            // Get element service
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

            // Get updated carousel settings
            CarouselSettings carouselSettings = (CarouselSettings)elementService.New(_authenticationService.TenantId);
            carouselSettings.ElementId = elementId;
            carouselSettings.Slides = new List<CarouselSlide>();
            List<CarouselSlideViewModel> slideViewModels = JsonConvert.DeserializeObject<List<CarouselSlideViewModel>>(form.Data);
            for (int index = 0; index < slideViewModels.Count; index++)
            {
                CarouselSlideViewModel slideViewModel = slideViewModels[index];
                carouselSettings.Slides.Add(new CarouselSlide
                {
                    CarouselSlideId = Convert.ToInt64(slideViewModel.CarouselSlideId),
                    Description = slideViewModel.Description,
                    ElementId = elementId,
                    ImageTenantId = tenantId,
                    ImageUploadId = Convert.ToInt64(slideViewModel.ImageUploadId),
                    Name = slideViewModel.Name,
                    PageId = string.IsNullOrWhiteSpace(slideViewModel.PageId) ? null : (long?)Convert.ToInt64(slideViewModel.PageId),
                    PageText = string.IsNullOrWhiteSpace(slideViewModel.PageText) ? null : slideViewModel.PageText,
                    PageTenantId = string.IsNullOrWhiteSpace(slideViewModel.PageId) ? null : (long?)tenantId,
                    PreviewImageUploadId = Convert.ToInt64(slideViewModel.PreviewImageUploadId),
                    SortOrder = index,
                    TenantId = tenantId,
                    ThumbnailImageUploadId = Convert.ToInt64(slideViewModel.ThumbnailImageUploadId)
                });
            }

            // Perform the update
            elementService.Update(carouselSettings);
        }

        private string PostSlideForm(Form form, long tenantId, long pageId, long elementId)
        {
            // Get carousel slide details
            long carouselSlideId = Convert.ToInt64(form.Data);
            string uploadIds = ((UploadField)form.Fields["upload"]).Value;
            string pageValue = ((SelectListField<string>)form.Fields["page"]).Value;
            CarouselSlide slide = new CarouselSlide
            {
                TenantId = tenantId,
                ElementId = elementId,
                CarouselSlideId = carouselSlideId,
                Name = ((TextField)form.Fields["name"]).Value,
                Description = ((MultiLineTextField)form.Fields["description"]).Value,
                PageText = ((TextField)form.Fields["pageText"]).Value,
                PageId = pageValue == string.Empty ? null : (long?)Convert.ToInt64(pageValue),
                PageTenantId = pageValue == string.Empty ? null : (long?)tenantId
            };
            if (!string.IsNullOrWhiteSpace(uploadIds))
            {
                string[] uploadParts = uploadIds.Split('|');
                slide.ImageTenantId = tenantId;
                slide.ThumbnailImageUploadId = Convert.ToInt64(uploadParts[0]);
                slide.PreviewImageUploadId = Convert.ToInt64(uploadParts[1]);
                slide.ImageUploadId = Convert.ToInt64(uploadParts[2]);
            }

            // Validate supplied data
            _carouselValidator.ValidateSlide(slide);

            // Determine whether or not there are uncommitted slide images
            bool uncommittedSlideImages = true;
            if (carouselSlideId > 0)
            {
                ICarouselService carouselService = (ICarouselService)_elementFactory.GetElementService(FormId);
                CarouselSlide currentSlide = carouselService.ReadSlide(tenantId, elementId, carouselSlideId);
                uncommittedSlideImages = currentSlide.ImageUploadId != slide.ImageUploadId;
            }

            // Get slide view model and return it's JSON representation as form result status
            CarouselSlideViewModel slideViewModel = GetSlideViewModel(slide, uncommittedSlideImages);
            return JsonConvert.SerializeObject(slideViewModel);
        }

        private FormResult PostCreateUpload(long elementId, string name, string contentType, byte[] content)
        {
            // Get tenant identifier, website identifier
            long tenantId = _authenticationService.TenantId;

            // Get upload model
            CreateUploadModel model = new CreateUploadModel
            {
                Content = content,
                ContentType = contentType,
                Name = name,
                TenantId = tenantId
            };

            // Create uploads, ready to be assigned to slide when form submitted
            IUploadElementService carouselService = (IUploadElementService)_elementFactory.GetElementService(FormId);
            ImageUploadIds uploadIds = carouselService.PrepareImages2(tenantId, elementId, model);

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
                    case "slide":
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
                    case "slide":
                        status = PostSlideForm(form, tenantId, pageId, elementId);
                        break;

                    case "slides":
                        PostSlidesForm(form, pageId, elementId);
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
