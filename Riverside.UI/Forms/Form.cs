using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Defines a form.
    /// </summary>
    public class Form
    {
        /// <summary>
        /// Form identifier.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Form context.
        /// </summary>
        [JsonProperty(PropertyName = "context")]
        public string Context { get; set; }

        /// <summary>
        /// Can be used to store additional data that may be used by the HTML and JavaScript views that depend on this form.
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        /// <summary>
        /// The fields that make up this form, keyed by form field name.
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public IDictionary<string, IFormField> Fields { get; set; }

        /// <summary>
        /// Sub forms that may be displayed within a parent form.
        /// </summary>
        [JsonProperty(PropertyName = "subForms")]
        public IDictionary<string, Form> SubForms { get; set; }

        /// <summary>
        /// Field sets.
        /// </summary>
        [JsonProperty(PropertyName = "fieldSets")]
        public IList<FormFieldSet> FieldSets { get; set; }

        /// <summary>
        /// Field sets, keyed by name.
        /// </summary>
        [JsonProperty(PropertyName = "namedFieldSets")]
        public IDictionary<string, FormFieldSet> NamedFieldSets { get; set; }

        /// <summary>
        /// Submit button text.
        /// </summary>
        [JsonProperty(PropertyName = "submitLabel")]
        public string SubmitLabel { get; set; }

        /// <summary>
        /// Form wide custom error or validation messages.
        /// </summary>
        [JsonProperty(PropertyName = "customErrorMessages")]
        public IList<string> CustomErrorMessages { get; set; }
    }
}
