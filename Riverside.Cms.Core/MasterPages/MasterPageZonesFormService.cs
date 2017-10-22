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
using Riverside.Utilities.Validation;
using Riverside.UI.Forms;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Form and element service for master page zones administration.
    /// </summary>
    public class MasterPageZonesFormService : IFormService, IBasicElementService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IDataAnnotationsService _dataAnnotationsService;
        private IElementService _elementService;
        private IFormHelperService _formHelperService;
        private IMasterPageService _masterPageService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Provides access to authentication services.</param>
        /// <param name="authorizationService">Provides access to authentication services.</param>
        /// <param name="dataAnnotationsService">Retrieves information from data annotations.</param>
        /// <param name="elementService">Used to enumerate elements.</param>
        /// <param name="formHelperService">Provides form helper utilities.</param>
        /// <param name="masterPageService">For administration of master pages.</param>
        public MasterPageZonesFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IDataAnnotationsService dataAnnotationsService, IElementService elementService, IFormHelperService formHelperService, IMasterPageService masterPageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _dataAnnotationsService = dataAnnotationsService;
            _elementService = elementService;
            _formHelperService = formHelperService;
            _masterPageService = masterPageService;
        }

        /// <summary>
        /// Returns GUID, identifying the type of element that this custom element service is associated with.
        /// </summary>
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("6e628c1b-7876-436c-b2d0-ac6a4859d507");
            }
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId
        {
            get
            {
                return new Guid("6e628c1b-7876-436c-b2d0-ac6a4859d507");
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
            long masterPageId = Convert.ToInt64(pageContext.RouteValues["masterpageid"]);
            return new MasterPageContent { PartialViewName = "UpdateMasterPageZonesAdmin", FormContext = masterPageId.ToString() };
        }

        /// <summary>
        /// Gets form for update or creation of zone (restricted to editing name).
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form or update or creation of zone.</returns>
        private Form GetMasterPageZoneForm(string context)
        {
            // Construct form data, consisting of master page zone form action button labels
            MasterPageZoneFormData formData = new MasterPageZoneFormData { Labels = new Dictionary<string, string>() };
            formData.Labels.Add("update", MasterPageResource.UpdateZoneButtonLabel);
            formData.Labels.Add("create", MasterPageResource.CreateZoneButtonLabel);
            formData.MasterPageZone = new MasterPageZone { Name = string.Empty };

            // Construct form
            string data = JsonConvert.SerializeObject(formData);
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, Data = data };
            form.Fields.Add("name", new TextField
            {
                Name = "zoneName",
                Label = MasterPageResource.ZoneNameLabel,
                Required = true,
                RequiredErrorMessage = MasterPageResource.ZoneNameRequiredMessage,
                MaxLength = MasterPageLengths.ZoneNameMaxLength,
                MaxLengthErrorMessage = string.Format(MasterPageResource.ZoneNameMaxLengthMessage, "name", MasterPageLengths.ZoneNameMaxLength)
            });

            // Return result
            return form;
        }

        /// <summary>
        /// Retrieves form for master page zones update.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form object.</returns>
        private Form GetMasterPageZonesForm(string context)
        {
            // Get tenant identifier
            long tenantId = _authenticationService.TenantId;

            // Get identifier of master page that is being updated
            string[] parts = context.Split('|');
            long masterPageId = Convert.ToInt64(parts[0]);

            // Get existing master page details
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId);

            // Construct view model
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.SubmitLabel = MasterPageResource.UpdateZonesButtonLabel;

            // Create sub forms
            form.SubForms = new Dictionary<string, Form>();
            form.SubForms.Add("zone", GetMasterPageZoneForm(context));

            // Set master page as form data
            form.Data = JsonConvert.SerializeObject(masterPage.MasterPageZones);

            // Return form
            return form;
        }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdateMasterPages);

            // The form that we will return
            return GetMasterPageZonesForm(context);
        }

        /// <summary>
        /// Performs update of master page zones given submitted form data.
        /// </summary>
        /// <param name="form">Form containing updated master page zones data.</param>
        /// <returns>Result of form post.</returns>
        private FormResult UpdateMasterPageZones(Form form)
        {
            // Get master page details
            string[] parts = form.Context.Split('|');
            long tenantId = _authenticationService.TenantId;
            long masterPageId = Convert.ToInt64(parts[0]);
            List<MasterPageZone> masterPageZones = JsonConvert.DeserializeObject<List<MasterPageZone>>(form.Data);

            // Do the update
            _masterPageService.UpdateZones(tenantId, masterPageId, masterPageZones);

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
                // Check permissions
                _authorizationService.AuthorizeUserForFunction(Functions.UpdateMasterPages);

                // The form result
                FormResult formResult = UpdateMasterPageZones(form);

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
    }
}
