using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Interface implemented by those fields that can be validated using a regular expression.
    /// </summary>
    public interface IPatternField
    {
        /// <summary>
        /// Regular expression validation for field.
        /// </summary>
        [JsonProperty(PropertyName = "pattern")]
        string Pattern { get; set; }

        /// <summary>
        /// Error message displayed when field regular expression validation fails.
        /// </summary>
        [JsonProperty(PropertyName = "patternErrorMessage")]
        string PatternErrorMessage { get; set; }
    }
}
