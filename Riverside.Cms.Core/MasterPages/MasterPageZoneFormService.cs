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
    /// Form and element service for master page zone administration.
    /// </summary>
    public class MasterPageZoneFormService : IFormService, IBasicElementService
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
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="dataAnnotationsService">Retrieves information from data annotations.</param>
        /// <param name="elementService">Used to enumerate elements.</param>
        /// <param name="formHelperService">Provides form helper utilities.</param>
        /// <param name="masterPageService">For administration of master pages.</param>
        public MasterPageZoneFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IDataAnnotationsService dataAnnotationsService, IElementService elementService, IFormHelperService formHelperService, IMasterPageService masterPageService)
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
                return new Guid("24281fa2-edad-4af9-9f60-e4fc869061c5");
            }
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId
        {
            get
            {
                return new Guid("24281fa2-edad-4af9-9f60-e4fc869061c5");
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
            return new ElementInfo<ElementSettings, MasterPageZoneContent> { Settings = settings, Content = content };
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
            long masterPageZoneId = Convert.ToInt64(pageContext.RouteValues["masterpagezoneid"]);
            return new MasterPageZoneContent { PartialViewName = "UpdateMasterPageZoneAdmin", MasterPageId = masterPageId, MasterPageZoneId = masterPageZoneId };
        }

        /// <summary>
        /// Gets items used to populate element select list field.
        /// </summary>
        /// <param name="masterPages">Master page collection.</param>
        /// <returns>List of field items.</returns>
        private List<ListFieldItem<string>> GetElementIdListFieldItems(IEnumerable<MasterPage> masterPages)
        {
            List<ListFieldItem<string>> items = new List<ListFieldItem<string>>();
            items.Add(new ListFieldItem<string> { Name = MasterPageResource.ElementIdDefaultOption, Value = string.Empty });
            foreach (MasterPage masterPage in masterPages)
            {
                foreach (MasterPageZone masterPageZone in masterPage.MasterPageZones)
                {
                    foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
                    {
                        string name = string.Format("{0} / {1} / {2}", masterPage.Name, masterPageZone.Name, masterPageZoneElement.Element.Name);
                        string value = string.Format("{0}|{1}|{2}|{3}", masterPage.MasterPageId, masterPageZone.MasterPageZoneId, masterPageZoneElement.MasterPageZoneElementId, masterPageZoneElement.ElementId);
                        items.Add(new ListFieldItem<string> { Name = name, Value = value });
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Get items used to populate element type select list field.
        /// </summary>
        /// <param name="elementTypes">The different types of element.</param>
        /// <returns>List of field items.</returns>
        private List<ListFieldItem<string>> GetElementTypeListFieldItems(IEnumerable<ElementType> elementTypes)
        {
            List<ListFieldItem<string>> items = new List<ListFieldItem<string>>();
            items.Add(new ListFieldItem<string> { Name = MasterPageResource.ElementTypeIdDefaultOption, Value = string.Empty });
            foreach (ElementType elementType in elementTypes)
                items.Add(new ListFieldItem<string> { Name = elementType.Name, Value = elementType.ElementTypeId.ToString() });
            return items;
        }

        /// <summary>
        /// Retrieves form for creating or updating master page zone elements.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form object.</returns>
        private Form GetMasterPageZoneElementForm(string context)
        {
            // Construct form data, consisting of master page zone element form action button labels
            MasterPageZoneElementFormData formData = new MasterPageZoneElementFormData { Labels = new Dictionary<string, string>() };
            formData.Labels.Add("update", MasterPageResource.UpdateZoneElementButtonLabel);
            formData.Labels.Add("create", MasterPageResource.CreateZoneElementButtonLabel);
            formData.MasterPageZoneElement = new MasterPageZoneElement();

            // Get all elements
            long tenantId = _authenticationService.TenantId;
            IEnumerable<MasterPage> masterPages = _masterPageService.ListElementsByMasterPage(tenantId);

            // Construct form
            string data = JsonConvert.SerializeObject(formData);
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, Data = data };
            form.Fields.Add("elementId", new SelectListField<string>
            {
                Name = "elementId",
                Label = MasterPageResource.ElementIdLabel,
                Items = GetElementIdListFieldItems(masterPages),
                Required = true,
                RequiredErrorMessage = MasterPageResource.ElementIdRequiredMessage
            });
            form.Fields.Add("beginRender", new MultiLineTextField
            {
                Name = "beginRenderZoneElement",
                Label = MasterPageResource.ZoneElementBeginRenderLabel,
                Rows = 4
            });
            form.Fields.Add("endRender", new MultiLineTextField
            {
                Name = "endRenderZoneElement",
                Label = MasterPageResource.ZoneElementEndRenderLabel,
                Rows = 4
            });

            // Return result
            return form;
        }

        /// <summary>
        /// Retrieves form for creating or updating master page zone elements that will be created on form submission.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form object.</returns>
        private Form GetMasterPageZoneElementNewForm(string context)
        {
            // Construct form data, consisting of master page zone element form action button labels
            MasterPageZoneElementFormData formData = new MasterPageZoneElementFormData { Labels = new Dictionary<string, string>() };
            formData.Labels.Add("update", MasterPageResource.UpdateZoneElementNewButtonLabel);
            formData.Labels.Add("create", MasterPageResource.CreateZoneElementNewButtonLabel);

            // Get all elements
            long tenantId = _authenticationService.TenantId;
            IEnumerable<ElementType> elementTypes = _elementService.ListTypes();

            // Construct form
            string data = JsonConvert.SerializeObject(formData);
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, Data = data };
            form.Fields.Add("elementType", new SelectListField<string>
            {
                Name = "elementType",
                Label = MasterPageResource.ElementTypeLabel,
                Items = GetElementTypeListFieldItems(elementTypes),
                Required = true,
                RequiredErrorMessage = MasterPageResource.ElementTypeRequiredMessage
            });
            form.Fields.Add("name", new TextField
            {
                Name = "name",
                Label = MasterPageResource.ElementNameLabel,
                Required = true,
                RequiredErrorMessage = MasterPageResource.ElementNameRequiredMessage,
                MaxLength = ElementLengths.NameMaxLength,
                MaxLengthErrorMessage = string.Format(MasterPageResource.ElementNameMaxLengthMessage, "name", ElementLengths.NameMaxLength)
            });
            form.Fields.Add("beginRender", new MultiLineTextField
            {
                Name = "beginRenderZoneElement",
                Label = MasterPageResource.ZoneElementBeginRenderLabel,
                Rows = 4
            });
            form.Fields.Add("endRender", new MultiLineTextField
            {
                Name = "endRenderZoneElement",
                Label = MasterPageResource.ZoneElementEndRenderLabel,
                Rows = 4
            });

            // Return result
            return form;
        }

        /// <summary>
        /// Retrieves form for creating or updating master page zone element types.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form object.</returns>
        private Form GetMasterPageZoneElementTypeForm(string context)
        {
            // Construct form data, consisting of master page zone element type form action button labels
            MasterPageZoneElementTypeFormData formData = new MasterPageZoneElementTypeFormData { Labels = new Dictionary<string, string>() };
            formData.Labels.Add("update", MasterPageResource.UpdateZoneElementTypeButtonLabel);
            formData.Labels.Add("create", MasterPageResource.CreateZoneElementTypeButtonLabel);
            formData.MasterPageZoneElementType = new MasterPageZoneElementType();

            // Get all element types
            long tenantId = _authenticationService.TenantId;
            IEnumerable<ElementType> elementTypes = _elementService.ListTypes();

            // Construct form
            string data = JsonConvert.SerializeObject(formData);
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, Data = data };
            form.Fields.Add("elementTypeId", new SelectListField<string>
            {
                Name = "elementTypeId",
                Label = MasterPageResource.ElementTypeIdLabel,
                Items = new List<ListFieldItem<string>> { new ListFieldItem<string> { Name = MasterPageResource.ElementTypeIdDefaultOption, Value = string.Empty } },
                Required = true,
                RequiredErrorMessage = MasterPageResource.ElementTypeIdRequiredMessage
            });
            foreach (ElementType elementType in elementTypes)
                ((SelectListField<string>)form.Fields["elementTypeId"]).Items.Add(new ListFieldItem<string> { Name = elementType.Name, Value = elementType.ElementTypeId.ToString() });

            // Return result
            return form;
        }

        /// <summary>
        /// Retrieves form for master page zone update.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form object.</returns>
        private Form GetMasterPageZoneForm(string context)
        {
            // Get tenant identifier
            long tenantId = _authenticationService.TenantId;

            // Get identifier of master page zone that is being updated
            string[] parts = context.Split('|');
            long masterPageId = Convert.ToInt64(parts[0]);
            long masterPageZoneId = Convert.ToInt64(parts[1]);

            // Get existing master page details
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId);
            MasterPageZone masterPageZone = masterPage.MasterPageZones.Where(mpz => mpz.MasterPageZoneId == masterPageZoneId).FirstOrDefault();

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, SubmitLabel = MasterPageResource.UpdateZoneButtonLabel };
            form.Fields.Add("name", new TextField
            {
                Name = "zoneName",
                Label = MasterPageResource.ZoneNameLabel,
                Required = true,
                RequiredErrorMessage = MasterPageResource.ZoneNameRequiredMessage,
                MaxLength = MasterPageLengths.ZoneNameMaxLength,
                MaxLengthErrorMessage = string.Format(MasterPageResource.ZoneNameMaxLengthMessage, "name", MasterPageLengths.ZoneNameMaxLength)
            });
            form.Fields.Add("adminType", new SelectListField<string>
            {
                Name = "adminType",
                Label = MasterPageResource.AdminTypeLabel,
                Items = new List<ListFieldItem<string>> {
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<MasterPageZoneAdminType>(MasterPageZoneAdminType.Static), Value = ((int)MasterPageZoneAdminType.Static).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<MasterPageZoneAdminType>(MasterPageZoneAdminType.Editable), Value = ((int)MasterPageZoneAdminType.Editable).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<MasterPageZoneAdminType>(MasterPageZoneAdminType.Configurable), Value = ((int)MasterPageZoneAdminType.Configurable).ToString() }
                },
                Required = true,
                RequiredErrorMessage = MasterPageResource.AdminTypeRequiredMessage
            });
            form.Fields.Add("contentType", new SelectListField<string>
            {
                Name = "contentType",
                Label = MasterPageResource.ContentTypeLabel,
                Items = new List<ListFieldItem<string>> {
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<MasterPageZoneContentType>(MasterPageZoneContentType.Standard), Value = ((int)MasterPageZoneContentType.Standard).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<MasterPageZoneContentType>(MasterPageZoneContentType.Main), Value = ((int)MasterPageZoneContentType.Main).ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<MasterPageZoneContentType>(MasterPageZoneContentType.Comment), Value = ((int)MasterPageZoneContentType.Comment).ToString() }
                },
                Required = true,
                RequiredErrorMessage = MasterPageResource.ContentTypeRequiredMessage
            });
            form.Fields.Add("beginRender", new MultiLineTextField
            {
                Name = "beginRenderZone",
                Label = MasterPageResource.ZoneBeginRenderLabel,
                Rows = 4
            });
            form.Fields.Add("endRender", new MultiLineTextField
            {
                Name = "endRenderZone",
                Label = MasterPageResource.ZoneEndRenderLabel,
                Rows = 4,
                Value = masterPageZone.EndRender
            });

            // Create sub forms
            form.SubForms = new Dictionary<string, Form>();
            form.SubForms.Add("zoneElement", GetMasterPageZoneElementForm(context));
            form.SubForms.Add("zoneElementNew", GetMasterPageZoneElementNewForm(context));
            form.SubForms.Add("zoneElementType", GetMasterPageZoneElementTypeForm(context));

            // Set master page zone as form data
            form.Data = JsonConvert.SerializeObject(masterPageZone);

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
            return GetMasterPageZoneForm(context);
        }

        /// <summary>
        /// Performs update of master page zone given submitted form data.
        /// </summary>
        /// <param name="form">Form containing updated master page zone data.</param>
        /// <returns>Result of form post.</returns>
        private FormResult UpdateMasterPageZone(Form form)
        {
            // Get master page details
            string[] parts = form.Context.Split('|');
            long tenantId = _authenticationService.TenantId;
            long masterPageId = Convert.ToInt64(parts[0]);
            long masterPageZoneId = Convert.ToInt64(parts[1]);
            MasterPageZone masterPageZone = JsonConvert.DeserializeObject<MasterPageZone>(form.Data);
            masterPageZone.TenantId = tenantId;
            masterPageZone.MasterPageId = masterPageId;
            masterPageZone.MasterPageZoneId = masterPageZoneId;

            // Do the update
            _masterPageService.UpdateZone(masterPageZone);

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
                FormResult formResult = UpdateMasterPageZone(form);

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
