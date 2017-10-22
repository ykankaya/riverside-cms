using System;
using System.Collections.Generic;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Footers
{
    public class FooterAdminFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;

        public FooterAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IElementFactory elementFactory, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
        }

        public Guid FormId { get { return new Guid("f1c2b384-4909-47c8-ada7-cd3cc7f32620"); } }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current footer settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            FooterSettings footerSettings = (FooterSettings)elementService.New(_authenticationService.TenantId);
            footerSettings.ElementId = elementId;
            elementService.Read(footerSettings);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("message", new MultiLineTextField
            {
                Name = "message",
                Label = ElementResource.FooterMessageLabel,
                Value = footerSettings.Message,
                Rows = 10
            });
            form.Fields.Add("showLoggedOffUserOptions", new BooleanField
            {
                Name = "showLoggedOffUserOptions",
                Label = ElementResource.ShowLoggedOffUserOptionsLabel,
                Value = footerSettings.ShowLoggedOffUserOptions
            });
            form.Fields.Add("showLoggedOnUserOptions", new BooleanField
            {
                Name = "showLoggedOnUserOptions",
                Label = ElementResource.ShowLoggedOnUserOptionsLabel,
                Value = footerSettings.ShowLoggedOnUserOptions
            });
            form.SubmitLabel = ElementResource.FooterButtonLabel;

            // Return result
            return form;
        }

        public FormResult PostForm(Form form)
        {
            try
            {
                // Check permissions
                _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

                // Get page and element identifiers
                string[] parts = form.Context.Split('|');
                long pageId = Convert.ToInt64(parts[0]);
                long elementId = Convert.ToInt64(parts[1]);

                // Get the footer element service
                IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

                // Get updated footer settings
                FooterSettings footerSettings = (FooterSettings)elementService.New(_authenticationService.TenantId);
                footerSettings.ElementId = elementId;
                footerSettings.Message = string.IsNullOrWhiteSpace(((MultiLineTextField)form.Fields["message"]).Value) ? null : ((MultiLineTextField)form.Fields["message"]).Value;
                footerSettings.ShowLoggedOffUserOptions = ((BooleanField)form.Fields["showLoggedOffUserOptions"]).Value;
                footerSettings.ShowLoggedOnUserOptions = ((BooleanField)form.Fields["showLoggedOnUserOptions"]).Value;

                // Perform the update
                elementService.Update(footerSettings);

                // Return form result with no errors
                return _formHelperService.GetFormResult();
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
