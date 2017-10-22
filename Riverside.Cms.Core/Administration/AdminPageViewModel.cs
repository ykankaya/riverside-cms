using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Riverside.Cms.Core.Administration
{
    public class AdminPageViewModel<TModel>
    {
        [JsonProperty(PropertyName = "model")]
        public TModel Model { get; set; }

        [JsonProperty(PropertyName = "navigation")]
        public NavigationViewModel Navigation { get; set; }
    }
}
