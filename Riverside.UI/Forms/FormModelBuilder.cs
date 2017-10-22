using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Provides model binding for form objects.
    /// </summary>
    public class FormModelBinder : IModelBinder
    {
        /// <summary>
        /// Retrieves strongly typed custom error messages from dynamic object created from JSON.
        /// </summary>
        /// <param name="dynamicCustomErrorMessages">Dynamic custom error messages.</param>
        /// <returns>List of custom error message strings.</returns>
        private List<string> GetCustomErrorMessages(dynamic dynamicCustomErrorMessages)
        {
            List<string> customErrorMessages = new List<string>();
            if (dynamicCustomErrorMessages != null)
            {
                for (int index = 0; index < dynamicCustomErrorMessages.Count; index++)
                    customErrorMessages.Add((string)dynamicCustomErrorMessages[index]);
            }
            return customErrorMessages;
        }

        /// <summary>
        /// Retrieves strongly typed list of string based list field items from dynamic object created from JSON.
        /// </summary>
        /// <param name="dynamicItems">Dynamic list of list field items.</param>
        /// <returns>List of string based list field items.</returns>
        private List<ListFieldItem<string>> GetListItems(dynamic dynamicItems)
        {
            List<ListFieldItem<string>> items = new List<ListFieldItem<string>>();
            return items;
        }

        /// <summary>
        /// Retrieve stringly typed form fields, keyed by form field name, from dynamic fields. TODO: Populate fields via interfaces and re-use code?
        /// </summary>
        /// <param name="dynamicFields">Dynamic representation of form fields from posted JSON.</param>
        /// <returns>Form fields.</returns>
        private IDictionary<string, IFormField> GetFields(dynamic dynamicFields)
        {
            IDictionary<string, IFormField> fields = new Dictionary<string, IFormField>();
            if (dynamicFields != null)
            {
                foreach (dynamic dynamicField in dynamicFields)
                {
                    string name = dynamicField.Name;
                    dynamic value = dynamicField.Value;
                    FormFieldType formFieldType = (FormFieldType)(int)value.fieldType;
                    switch (formFieldType)
                    {
                        case FormFieldType.Checkbox:
                            fields.Add(name, new BooleanField
                            {
                                CustomErrorMessages = GetCustomErrorMessages(value.customErrorMessages),
                                Name = value.name,
                                Label = value.label,
                                Value = value.value
                            });
                            break;

                        case FormFieldType.PasswordTextField:
                            fields.Add(name, new PasswordTextField
                            {
                                CustomErrorMessages = GetCustomErrorMessages(value.customErrorMessages),
                                Label = value.label,
                                MaxLength = value.maxLength,
                                MaxLengthErrorMessage = value.maxLengthErrorMessage,
                                MinLength = value.minLength,
                                MinLengthErrorMessage = value.minLengthErrorMessage,
                                Name = value.name,
                                Pattern = value.pattern,
                                PatternErrorMessage = value.patternErrorMessage,
                                Required = value.required,
                                RequiredErrorMessage = value.requiredErrorMessage,
                                Value = value.value
                            });
                            break;

                        case FormFieldType.TextField:
                            fields.Add(name, new TextField
                            {
                                CustomErrorMessages = GetCustomErrorMessages(value.customErrorMessages),
                                Label = value.label,
                                MaxLength = value.maxLength,
                                MaxLengthErrorMessage = value.maxLengthErrorMessage,
                                MinLength = value.minLength,
                                MinLengthErrorMessage = value.minLengthErrorMessage,
                                Name = value.name,
                                Pattern = value.pattern,
                                PatternErrorMessage = value.patternErrorMessage,
                                Required = value.required,
                                RequiredErrorMessage = value.requiredErrorMessage,
                                Value = value.value
                            });
                            break;

                        case FormFieldType.UploadField:
                            fields.Add(name, new UploadField
                            {
                                CustomErrorMessages = GetCustomErrorMessages(value.customErrorMessages),
                                Label = value.label,
                                Name = value.name,
                                Value = value.value
                            });
                            break;

                        case FormFieldType.IntegerField:
                            fields.Add(name, new IntegerField
                            {
                                CustomErrorMessages = GetCustomErrorMessages(value.customErrorMessages),
                                Label = value.label,
                                Max = value.max,
                                MaxErrorMessage = value.maxErrorMessage,
                                Min = value.min,
                                MinErrorMessage = value.minErrorMessage,
                                Name = value.name,
                                Required = value.required,
                                RequiredErrorMessage = value.requiredErrorMessage,
                                Value = value.value
                            });
                            break;

                        case FormFieldType.MultiLineTextField:
                            fields.Add(name, new MultiLineTextField
                            {
                                CustomErrorMessages = GetCustomErrorMessages(value.customErrorMessages),
                                Label = value.label,
                                MaxLength = value.maxLength,
                                MaxLengthErrorMessage = value.maxLengthErrorMessage,
                                MinLength = value.minLength,
                                MinLengthErrorMessage = value.minLengthErrorMessage,
                                Name = value.name,
                                Pattern = value.pattern,
                                PatternErrorMessage = value.patternErrorMessage,
                                Required = value.required,
                                RequiredErrorMessage = value.requiredErrorMessage,
                                Rows = value.rows,
                                Value = value.value
                            });
                            break;

                        case FormFieldType.SelectListField:
                            fields.Add(name, new SelectListField<string>
                            {
                                CustomErrorMessages = GetCustomErrorMessages(value.customErrorMessages),
                                Items = GetListItems(value.items),
                                Label = value.label,
                                Name = value.name,
                                Required = value.required,
                                RequiredErrorMessage = value.requiredErrorMessage,
                                Value = value.value,
                            });
                            break;
                    }
                }
            }
            return fields;
        }

        /// <summary>
        /// Retrieves list of strongly type field sets from dynamic object created from JSON.
        /// </summary>
        /// <param name="dynamicFieldSets">Dynamic field sets.</param>
        /// <returns>List of field sets.</returns>
        private IList<FormFieldSet> GetFieldSets(dynamic dynamicFieldSets)
        {
            List<FormFieldSet> fieldSets = new List<FormFieldSet>();
            if (dynamicFieldSets != null)
            {
                for (int index = 0; index < dynamicFieldSets.Count; index++)
                {
                    FormFieldSet fieldSet = new FormFieldSet
                    {
                        Fields = GetFields(dynamicFieldSets[index].fields),
                        FieldSets = GetFieldSets(dynamicFieldSets[index].fieldSets)
                    };
                    fieldSets.Add(fieldSet);
                }
            }
            return fieldSets;
        }

        /// <summary>
        /// Retrieves list of strongly typed sub forms from dynamic object created from JSON.
        /// </summary>
        /// <param name="dynamicSubForms">Dynamic sub forms.</param>
        /// <returns>Dictionary of sub forms.</returns>
        private IDictionary<string, Form> GetSubForms(dynamic dynamicSubForms)
        {
            IDictionary<string, Form> subForms = new Dictionary<string, Form>();
            if (dynamicSubForms != null)
            {
                foreach (dynamic dynamicSubForm in dynamicSubForms)
                {
                    string name = dynamicSubForm.Name;
                    dynamic value = dynamicSubForm.Value;
                    Form form = GetForm(value);
                    subForms.Add(name, form);
                }
            }
            return subForms;
        }

        /// <summary>
        /// Get form from dynamic content.
        /// </summary>
        /// <param name="dynamicForm">Dynamic form object.</param>
        /// <returns>Populate form object.</returns>
        private Form GetForm(dynamic dynamicForm)
        {
            return new Form
            {
                Id = dynamicForm.id,
                Context = dynamicForm.context,
                Data = dynamicForm.data,
                Fields = GetFields(dynamicForm.fields),
                SubmitLabel = dynamicForm.submitLabel,
                CustomErrorMessages = GetCustomErrorMessages(dynamicForm.customErrorMessages),
                FieldSets = GetFieldSets(dynamicForm.fieldSets),
                SubForms = GetSubForms(dynamicForm.subForms)
            };
        }

        /// <summary>
        /// Useful article: http://www.c-sharpcorner.com/article/custom-model-binding-in-asp-net-core-mvc/
        /// </summary>
        /// <param name="bindingContext">Model binding context.</param>
        /// <returns>Task.</returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException("Binding context must not be null");

            if (bindingContext.ModelType != typeof(Form))
                throw new Exception("Model type is not form");

            string content = null;
            using (var sr = new StreamReader(bindingContext.HttpContext.Request.Body))
                content = sr.ReadToEnd();
            dynamic dynamicForm = JObject.Parse(content);
            Form form = GetForm(dynamicForm);

            bindingContext.Result = ModelBindingResult.Success(form);

            return Task.CompletedTask;
        }
    }
}
