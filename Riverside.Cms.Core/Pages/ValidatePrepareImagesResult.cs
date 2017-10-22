using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.MasterPages;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Holds interesting information retrieved during validate prepare images call. By returning this information from the validator, we don't have
    /// to make additional calls from our service to get the information again.
    /// </summary>
    public class ValidatePrepareImagesResult
    {
        /// <summary>
        /// Master page details.
        /// </summary>
        public MasterPage MasterPage { get; set; }

        /// <summary>
        /// Dimensions of image that user uploaded.
        /// </summary>
        public Size Size { get; set; }
    }
}
