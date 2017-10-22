using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riverside.UI.Forms;

namespace Riverside.Cms.Core.Webs
{
    /// <summary>
    /// View model used for website CRUD actions.
    /// </summary>
    public class WebViewModel : IForm
    {
        /// <summary>
        /// Text field for input of website name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public TextField Name { get; set; }

        /// <summary>
        /// Text field for input of domain that website will be hosted on.
        /// </summary>
        [JsonProperty(PropertyName = "domain")]
        public TextField Domain { get; set; }

        /// <summary>
        /// Select list field for selection of template.
        /// </summary>
        [JsonProperty(PropertyName = "template")]
        public SelectListField<string> Template { get; set; }

        /// <summary>
        /// Form wide custom error or validation messages.
        /// </summary>
        [JsonProperty(PropertyName = "customErrorMessages")]
        public IList<string> CustomErrorMessages { get; set; }

        /// <summary>
        /// Returns name of website.
        /// </summary>
        /// <returns>Website name.</returns>
        public override string ToString()
        {
            return string.Format("{0}", Name.Value);
        }
    }
}
