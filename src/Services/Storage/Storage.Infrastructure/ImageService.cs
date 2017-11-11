using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Riverside.Cms.Services.Storage.Domain;
using SixLabors.ImageSharp;

namespace Riverside.Cms.Services.Storage.Infrastructure
{
    public class ImageService : IImageService
    {
        public ImageMetadata GetImageMetadata(Stream stream)
        {
            using (Image<Rgba32> image = Image.Load(stream))
            {
                return new ImageMetadata
                {
                    Width = image.Width,
                    Height = image.Height
                };
            }
        }
    }
}
