using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// File upload field.
    /// </summary>
    public class UploadField : FormField<string>
    {
        /// <summary>
        /// Indicates the type of form field.
        /// </summary>
        [JsonProperty(PropertyName = "fieldType")]
        public override FormFieldType FormFieldType { get { return FormFieldType.UploadField; } }
    }
}
