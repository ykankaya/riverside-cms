using System;
using System.Collections.Generic;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Forms
{
    public class FormFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;

        public FormFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IElementFactory elementFactory, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
        }

        public Guid FormId { get { return new Guid("eafbd5ab-8c98-4edc-b8e1-42f5e8bfe2dc"); } }

        private Form GetUserForm(string context)
        {
            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);

            // Get form settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            FormSettings formSettings = (FormSettings)elementService.New(_authenticationService.TenantId);
            formSettings.ElementId = elementId;
            elementService.Read(formSettings);

            // Construct form
            Form form = new Form
            {
                FieldSets = new List<FormFieldSet>(),
                Id = FormId.ToString(),
                Context = context,
                SubmitLabel = formSettings.SubmitButtonLabel
            };

            // Populate fields from form settings
            foreach (FormElementField formElementField in formSettings.Fields)
            {
                FormFieldSet fieldSet = new FormFieldSet { Fields = new Dictionary<string, IFormField>() };
                switch (formElementField.FieldType)
                {
                    case FormElementFieldType.TextField:
                        fieldSet.Fields.Add("field", new TextField
                        {
                            Name = "field_" + formElementField.FormFieldId,
                            Label = formElementField.Label,
                            Required = formElementField.Required,
                            RequiredErrorMessage = string.Format(ElementResource.FormFieldRequiredMessage, formElementField.Label)
                        });
                        break;

                    case FormElementFieldType.MultiLineTextField:
                        fieldSet.Fields.Add("field", new MultiLineTextField
                        {
                            Name = "field_" + formElementField.FormFieldId,
                            Label = formElementField.Label,
                            Required = formElementField.Required,
                            RequiredErrorMessage = string.Format(ElementResource.FormFieldRequiredMessage, formElementField.Label),
                            Rows = 5
                        });
                        break;
                }
                form.FieldSets.Add(fieldSet);
            }

            // Return form
            return form;
        }

        private Form GetAdminForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);

            // Get current form settings
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);
            FormSettings formSettings = (FormSettings)elementService.New(_authenticationService.TenantId);
            formSettings.ElementId = elementId;
            elementService.Read(formSettings);

            // Construct main form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, FieldSets = new List<FormFieldSet>() };
            form.Fields.Add("recipientEmail", new MultiLineTextField
            {
                Name = "recipientEmail",
                Label = ElementResource.FormRecipientEmailLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.FormRecipientEmailRequiredMessage,
                Value = formSettings.RecipientEmail,
                Rows = 4
            });
            form.Fields.Add("submitButtonLabel", new TextField
            {
                Name = "submitButtonLabel",
                Label = ElementResource.FormSubmitButtonLabelLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.FormSubmitButtonLabelRequiredMessage,
                MaxLength = FormLengths.SubmitButtonLabelMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.FormSubmitButtonLabelMaxLengthMessage, "submitButtonLabel", FormLengths.SubmitButtonLabelMaxLength),
                Value = formSettings.SubmitButtonLabel
            });
            form.Fields.Add("submittedMessage", new TextField
            {
                Name = "submittedMessage",
                Label = ElementResource.FormSubmittedMessageLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.FormSubmittedMessageRequiredMessage,
                Value = formSettings.SubmittedMessage
            });
            form.SubmitLabel = ElementResource.FormButtonLabel;

            // Add form fields
            foreach (FormElementField field in formSettings.Fields)
            {
                FormFieldSet fieldSet = new FormFieldSet { Fields = new Dictionary<string, IFormField>() };
                fieldSet.Fields.Add("label", new TextField
                {
                    Name = string.Format("field_{0}_label", field.FormFieldId),
                    Label = ElementResource.FormFieldLabelLabel,
                    Required = true,
                    RequiredErrorMessage = ElementResource.FormFieldLabelRequiredMessage,
                    Value = field.Label
                });
                fieldSet.Fields.Add("type", new SelectListField<string>
                {
                    Name = string.Format("field_{0}_type", field.FormFieldId),
                    Label = ElementResource.FormFieldTypeLabel,
                    Value = field.FieldType.ToString(),
                    Items = new List<ListFieldItem<string>> {
                        new ListFieldItem<string> { Name = ElementResource.FormFieldTypeTextLabel, Value = FormElementFieldType.TextField.ToString() },
                        new ListFieldItem<string> { Name = ElementResource.FormFieldTypeMultiLineLabel, Value = FormElementFieldType.MultiLineTextField.ToString() },
                    }
                });
                fieldSet.Fields.Add("required", new BooleanField
                {
                    Name = string.Format("field_{0}_required", field.FormFieldId),
                    Label = ElementResource.FormFieldRequiredLabel,
                    Value = field.Required
                });
                form.FieldSets.Add(fieldSet);
            }

            // Fields set containing default fields for a new form field
            form.NamedFieldSets = new Dictionary<string, FormFieldSet>();
            FormFieldSet namedFieldSet = new FormFieldSet { Fields = new Dictionary<string, IFormField>() };
            long formFieldId = 0;
            namedFieldSet.Fields.Add("label", new TextField
            {
                Name = string.Format("field_{0}_label", formFieldId),
                Label = ElementResource.FormFieldLabelLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.FormFieldLabelRequiredMessage,
                Value = ElementResource.FormFieldLabelDefaultValue
            });
            namedFieldSet.Fields.Add("type", new SelectListField<string>
            {
                Name = string.Format("field_{0}_type", formFieldId),
                Label = ElementResource.FormFieldTypeLabel,
                Value = FormElementFieldType.TextField.ToString(),
                Items = new List<ListFieldItem<string>> {
                    new ListFieldItem<string> { Name = ElementResource.FormFieldTypeTextLabel, Value = FormElementFieldType.TextField.ToString() },
                    new ListFieldItem<string> { Name = ElementResource.FormFieldTypeMultiLineLabel, Value = FormElementFieldType.MultiLineTextField.ToString() },
                }
            });
            namedFieldSet.Fields.Add("required", new BooleanField
            {
                Name = string.Format("field_{0}_required", formFieldId),
                Label = ElementResource.FormFieldRequiredLabel,
                Value = false
            });
            form.NamedFieldSets.Add("newField", namedFieldSet);

            // Return result
            return form;
        }

        public Form GetForm(string context)
        {
            // The form that we will return
            Form form = null;

            // Get action from context
            string action = context.Split('|')[0];

            // Get the correct form based on action
            switch (action)
            {
                case "":
                    form = GetUserForm(context);
                    break;

                case "admin":
                    form = GetAdminForm(context);
                    break;
            }

            // Return result
            return form;
        }

        private IList<FormElementFieldValue> GetFieldValues(Form form)
        {
            List<FormElementFieldValue> fieldValues = new List<FormElementFieldValue>();
            foreach (FormFieldSet fieldSet in form.FieldSets)
            {
                FormField<string> field = (FormField<string>)fieldSet.Fields["field"];
                long formFieldId = Convert.ToInt64(field.Name.Split('_')[1]);
                fieldValues.Add(new FormElementFieldValue { FormFieldId = formFieldId, Label = field.Label, Value = field.Value });
            }
            return fieldValues;
        }

        private FormResult PostUserForm(Form form)
        {
            // Get page and element identifiers
            string[] parts = form.Context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);

            // Get website identifier
            long tenantId = _authenticationService.TenantId;

            // Send form field values to recipients
            IList<FormElementFieldValue> fieldValues = GetFieldValues(form);
            FormService formService = (FormService)_elementFactory.GetElementService(FormId);
            formService.Send(tenantId, pageId, elementId, fieldValues);

            // Return form result with no errors
            return _formHelperService.GetFormResult();
        }

        private List<FormElementField> GetFields(long elementId, Form form)
        {
            List<FormElementField> fields = new List<FormElementField>();
            for (int index = 0; index < form.FieldSets.Count; index++)
            {
                FormFieldSet fieldSet = form.FieldSets[index];
                FormElementFieldType fieldType;
                Enum.TryParse<FormElementFieldType>(((SelectListField<string>)fieldSet.Fields["type"]).Value, out fieldType);
                long formFieldId = Math.Max(Convert.ToInt64(((TextField)fieldSet.Fields["label"]).Name.Split('_')[1]), 0);
                fields.Add(new FormElementField
                {
                    ElementId = elementId,
                    Label = ((TextField)fieldSet.Fields["label"]).Value,
                    Required = ((BooleanField)fieldSet.Fields["required"]).Value,
                    SortOrder = index,
                    FieldType = fieldType,
                    FormFieldId = formFieldId
                });
            }
            return fields;
        }

        private FormResult PostAdminForm(Form form)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = form.Context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);

            // Get the tag cloud element service
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

            // Get updated form settings
            FormSettings formSettings = (FormSettings)elementService.New(_authenticationService.TenantId);
            formSettings.ElementId = elementId;
            formSettings.Fields = GetFields(elementId, form);
            formSettings.RecipientEmail = ((MultiLineTextField)form.Fields["recipientEmail"]).Value;
            formSettings.SubmitButtonLabel = ((TextField)form.Fields["submitButtonLabel"]).Value;
            formSettings.SubmittedMessage = ((TextField)form.Fields["submittedMessage"]).Value;

            // Perform the update
            elementService.Update(formSettings);

            // Return form result with no errors
            return _formHelperService.GetFormResult();
        }

        public FormResult PostForm(Form form)
        {
            try
            {
                // The form result
                FormResult formResult = null;

                // Split context into different parts
                string action = form.Context.Split('|')[0];

                // Perform the correct action based on form context
                switch (action)
                {
                    case "":
                        formResult = PostUserForm(form);
                        break;

                    case "admin":
                        formResult = PostAdminForm(form);
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
    }
}
