using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Holds image upload details.
    /// </summary>
    public class Image : Upload
    {
        /// <summary>
        /// Width of image (pixels).
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of image (pixels).
        /// </summary>
        public int Height { get; set; }
    }
}
