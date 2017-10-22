using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Integer field details.
    /// </summary>
    public class IntegerField : FormField<int?>, IRequiredField, IRangeField
    {
        /// <summary>
        /// Indicates whether form field is mandatory.
        /// </summary>
        [JsonProperty(PropertyName = "required")]
        public bool Required { get; set; }

        /// <summary>
        /// Error message to display when mandatory field is empty.
        /// </summary>
        [JsonProperty(PropertyName = "requiredErrorMessage")]
        public string RequiredErrorMessage { get; set; }

        /// <summary>
        /// Min field value.
        /// </summary>
        [JsonProperty(PropertyName = "min")]
        public int? Min { get; set; }

        /// <summary>
        /// Max field value.
        /// </summary>
        [JsonProperty(PropertyName = "max")]
        public int? Max { get; set; }

        /// <summary>
        /// Error message displayed when numeric value less than minimum specified.
        /// </summary>
        [JsonProperty(PropertyName = "minErrorMessage")]
        public string MinErrorMessage { get; set; }

        /// <summary>
        /// Error message displayed when numeric value greater than maximum specified.
        /// </summary>
        [JsonProperty(PropertyName = "maxErrorMessage")]
        public string MaxErrorMessage { get; set; }

        /// <summary>
        /// Indicates the type of form field.
        /// </summary>
        [JsonProperty(PropertyName = "fieldType")]
        public override FormFieldType FormFieldType { get { return FormFieldType.IntegerField; } }
    }
}
