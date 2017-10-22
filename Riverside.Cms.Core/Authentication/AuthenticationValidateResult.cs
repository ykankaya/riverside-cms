using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Webs;

namespace Riverside.Cms.Core.Authentication
{
    /// <summary>
    /// Holds interesting information retrieved during validate prepare images call. By returning this information from the validator, we don't have
    /// to make additional calls from our service to get the information again.
    /// </summary>
    public class AuthenticationValidateResult
    {
        /// <summary>
        /// Website details.
        /// </summary>
        public Web Web { get; set; }

        /// <summary>
        /// Dimensions of image that user uploaded.
        /// </summary>
        public Size Size { get; set; }
    }
}
