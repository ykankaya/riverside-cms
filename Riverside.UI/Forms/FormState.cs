using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Encapsulates the state of a form. i.e. form validation failures.
    /// </summary>
    public class FormState
    {
        /// <summary>
        /// Holds form validation errors, keyed by field name.
        /// </summary>
        [JsonProperty(PropertyName = "errors")]
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
