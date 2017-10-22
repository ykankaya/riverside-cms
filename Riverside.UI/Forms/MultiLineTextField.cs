using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Multi-line text field.
    /// </summary>
    public class MultiLineTextField : TextField
    {
        /// <summary>
        /// Row count.
        /// </summary>
        [JsonProperty(PropertyName = "rows")]
        public int Rows { get; set; }

        /// <summary>
        /// Indicates the type of form field.
        /// </summary>
        [JsonProperty(PropertyName = "fieldType")]
        public override FormFieldType FormFieldType { get { return FormFieldType.MultiLineTextField; } }
    }
}
