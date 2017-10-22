using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Interface implemented by fields that can have min and max string lengths.
    /// </summary>
    public interface ILengthField
    {
        /// <summary>
        /// When specified, is the maximum number of characters that can be input in this form field.
        /// </summary>
        [JsonProperty(PropertyName = "maxLength")]
        int? MaxLength { get; set; }

        /// <summary>
        /// Error message to display when maximum number of characters exceeded.
        /// </summary>
        [JsonProperty(PropertyName = "maxLengthErrorMessage")]
        string MaxLengthErrorMessage { get; set; }

        /// <summary>
        /// When specified, is the minimum number of characters that can be input in this form field.
        /// </summary>
        [JsonProperty(PropertyName = "minLength")]
        int? MinLength { get; set; }

        /// <summary>
        /// Error message to display when number of characters falls below minimum number.
        /// </summary>
        [JsonProperty(PropertyName = "minLengthErrorMessage")]
        string MinLengthErrorMessage { get; set; }
    }
}
