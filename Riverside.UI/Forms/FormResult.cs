using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Results that are sent back after a form post.
    /// </summary>
    public class FormResult
    {
        /// <summary>
        /// Status text can be populated with any data that should be passed back to the client form after submission.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// List of errors that occurred during form submission. If form submission successful, this list of errors should be empty.
        /// </summary>
        [JsonProperty(PropertyName = "errors")]
        public IList<FormError> Errors { get; set; }
    }
}
