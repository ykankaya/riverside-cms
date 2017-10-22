using System;
using System.Collections.Generic;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Html
{
    public class HtmlAdminFormService : IFormService, IFormUploadService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IHtmlUrlService _htmlUrlService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;

        public HtmlAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IHtmlUrlService htmlUrlService, IElementFactory elementFactory, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _htmlUrlService = htmlUrlService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
        }

        public Guid FormId { get { return new Guid("c92ee4c4-b133-44cc-8322-640e99c334dc"); } }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current HTML settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            HtmlSettings htmlSettings = (HtmlSettings)elementService.New(_authenticationService.TenantId);
            htmlSettings.ElementId = elementId;
            elementService.Read(htmlSettings);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("html", new MultiLineTextField
            {
                Name = "html",
                Label = ElementResource.HtmlHtmlLabel,
                Value = htmlSettings.Html,
                Rows = 15
            });
            form.Fields.Add("upload", new UploadField
            {
                Name = "upload",
                Label = ElementResource.HtmlUploadLabel
            });
            form.SubmitLabel = ElementResource.HtmlButtonLabel;

            // Return result
            return form;
        }

        public FormResult PostUpload(string id, string context, string name, string contentType, byte[] content)
        {
            try
            {
                // Check permissions
                _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

                // Get page and element identifiers
                string[] parts = context.Split('|');
                long pageId = Convert.ToInt64(parts[0]);
                long elementId = Convert.ToInt64(parts[1]);

                // Get upload model
                long tenantId = _authenticationService.TenantId;
                CreateUploadModel model = new CreateUploadModel
                {
                    Content = content,
                    ContentType = contentType,
                    Name = name,
                    TenantId = tenantId
                };

                // Create uploads, ready to be assigned to HTML element when form submitted
                IUploadElementService htmlService = (IUploadElementService)_elementFactory.GetElementService(FormId);
                long htmlUploadId = htmlService.PrepareImages(tenantId, elementId, model);

                // Return form result
                string status = _htmlUrlService.GetHtmlUploadUrl(elementId, htmlUploadId);
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

                // Get the HTML element service
                IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

                // Get updated HTML settings
                HtmlSettings htmlSettings = (HtmlSettings)elementService.New(_authenticationService.TenantId);
                htmlSettings.ElementId = elementId;
                htmlSettings.Html = ((MultiLineTextField)form.Fields["html"]).Value;

                // Perform the update
                elementService.Update(htmlSettings);

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
