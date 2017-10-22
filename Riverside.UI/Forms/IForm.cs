using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Interface for forms.
    /// </summary>
    public interface IForm
    {
        /// <summary>
        /// Form wide custom error or validation messages.
        /// </summary>
        [JsonProperty(PropertyName = "customErrorMessages")]
        IList<string> CustomErrorMessages { get; set; }
    }
}
