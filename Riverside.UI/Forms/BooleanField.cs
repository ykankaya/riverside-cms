using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Form field for displaying Boolean checkbox.
    /// </summary>
    public class BooleanField : FormField<bool>
    {
        /// <summary>
        /// Indicates the type of form field.
        /// </summary>
        [JsonProperty(PropertyName = "fieldType")]
        public override FormFieldType FormFieldType { get { return FormFieldType.Checkbox; } }
    }
}
