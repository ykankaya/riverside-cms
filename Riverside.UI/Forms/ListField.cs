using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Form field containing list of items.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying field value.</typeparam>
    public abstract class ListField<TValue> : FormField<TValue>, IRequiredField
    {
        /// <summary>
        /// Field list items.
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public List<ListFieldItem<TValue>> Items { get; set; }

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
    }
}
