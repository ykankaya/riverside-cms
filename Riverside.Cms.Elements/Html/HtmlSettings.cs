using Riverside.Cms.Core.Elements;
using Riverside.Utilities.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Html
{
    public class HtmlSettings : ElementSettings
    {
        /// <summary>
        /// HTML to be rendered.
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Width of thumbnail images. TODO: Implement this.
        /// </summary>
        public int ThumbnailImageWidth { get { return 150; } }

        /// <summary>
        /// Height of thumbnail images. TODO: Implement this.
        /// </summary>
        public int ThumbnailImageHeight { get { return 150; } }

        /// <summary>
        /// Determines how thumbnail images generated. TODO: Implement this.
        /// </summary>
        public ResizeMode ThumbnailImageResizeMode { get { return ResizeMode.MaintainAspect; } }

        /// <summary>
        /// Width of preview images. TODO: Implement this.
        /// </summary>
        public int PreviewImageWidth { get { return 1200; } }

        /// <summary>
        /// Height of preview image. TODO: Implement this.
        /// </summary>
        public int PreviewImageHeight { get { return 32000; } }

        /// <summary>
        /// Determines how preview images generated. TODO: Implement this.
        /// </summary>
        public ResizeMode PreviewImageResizeMode { get { return ResizeMode.MaintainAspect; } }

        /// <summary>
        /// The HTML uploads associated with this element.
        /// </summary>
        public List<HtmlUpload> Uploads { get; set; }
    }
}
