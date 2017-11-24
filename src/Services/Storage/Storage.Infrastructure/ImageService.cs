using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Riverside.Cms.Services.Storage.Domain;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

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

        public Stream ResizeImage(Stream stream, ResizeOptions options)
        {
            IImageFormat format;
            using (Image<Rgba32> image = Image.Load(stream, out format))
            {
                switch (options.Mode)
                {
                    case ResizeMode.Simple:
                        image.Mutate(x => x
                            .Resize(options.Width, options.Height));
                        break;
                }

                MemoryStream ms = new MemoryStream();
                image.Save(ms, format);
                ms.Position = 0;
                return ms;
            }
        }
    }
}
