using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Forms
{
    public interface IFormField
    {
        [JsonProperty(PropertyName = "label")]
        string Label { get; set; }

        [JsonProperty(PropertyName = "customErrorMessages")]
        List<string> CustomErrorMessages { get; set; }
    }

    public interface IFormField<TValue> : IFormField
    {
        [JsonProperty(PropertyName = "value")]
        TValue Value { get; set; }
    }
}
