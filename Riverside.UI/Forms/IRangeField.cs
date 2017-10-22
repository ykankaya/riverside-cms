using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Interface implemented by fields that can have min and max values.
    /// </summary>
    public interface IRangeField
    {
        /// <summary>
        /// Min field value.
        /// </summary>
        [JsonProperty(PropertyName = "min")]
        int? Min { get; set; }

        /// <summary>
        /// Max field value.
        /// </summary>
        [JsonProperty(PropertyName = "max")]
        int? Max { get; set; }

        /// <summary>
        /// Error message displayed when numeric value less than minimum specified.
        /// </summary>
        [JsonProperty(PropertyName = "minErrorMessage")]
        string MinErrorMessage { get; set; }

        /// <summary>
        /// Error message displayed when numeric value greater than maximum specified.
        /// </summary>
        [JsonProperty(PropertyName = "maxErrorMessage")]
        string MaxErrorMessage { get; set; }
    }
}
