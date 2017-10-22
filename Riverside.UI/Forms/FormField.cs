using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Base class for form fields.
    /// </summary>
    /// <typeparam name="TValue">Type of underlying field value.</typeparam>
    public abstract class FormField<TValue> : IFormField<TValue>
    {
        /// <summary>
        /// Field label.
        /// </summary>
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        /// <summary>
        /// Field name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Field value.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public TValue Value { get; set; }

        /// <summary>
        /// Error messages related to this field.
        /// </summary>
        [JsonProperty(PropertyName = "customErrorMessages")]
        public List<string> CustomErrorMessages { get; set; }

        /// <summary>
        /// Indicates the type of form field.
        /// </summary>
        [JsonProperty(PropertyName = "fieldType")]
        public abstract FormFieldType FormFieldType { get; }

        /// <summary>
        /// Returns form field value.
        /// </summary>
        /// <returns>Field value.</returns>
        public override string ToString()
        {
            return string.Format("{0}", Value);
        }
    }
}
