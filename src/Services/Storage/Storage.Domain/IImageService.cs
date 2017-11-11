using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Riverside.Cms.Services.Storage.Domain
{
    public interface IImageService
    {
        ImageMetadata GetImageMetadata(Stream stream);
    }
}
