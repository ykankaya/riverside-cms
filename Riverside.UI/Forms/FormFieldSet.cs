using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    public class FormFieldSet
    {
        /// <summary>
        /// The fields that make up this form, keyed by form field name.
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public IDictionary<string, IFormField> Fields { get; set; }

        /// <summary>
        /// Child form field sets.
        /// </summary>
        [JsonProperty(PropertyName = "fieldSets")]
        public IList<FormFieldSet> FieldSets { get; set; }

        /// <summary>
        /// Field sets, keyed by name.
        /// </summary>
        [JsonProperty(PropertyName = "namedFieldSets")]
        public IDictionary<string, FormFieldSet> NamedFieldSets { get; set; }
    }
}
