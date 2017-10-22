using System;
using System.Collections.Generic;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Annotations;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.CodeSnippets
{
    public class CodeSnippetAdminFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IDataAnnotationsService _dataAnnotationsService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;

        public CodeSnippetAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IDataAnnotationsService dataAnnotationsService, IElementFactory elementFactory, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _dataAnnotationsService = dataAnnotationsService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
        }

        public Guid FormId { get { return new Guid("5401977d-865f-4a7a-b416-0a26305615de"); } }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current code snippet settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            CodeSnippetSettings codeSnippetSettings = (CodeSnippetSettings)elementService.New(_authenticationService.TenantId);
            codeSnippetSettings.ElementId = elementId;
            elementService.Read(codeSnippetSettings);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("code", new MultiLineTextField
            {
                Name = "code",
                Label = ElementResource.CodeSnippetCodeLabel,
                Value = codeSnippetSettings.Code,
                Rows = 10,
                Required = true,
                RequiredErrorMessage = ElementResource.CodeSnippetCodeRequiredMessage
            });

            form.Fields.Add("language", new SelectListField<string>
            {
                Name = "language",
                Label = ElementResource.CodeSnippetLanguageLabel,
                Value = Convert.ToString((int)codeSnippetSettings.Language),
                Items = new List<ListFieldItem<string>>()
            });
            SelectListField<string> selectListField = (SelectListField<string>)form.Fields["language"];
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Apache), Value = Convert.ToString((int)Language.Apache) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Bash), Value = Convert.ToString((int)Language.Bash) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.CoffeeScript), Value = Convert.ToString((int)Language.CoffeeScript) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.CPlusPlus), Value = Convert.ToString((int)Language.CPlusPlus) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.CSharp), Value = Convert.ToString((int)Language.CSharp) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Css), Value = Convert.ToString((int)Language.Css) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Diff), Value = Convert.ToString((int)Language.Diff) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Html), Value = Convert.ToString((int)Language.Html) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Http), Value = Convert.ToString((int)Language.Http) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Ini), Value = Convert.ToString((int)Language.Ini) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Java), Value = Convert.ToString((int)Language.Java) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.JavaScript), Value = Convert.ToString((int)Language.JavaScript) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Json), Value = Convert.ToString((int)Language.Json) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Makefile), Value = Convert.ToString((int)Language.Makefile) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Markdown), Value = Convert.ToString((int)Language.Markdown) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Nginx), Value = Convert.ToString((int)Language.Nginx) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.ObjectiveC), Value = Convert.ToString((int)Language.ObjectiveC) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Perl), Value = Convert.ToString((int)Language.Perl) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Php), Value = Convert.ToString((int)Language.Php) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Python), Value = Convert.ToString((int)Language.Python) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Ruby), Value = Convert.ToString((int)Language.Ruby) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Sql), Value = Convert.ToString((int)Language.Sql) });
            selectListField.Items.Add(new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<Language>(Language.Xml), Value = Convert.ToString((int)Language.Xml) });
            form.SubmitLabel = ElementResource.CodeSnippetButtonLabel;

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

                // Get the code snippet element service
                IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

                // Get updated code snippet settings
                CodeSnippetSettings codeSnippetSettings = (CodeSnippetSettings)elementService.New(_authenticationService.TenantId);
                codeSnippetSettings.ElementId = elementId;
                codeSnippetSettings.Code = ((MultiLineTextField)form.Fields["code"]).Value;
                codeSnippetSettings.Language = (Language)Convert.ToInt32(((SelectListField<string>)form.Fields["language"]).Value);

                // Perform the update
                elementService.Update(codeSnippetSettings);

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
