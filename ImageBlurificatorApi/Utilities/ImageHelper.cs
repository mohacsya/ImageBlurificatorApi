using ImageBlurificatorApi.Models.Internal;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageBlurificatorApi.Utilities
{
    public static class ImageHelper
    {
        /// <summary>
        /// Holds the mapping of EncodingType to ImageEncodingInfo. Add more encodings as needed.
        /// </summary>
        private static readonly Dictionary<EncodingType, ImageEncodingInfo> EncodingInfos = new()
        {
            { EncodingType.PNG, new ImageEncodingInfo(PixelFormat.Format32bppArgb, 4, "image/png", ImageFormat.Png) },
            { EncodingType.JPG, new ImageEncodingInfo(PixelFormat.Format24bppRgb, 3, "image/jpeg", ImageFormat.Jpeg) }
        };

        /// <summary>
        /// Converts a base64 encoded string to a Bitmap.
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static Bitmap ConvertBase64ToBitmap(string base64String)
        {
            int commaIndex = base64String.IndexOf(',');
            if (commaIndex >= 0)
                base64String = base64String[(commaIndex + 1)..]; // Remove data URI prefix if present

            byte[] bytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(bytes))
            using (var img = Image.FromStream(ms, useEmbeddedColorManagement: true, validateImageData: true))
            {
                // Create a decoupled bitmap so ms can be disposed safely
                return new Bitmap(img);
            }
        }
        /// <summary>
        /// Converts the provided Bitmap to a raw RGB byte array using the specified encoding info.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="encodingInfo"></param>
        /// <returns></returns>
        public static byte[] ConvertBitmapToRgbBytes(Bitmap bitmap, ImageEncodingInfo encodingInfo)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            // Lock the bitmap's bits for direct memory access in the specified pixel format
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, encodingInfo.PixelFormat);
            byte[] imageBytes;
            try
            {
                int bytes = bitmap.Width * bitmap.Height * encodingInfo.Channels;
                imageBytes = new byte[bytes];
                // Copy pixel data from unmanaged memory to managed array
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, imageBytes, 0, bytes);
                return imageBytes;
            }
            finally
            {
                // Always unlock the bits, even if an exception occurs
                bitmap.UnlockBits(bmpData);
            }
        }

        /// <summary>
        /// Converts raw RGB byte array to a Bitmap using the provided encoding info.
        /// </summary>
        /// <param name="rgbBytes"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="encodingInfo"></param>
        /// <returns></returns>
        public static Bitmap ConvertRgbBytesToBitmap(byte[] rgbBytes, int width, int height, ImageEncodingInfo encodingInfo)
        {
            Bitmap bitmap = new Bitmap(width, height, encodingInfo.PixelFormat);
            Rectangle rect = new Rectangle(0, 0, width, height);

            // Lock the bitmap's bits for writing pixel data directly
            System.Drawing.Imaging.BitmapData imageData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, encodingInfo.PixelFormat);
            bool success = false;
            try
            {
                // Copy raw RGB bytes into the bitmap's memory
                System.Runtime.InteropServices.Marshal.Copy(rgbBytes, 0, imageData.Scan0, rgbBytes.Length);
                success = true;
                return bitmap;
            }
            finally
            {
                bitmap.UnlockBits(imageData);
                if(!success)
                {
                    // Disposing the bitmap manually in case of errors.
                    bitmap.Dispose();
                }
            }
        }

        /// <summary>
        /// Get the ImageEncodingInfo for the specified EncodingType.
        /// </summary>
        /// <param name="encodingType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ImageEncodingInfo GetEncodingInfo(EncodingType encodingType)
        {   
            if(!EncodingInfos.TryGetValue(encodingType, out ImageEncodingInfo? encodingInfo))
            {
                // Throw if the encoding type is not supported
                throw new ArgumentException($"Unsupported encoding type: {encodingType}", nameof(encodingType));
            }
            return encodingInfo;
        }

        /// <summary>
        /// Converts the provided Bitmap to a byte array in the specified encoding format. Usable for saving or transferring the image as encoded byte array.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="encodingInfo"></param>
        /// <returns></returns>
        public static byte[] ConvertBitmapToBytes(Bitmap bitmap, ImageEncodingInfo encodingInfo)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, encodingInfo.ImageFormat);
                return ms.ToArray();
            }
        }
    }
}
