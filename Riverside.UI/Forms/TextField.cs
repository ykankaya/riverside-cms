using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Text field.
    /// </summary>
    public class TextField : FormField<string>, IRequiredField, ILengthField, IPatternField
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
        /// When specified, is the maximum number of characters that can be input in this form field.
        /// </summary>
        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }

        /// <summary>
        /// Error message to display when maximum number of characters exceeded.
        /// </summary>
        [JsonProperty(PropertyName = "maxLengthErrorMessage")]
        public string MaxLengthErrorMessage { get; set; }

        /// <summary>
        /// When specified, is the minimum number of characters that can be input in this form field.
        /// </summary>
        [JsonProperty(PropertyName = "minLength")]
        public int? MinLength { get; set; }

        /// <summary>
        /// Error message to display when number of characters falls below minimum number.
        /// </summary>
        [JsonProperty(PropertyName = "minLengthErrorMessage")]
        public string MinLengthErrorMessage { get; set; }

        /// <summary>
        /// Regular expression validation for field.
        /// </summary>
        [JsonProperty(PropertyName = "pattern")]
        public string Pattern { get; set; }

        /// <summary>
        /// Error message displayed when field regular expression validation fails.
        /// </summary>
        [JsonProperty(PropertyName = "patternErrorMessage")]
        public string PatternErrorMessage { get; set; }

        /// <summary>
        /// Indicates the type of form field.
        /// </summary>
        [JsonProperty(PropertyName = "fieldType")]
        public override FormFieldType FormFieldType { get { return FormFieldType.TextField; } }
    }
}
