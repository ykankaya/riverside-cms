using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Data that is sent back in the zone element type form. Contains submit button labels for create and update actions, as well as the default values
    /// for master page zone element type when creating a new master page zone element type.
    /// </summary>
    public class MasterPageZoneElementTypeFormData
    {
        /// <summary>
        /// Keys for labels are "create" and "update". Corresponding values are the text that appear on action buttons.
        /// </summary>
        [JsonProperty(PropertyName = "labels")]
        public Dictionary<string, string> Labels { get; set; }

        /// <summary>
        /// Master page zone element type that can be used to populate the initial create master page zone element type form.
        /// </summary>
        [JsonProperty(PropertyName = "masterPageZoneElementType")]
        public MasterPageZoneElementType MasterPageZoneElementType { get; set; }
    }
}
