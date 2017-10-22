using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riverside.UI.Forms;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// View model for master page.
    /// </summary>
    public class MasterPageViewModel : IForm
    {
        /// <summary>
        /// Form wide custom error or validation messages.
        /// </summary>
        [JsonProperty(PropertyName = "customErrorMessages")]
        public IList<string> CustomErrorMessages { get; set; }
    }
}
