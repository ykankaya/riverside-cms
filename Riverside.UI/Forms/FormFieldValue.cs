using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Contains a form field key, which uniquely identifies a form field, and the value of that form field.
    /// </summary>
    public class FormFieldValue
    {
        /// <summary>
        /// Key that uniquely identifies a field within a form.
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        /// <summary>
        /// The value of a form field.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public object Value { get; set; }
    }
}
