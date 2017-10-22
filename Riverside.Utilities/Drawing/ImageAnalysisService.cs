using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Riverside.Utilities.Drawing
{
    /// <summary>
    /// Implements image analysis tasks.
    /// </summary>
    public class ImageAnalysisService : IImageAnalysisService
    {
        /// <summary>
        /// Gets image dimensions from the supplier image content. If content cannot be identified as an image, then null is returned.
        /// </summary>
        /// <param name="content">Image content.</param>
        /// <returns>Width and height of image (or null if content is not identified as an image).</returns>
        public Size? GetImageDimensions(byte[] content)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(content))
                {
                    using (Image image = Image.FromStream(ms))
                    {
                        return new Size(image.Width, image.Height);
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// Resizes image using "simple" resizing.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="resizeInfo">Determines method by which source image is resized.</param>
        /// <returns>Resized image.</returns>
        private Image ResizeDrawingImageSimple(Image sourceImage, ResizeInfo resizeInfo)
        {
            Bitmap bitmap = new Bitmap(resizeInfo.Width, resizeInfo.Height, PixelFormat.Format24bppRgb);
            bitmap.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(sourceImage,
                    /* Note: The -1, +2 prevents ringing artifacts on resized image */
                    new Rectangle(-1, -1, resizeInfo.Width + 2, resizeInfo.Height + 2),
                    new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                    GraphicsUnit.Pixel);
            }
            return bitmap;
        }

        /// <summary>
        /// Resizes image using "maintain aspect" resizing.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="resizeInfo">Determines method by which source image is resized.</param>
        /// <returns>Resized image.</returns>
        private Image ResizeDrawingImageMaintainAspect(Image sourceImage, ResizeInfo resizeInfo)
        {
            double aspectHeightRatio = ((double)resizeInfo.Height) / ((double)sourceImage.Height);
            double aspectWidthRatio = ((double)resizeInfo.Width) / ((double)sourceImage.Width);
            double aspectRatio = Math.Min(aspectHeightRatio, aspectWidthRatio);
            int width = (int)(((double)sourceImage.Width) * aspectRatio);
            int height = (int)(((double)sourceImage.Height) * aspectRatio);
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bitmap.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(sourceImage,
                    /* Note: The -1, +2 prevents ringing artifacts on resized image */
                    new Rectangle(-1, -1, width + 2, height + 2),
                    new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                    GraphicsUnit.Pixel);
            }
            return bitmap;
        }

        /// <summary>
        /// Resizes image using "crop" resizing.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="resizeInfo">Determines method by which source image is resized.</param>
        /// <returns>Resized image.</returns>
        private Image ResizeDrawingImageCrop(Image sourceImage, ResizeInfo resizeInfo)
        {
            double cropHeightRatio = ((double)resizeInfo.Height) / ((double)sourceImage.Height);
            double cropWidthRatio = ((double)resizeInfo.Width) / ((double)sourceImage.Width);
            double cropRatio = Math.Max(cropHeightRatio, cropWidthRatio);
            int unCroppedWidth = (int)(((double)sourceImage.Width) * cropRatio);
            int unCroppedHeight = (int)(((double)sourceImage.Height) * cropRatio);
            int left = -(unCroppedWidth - resizeInfo.Width) / 2;
            int top = -(unCroppedHeight - resizeInfo.Height) / 2;
            Bitmap bitmap = new Bitmap(resizeInfo.Width, resizeInfo.Height, PixelFormat.Format24bppRgb);
            bitmap.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(sourceImage,
                    /* Note: The -1, +2 prevents ringing artifacts on resized image */
                    new Rectangle(left - 1, top - 1, unCroppedWidth + 2, unCroppedHeight + 2),
                    new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                    GraphicsUnit.Pixel);
            }
            return bitmap;
        }

        /// <summary>
        /// Resizes source image using specified resize information.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="resizeInfo">Determines method by which source image is resized.</param>
        /// <returns>The destination image.</returns>
        private Image ResizeDrawingImage(Image sourceImage, ResizeInfo resizeInfo)
        {
            switch (resizeInfo.ResizeMode)
            {
                case ResizeMode.MaintainAspect:
                    return ResizeDrawingImageMaintainAspect(sourceImage, resizeInfo);

                case ResizeMode.Crop:
                    return ResizeDrawingImageCrop(sourceImage, resizeInfo);

                default: /* ResizeMode.Simple */
                    return ResizeDrawingImageSimple(sourceImage, resizeInfo);
            }
        }

        /// <summary>
        /// Resizes an image.
        /// </summary>
        /// <param name="sourceContent">The content of the original source image that is resized.</param>
        /// <param name="resizeInfo">Describes how image is resized.</param>
        /// <returns>Resized image contents.</returns>
        public byte[] ResizeImage(byte[] sourceContent, ResizeInfo resizeInfo)
        {
            using (MemoryStream ms = new MemoryStream(sourceContent))
            {
                using (Image image = Image.FromStream(ms))
                {
                    using (Image resizedImage = ResizeDrawingImage(image, resizeInfo))
                    {
                        using (MemoryStream destMemoryStream = new MemoryStream())
                        {
                            resizedImage.Save(destMemoryStream, image.RawFormat);
                            byte[] destContent = new Byte[destMemoryStream.Length];
                            destMemoryStream.Seek(0, SeekOrigin.Begin);
                            destMemoryStream.Read(destContent, 0, (int)destMemoryStream.Length);
                            return destContent;
                        };
                    }
                }
            }
        }
    }
}
