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
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Annotations;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Validation;
using Riverside.UI.Forms;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Form and element service for master page details administration.
    /// </summary>
    public class MasterPageFormService : IFormService, IBasicElementService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IDataAnnotationsService _dataAnnotationsService;
        private IElementService _elementService;
        private IFormHelperService _formHelperService;
        private IMasterPageService _masterPageService;
        private IPageService _pageService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Provides access to authentication services.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="dataAnnotationsService">Retrieves information from data annotations.</param>
        /// <param name="elementService">Used to enumerate elements.</param>
        /// <param name="formHelperService">Provides form helper utilities.</param>
        /// <param name="masterPageService">For administration of master pages.</param>
        public MasterPageFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IDataAnnotationsService dataAnnotationsService, IElementService elementService, IFormHelperService formHelperService, IMasterPageService masterPageService, IPageService pageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _dataAnnotationsService = dataAnnotationsService;
            _elementService = elementService;
            _formHelperService = formHelperService;
            _masterPageService = masterPageService;
            _pageService = pageService;
        }

        /// <summary>
        /// Returns GUID, identifying the type of element that this custom element service is associated with.
        /// </summary>
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("3b945a38-bb9c-41ff-9e1a-42607b75458e");
            }
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId
        {
            get
            {
                return new Guid("3b945a38-bb9c-41ff-9e1a-42607b75458e");
            }
        }

        /// <summary>
        /// Creates a new instance of a type of element settings.
        /// </summary>
        /// <param name="tenantId">Identifies the tenant that newly created element settings belong to.</param>
        /// <returns>Newly created element instance.</returns>
        public IElementSettings New(long tenantId)
        {
            return new ElementSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        /// <summary>
        /// Creates and returns strongly typed element info instance, populated with supplied element settings and content.
        /// </summary>
        /// <param name="settings">Element settings.</param>
        /// <param name="content">Element content.</param>
        /// <returns>An element info object containing settings and content.</returns>
        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<ElementSettings, MasterPageContent> { Settings = settings, Content = content };
        }

        /// <summary>
        /// Retrieves dynamic element content.
        /// </summary>
        /// <param name="settings">Contains element settings.</param>
        /// <param name="pageContext">Page context.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element content.</returns>
        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            long? masterPageId = null;
            if (pageContext.RouteValues.ContainsKey("masterpageid"))
                masterPageId = Convert.ToInt64(pageContext.RouteValues["masterpageid"]);
            if (masterPageId.HasValue)
                return new MasterPageContent { PartialViewName = "UpdateMasterPageAdmin", FormContext = string.Format("update|{0}", masterPageId) };
            else
                return new MasterPageContent { PartialViewName = "UpdateMasterPageAdmin", FormContext = "create" };
        }

        /// <summary>
        /// Returns form action, given form context. Throws exception if form context invalid.
        /// </summary>
        /// <param name="formContext">Form context.</param>
        /// <returns>String identifying "create" or "update" action.</returns>
        private string GetAction(string formContext)
        {
            string[] parts = formContext.Split('|');
            if (parts.Length > 2)
                throw new ArgumentException("Invalid form context");
            if (parts.Length == 1 && parts[0] != "create")
                throw new ArgumentException("Invalid form context");
            if (parts.Length == 2 && parts[0] != "update")
                throw new ArgumentException("Invalid form context");
            long masterPageId;
            if (parts.Length == 2 && !Int64.TryParse(parts[1], out masterPageId))
                throw new ArgumentException("Invalid form context");
            return parts[0];
        }

        /// <summary>
        /// Gets form for updating main master page details.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form for updating main master page details.</returns>
        private Form GetMasterPageForm(string context)
        {
            // Get tenant identifier
            long tenantId = _authenticationService.TenantId;

            // Get possible parent pages for ancestor page
            ISearchParameters searchParameters = new SearchParameters { PageIndex = 0, PageSize = 1000 }; // TODO: Need way to return all pages, not have some max bound upper limit
            ISearchResult<Page> result = _pageService.List(tenantId, searchParameters, null, PageSortBy.Name, true, true, PageType.Folder, false);

            // Construct view model
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("name", new TextField
            {
                Name = "name",
                Label = MasterPageResource.NameLabel,
                Required = true,
                RequiredErrorMessage = MasterPageResource.NameRequiredMessage,
                MaxLength = MasterPageLengths.NameMaxLength,
                MaxLengthErrorMessage = string.Format(MasterPageResource.NameMaxLengthMessage, "name", MasterPageLengths.NameMaxLength)
            });
            form.Fields.Add("pageName", new TextField
            {
                Name = "pageName",
                Label = MasterPageResource.PageNameLabel,
                Required = true,
                RequiredErrorMessage = MasterPageResource.PageNameRequiredMessage,
                MaxLength = MasterPageLengths.PageNameMaxLength,
                MaxLengthErrorMessage = string.Format(MasterPageResource.PageNameMaxLengthMessage, "pageName", MasterPageLengths.PageNameMaxLength)
            });
            form.Fields.Add("pageDescription", new MultiLineTextField
            {
                Name = "pageDescription",
                Label = MasterPageResource.PageDescriptionLabel,
                Rows = 4
            });
            form.Fields.Add("ancestorPageId", new SelectListField<string>
            {
                Name = "ancestorPageId",
                Label = MasterPageResource.AncestorPageIdLabel,
                Items = new List<ListFieldItem<string>> { new ListFieldItem<string> { Name = MasterPageResource.AncestorPageIdDefaultOption, Value = string.Empty } }
            });
            foreach (Page page in result.Items)
                ((SelectListField<string>)form.Fields["ancestorPageId"]).Items.Add(new ListFieldItem<string> { Name = page.Name, Value = page.PageId.ToString() });
            form.Fields.Add("ancestorPageLevel", new SelectListField<string>
            {
                Name = "ancestorPageLevel",
                Label = MasterPageResource.AncestorPageLevelLabel,
                Items = new List<ListFieldItem<string>> {
                    new ListFieldItem<string> { Name = MasterPageResource.AncestorPageLevelDefaultOption, Value = string.Empty },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageLevel>(PageLevel.Parent), Value = ((int)PageLevel.Parent).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageLevel>(PageLevel.Grandparent), Value = ((int)PageLevel.Grandparent).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageLevel>(PageLevel.GreatGrandparent), Value = ((int)PageLevel.GreatGrandparent).ToString() }
                },
                Required = true,
                RequiredErrorMessage = MasterPageResource.AncestorPageLevelRequiredMessage
            });
            form.Fields.Add("pageType", new SelectListField<string>
            {
                Name = "pageType",
                Label = MasterPageResource.PageTypeLabel,
                Items = new List<ListFieldItem<string>> {
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageType>(PageType.Document), Value = ((int)PageType.Document).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageType>(PageType.Folder), Value = ((int)PageType.Folder).ToString() }
                },
                Required = true,
                RequiredErrorMessage = MasterPageResource.PageTypeRequiredMessage
            });
            form.Fields.Add("hasOccurred", new BooleanField
            {
                Name = "hasOccurred",
                Label = MasterPageResource.HasOccurredLabel
            });
            form.Fields.Add("hasImage", new BooleanField
            {
                Name = "hasImage",
                Label = MasterPageResource.HasImageLabel
            });
            form.Fields.Add("thumbnailImageWidth", new IntegerField
            {
                Name = "thumbnailImageWidth",
                Label = MasterPageResource.ThumbnailImageWidthLabel,
                Min = MasterPageValues.MinThumbnailImageWidth,
                MinErrorMessage = string.Format(MasterPageResource.ThumbnailImageWidthMinErrorMessage, "thumbnailImageWidth", MasterPageValues.MinThumbnailImageWidth),
                Max = MasterPageValues.MaxThumbnailImageWidth,
                MaxErrorMessage = string.Format(MasterPageResource.ThumbnailImageWidthMaxErrorMessage, "thumbnailImageWidth", MasterPageValues.MaxThumbnailImageWidth),
                Required = true,
                RequiredErrorMessage = MasterPageResource.ThumbnailImageWidthRequiredMessage
            });
            form.Fields.Add("thumbnailImageHeight", new IntegerField
            {
                Name = "thumbnailImageHeight",
                Label = MasterPageResource.ThumbnailImageHeightLabel,
                Min = MasterPageValues.MinThumbnailImageHeight,
                MinErrorMessage = string.Format(MasterPageResource.ThumbnailImageHeightMinErrorMessage, "thumbnailImageHeight", MasterPageValues.MinThumbnailImageHeight),
                Max = MasterPageValues.MaxThumbnailImageHeight,
                MaxErrorMessage = string.Format(MasterPageResource.ThumbnailImageHeightMaxErrorMessage, "thumbnailImageHeight", MasterPageValues.MaxThumbnailImageHeight),
                Required = true,
                RequiredErrorMessage = MasterPageResource.ThumbnailImageHeightRequiredMessage
            });
            form.Fields.Add("thumbnailImageResizeMode", new SelectListField<string>
            {
                Name = "thumbnailImageResizeMode",
                Label = MasterPageResource.ThumbnailImageResizeModeLabel,
                Items = new List<ListFieldItem<string>> {
                    new ListFieldItem<string> { Name = MasterPageResource.ResizeModeDefaultOption, Value = string.Empty },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<ResizeMode>(ResizeMode.Simple), Value = ((int)ResizeMode.Simple).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<ResizeMode>(ResizeMode.MaintainAspect), Value = ((int)ResizeMode.MaintainAspect).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<ResizeMode>(ResizeMode.Crop), Value = ((int)ResizeMode.Crop).ToString() }
                },
                Required = true,
                RequiredErrorMessage = MasterPageResource.ThumbnailImageResizeModeRequiredMessage
            });
            form.Fields.Add("previewImageWidth", new IntegerField
            {
                Name = "previewImageWidth",
                Label = MasterPageResource.PreviewImageWidthLabel,
                Min = MasterPageValues.MinPreviewImageWidth,
                MinErrorMessage = string.Format(MasterPageResource.PreviewImageWidthMinErrorMessage, "previewImageWidth", MasterPageValues.MinPreviewImageWidth),
                Max = MasterPageValues.MaxPreviewImageWidth,
                MaxErrorMessage = string.Format(MasterPageResource.PreviewImageWidthMaxErrorMessage, "previewImageWidth", MasterPageValues.MaxPreviewImageWidth),
                Required = true,
                RequiredErrorMessage = MasterPageResource.PreviewImageWidthRequiredMessage
            });
            form.Fields.Add("previewImageHeight", new IntegerField
            {
                Name = "previewImageHeight",
                Label = MasterPageResource.PreviewImageHeightLabel,
                Min = MasterPageValues.MinPreviewImageHeight,
                MinErrorMessage = string.Format(MasterPageResource.PreviewImageHeightMinErrorMessage, "previewImageHeight", MasterPageValues.MinPreviewImageHeight),
                Max = MasterPageValues.MaxPreviewImageHeight,
                MaxErrorMessage = string.Format(MasterPageResource.PreviewImageHeightMaxErrorMessage, "previewImageHeight", MasterPageValues.MaxPreviewImageHeight),
                Required = true,
                RequiredErrorMessage = MasterPageResource.PreviewImageHeightRequiredMessage
            });
            form.Fields.Add("previewImageResizeMode", new SelectListField<string>
            {
                Name = "previewImageResizeMode",
                Label = MasterPageResource.PreviewImageResizeModeLabel,
                Items = new List<ListFieldItem<string>> {
                    new ListFieldItem<string> { Name = MasterPageResource.ResizeModeDefaultOption, Value = string.Empty },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<ResizeMode>(ResizeMode.Simple), Value = ((int)ResizeMode.Simple).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<ResizeMode>(ResizeMode.MaintainAspect), Value = ((int)ResizeMode.MaintainAspect).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<ResizeMode>(ResizeMode.Crop), Value = ((int)ResizeMode.Crop).ToString() }
                },
                Required = true,
                RequiredErrorMessage = MasterPageResource.PreviewImageResizeModeRequiredMessage
            });
            form.Fields.Add("imageMinWidth", new IntegerField
            {
                Name = "imageMinWidth",
                Label = MasterPageResource.ImageMinWidthLabel,
                Min = MasterPageValues.MinImageMinWidth,
                MinErrorMessage = string.Format(MasterPageResource.ImageMinWidthMinErrorMessage, "imageMinWidth", MasterPageValues.MinImageMinWidth),
                Max = MasterPageValues.MaxImageMinWidth,
                MaxErrorMessage = string.Format(MasterPageResource.ImageMinWidthMaxErrorMessage, "imageMinWidth", MasterPageValues.MaxImageMinWidth),
                Required = true,
                RequiredErrorMessage = MasterPageResource.ImageMinWidthRequiredMessage
            });
            form.Fields.Add("imageMinHeight", new IntegerField
            {
                Name = "imageMinHeight",
                Label = MasterPageResource.ImageMinHeightLabel,
                Min = MasterPageValues.MinImageMinHeight,
                MinErrorMessage = string.Format(MasterPageResource.ImageMinHeightMinErrorMessage, "imageMinHeight", MasterPageValues.MinImageMinHeight),
                Max = MasterPageValues.MaxImageMinWidth,
                MaxErrorMessage = string.Format(MasterPageResource.ImageMinHeightMaxErrorMessage, "imageMinHeight", MasterPageValues.MaxImageMinHeight),
                Required = true,
                RequiredErrorMessage = MasterPageResource.ImageMinHeightRequiredMessage
            });
            form.Fields.Add("creatable", new BooleanField
            {
                Name = "creatable",
                Label = MasterPageResource.CreatableLabel
            });
            form.Fields.Add("deletable", new BooleanField
            {
                Name = "deletable",
                Label = MasterPageResource.DeletableLabel
            });
            form.Fields.Add("taggable", new BooleanField
            {
                Name = "taggable",
                Label = MasterPageResource.TaggableLabel
            });
            form.Fields.Add("administration", new BooleanField
            {
                Name = "administration",
                Label = MasterPageResource.AdministrationLabel
            });
            form.Fields.Add("beginRender", new MultiLineTextField
            {
                Name = "beginRender",
                Label = MasterPageResource.BeginRenderLabel,
                Rows = 4
            });
            form.Fields.Add("endRender", new MultiLineTextField
            {
                Name = "endRender",
                Label = MasterPageResource.EndRenderLabel,
                Rows = 4
            });

            // Return form
            return form;
        }

        /// <summary>
        /// Gets form for creating master page.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form for creating a new master page.</returns>
        private Form GetCreateMasterPageForm(string context)
        {
            Form form = GetMasterPageForm(context);
            form.SubmitLabel = MasterPageResource.CreateMasterPageButtonLabel;
            form.Data = JsonConvert.SerializeObject(new MasterPage());
            return form;
        }

        /// <summary>
        /// Gets form for updating existing master page.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form for creating a new master page.</returns>
        private Form GetUpdateMasterPageForm(string context)
        {
            // Get identifier of master page that is being updated
            string[] parts = context.Split('|');
            long masterPageId = Convert.ToInt64(parts[1]);

            // Get existing master page details
            long tenantId = _authenticationService.TenantId;
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId);

            // Get form
            Form form = GetMasterPageForm(context);
            form.SubmitLabel = MasterPageResource.UpdateMasterPageButtonLabel;
            form.Data = JsonConvert.SerializeObject(masterPage);
            return form;
        }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        public Form GetForm(string context)
        {
            // Get action
            string action = GetAction(context);

            // Get form based on action
            switch (action)
            {
                case "create":
                    _authorizationService.AuthorizeUserForFunction(Functions.CreateMasterPages);
                    return GetCreateMasterPageForm(context);

                case "update":
                    _authorizationService.AuthorizeUserForFunction(Functions.UpdateMasterPages);
                    return GetUpdateMasterPageForm(context);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Performs create of master page given submitted form data.
        /// </summary>
        /// <param name="form">Form containing master page to create.</param>
        /// <returns>Result of form post.</returns>
        private FormResult CreateMasterPage(Form form)
        {
            // Get master page details
            long tenantId = _authenticationService.TenantId;
            MasterPage masterPage = JsonConvert.DeserializeObject<MasterPage>(form.Data);
            masterPage.TenantId = tenantId;
            masterPage.MasterPageZones = new List<MasterPageZone>();

            // Do the update
            _masterPageService.Create(masterPage);

            // Return form result with no errors
            return _formHelperService.GetFormResult();
        }

        /// <summary>
        /// Performs update of master page details given submitted form data.
        /// </summary>
        /// <param name="form">Form containing updated master page zones data.</param>
        /// <returns>Result of form post.</returns>
        private FormResult UpdateMasterPage(Form form)
        {
            // Get identifier of master page that is being updated
            string[] parts = form.Context.Split('|');
            long masterPageId = Convert.ToInt64(parts[1]);

            // Get master page details
            long tenantId = _authenticationService.TenantId;
            MasterPage masterPage = JsonConvert.DeserializeObject<MasterPage>(form.Data);
            masterPage.TenantId = tenantId;
            masterPage.MasterPageId = masterPageId;

            // Do the update
            _masterPageService.Update(masterPage);

            // Return form result with no errors
            return _formHelperService.GetFormResult();
        }

        /// <summary>
        /// Submits form.
        /// </summary>
        /// <param name="form">View model containing form definition and submitted values.</param>
        /// <returns>Result of form post.</returns>
        public FormResult PostForm(Form form)
        {
            try
            {
                // Get action
                string action = GetAction(form.Context);

                // Perform action based on form context
                switch (action)
                {
                    case "create":
                        _authorizationService.AuthorizeUserForFunction(Functions.CreateMasterPages);
                        return CreateMasterPage(form);

                    case "update":
                        _authorizationService.AuthorizeUserForFunction(Functions.UpdateMasterPages);
                        return UpdateMasterPage(form);

                    default:
                        return null;
                }

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
