using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Form field containing list of items.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying field value.</typeparam>
    public class SelectListField<TValue> : ListField<TValue>
    {
        /// <summary>
        /// Indicates the type of form field.
        /// </summary>
        [JsonProperty(PropertyName = "fieldType")]
        public override FormFieldType FormFieldType { get { return FormFieldType.SelectListField; } }
    }
}
