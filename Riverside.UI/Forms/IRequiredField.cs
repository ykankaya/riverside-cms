using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Interface implemented by fields that can have mandatory values.
    /// </summary>
    public interface IRequiredField
    {
        /// <summary>
        /// Indicates whether form field is mandatory.
        /// </summary>
        [JsonProperty(PropertyName = "required")]
        bool Required { get; set; }

        /// <summary>
        /// Error message to display when mandatory field is empty.
        /// </summary>
        [JsonProperty(PropertyName = "requiredErrorMessage")]
        string RequiredErrorMessage { get; set; }
    }
}
