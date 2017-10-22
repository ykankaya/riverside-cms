using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Contains information about an item in a list field.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying field value.</typeparam>
    public class ListFieldItem<TValue>
    {
        /// <summary>
        /// List item name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// List item value.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public TValue Value { get; set; }
    }
}
