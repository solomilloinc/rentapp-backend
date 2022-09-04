using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NG.Web.Helpers
{
    public static class ImageHelper
    {
        #region Resize and Compression

        public static MemoryStream CompressAndResizeImage(MemoryStream data, int targetMaxSize)
        {
            Image originalImage = null, resizedImage = null;
            try
            {
                originalImage = Image.FromStream(data);
                var maxImageSize = new int[] { originalImage.Width, originalImage.Height }.Max();
                targetMaxSize = targetMaxSize <= maxImageSize ? targetMaxSize : maxImageSize;

                Size targetSize = GetTargetImageSize(originalImage, targetMaxSize, false);

                //resize the image to the specified height and width
                resizedImage = ResizeImage(originalImage, targetSize.Width, targetSize.Height);

                originalImage.Dispose();

                //save the resized image as a jpeg with a quality of 72
                return SaveJpeg(resizedImage, 72);
            }
            finally
            {
                originalImage?.Dispose();
                resizedImage?.Dispose();
            }
        }

        private static Lazy<Dictionary<string, ImageCodecInfo>> encoders = new Lazy<Dictionary<string, ImageCodecInfo>>(() =>
            ImageCodecInfo.GetImageEncoders().ToDictionary(x => x.MimeType.ToLower(), x => x));

        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            get { return encoders.Value; }
        }

        public static MemoryStream RotateImage(MemoryStream data, RotateFlipType rotateFlipType = RotateFlipType.Rotate90FlipNone)
        {
            Image image = null;
            try
            {
                image = Image.FromStream(data);

                image.RotateFlip(rotateFlipType);

                return SaveJpeg(image, 72);
            }
            finally
            {
                image?.Dispose();
            }
        }



        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        /// <summary>
        /// Saves an image as a jpeg image, with the given quality
        /// </summary>
        /// <param name="image"></param>
        /// <param name="quality">An integer from 0 to 100, with 100 being the
        /// highest quality</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// An invalid value was entered for image quality.
        /// </exception>
        public static MemoryStream SaveJpeg(Image image, int quality)
        {
            //ensure the quality is within the correct range
            if ((quality < 0) || (quality > 100))
            {
                //create the error message
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality. A value of {0} was specified.", quality);
                //throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }

            //create an encoder parameter for the image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //get the jpeg codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            //create a collection of all parameters that we will pass to the encoder
            EncoderParameters encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            MemoryStream stream = new MemoryStream();
            image.Save(stream, jpegCodec, encoderParams);

            return stream;
        }

        /// <summary>
        /// Returns the image codec with the given mime type
        /// </summary>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();

            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }

        #endregion

        #region Thumbnail

        public static MemoryStream CreateImagePreview(MemoryStream data, int minPixels = 200)
        {
            // Load image.
            using (Image image = Image.FromStream(data))
            {

                // Compute thumbnail size.
                Size thumbnailSize = GetTargetImageSize(image, minPixels);

                // Get thumbnail.
                using (Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero))
                {
                    return SaveJpeg(thumbnail, 72);
                }
            }
        }

        #endregion

        public static Size GetTargetImageSize(Image original, int maxPixels, bool scaleOnLargerDimension = true)
        {
            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            return GetTargetImageSize(originalWidth, originalHeight, maxPixels, scaleOnLargerDimension);
        }

        public static Size GetTargetImageSize(int originalWidth, int originalHeight, int maxPixels, bool scaleOnLargerDimension = true)
        {
            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (scaleOnLargerDimension)
            {
                if (originalWidth < originalHeight)
                {
                    factor = (double)maxPixels / originalWidth;
                }
                else
                {
                    factor = (double)maxPixels / originalHeight;
                }
            }
            else
            {
                if (originalWidth > originalHeight)
                {
                    factor = (double)maxPixels / originalWidth;
                }
                else
                {
                    factor = (double)maxPixels / originalHeight;
                }
            }
            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }

        public static Size GetImageSize(MemoryStream data)
        {
            Size size = new Size();
            using (Bitmap bitmap = new Bitmap(data))
            {
                size = bitmap.Size;
            }
            return size;
        }

        public static MemoryStream SaveImage(Bitmap bitmap, bool isPng, long compression)
        {
            var qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
            var quality = compression;
            var ratio = new EncoderParameter(qualityEncoder, quality);
            var codecParams = new EncoderParameters(1);
            codecParams.Param[0] = ratio;
            var codecInfo = ImageCodecInfo.GetImageEncoders().Single(o => o.FormatDescription == (!isPng ? "JPEG" : "PNG"));

            var stream = new MemoryStream();
            bitmap.Save(stream, codecInfo, codecParams); // Save to image file

            return stream;
        }

    }
}
