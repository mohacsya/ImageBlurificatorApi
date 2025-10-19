using ImageBlurificatorApi.Models.Internal;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageBlurificatorApi.Utilities
{
    public static class ImageHelper
    {
        private static readonly Dictionary<EncodingType, ImageEncodingInfo> EncodingInfos = new()
        {
            { EncodingType.PNG, new ImageEncodingInfo(PixelFormat.Format32bppArgb, 4, "image/png", ImageFormat.Png) },
            { EncodingType.JPG, new ImageEncodingInfo(PixelFormat.Format24bppRgb, 3, "image/jpeg", ImageFormat.Jpeg) }
        };

        public static Bitmap ConvertBase64ToBitmap(string base64String)
        {
            int commaIndex = base64String.IndexOf(',');
            if (commaIndex >= 0)
                base64String = base64String[(commaIndex + 1)..];

            byte[] bytes = Convert.FromBase64String(base64String);
            using var ms = new MemoryStream(bytes);
            using var img = Image.FromStream(ms, useEmbeddedColorManagement: true, validateImageData: true);
            return new Bitmap(img);
        }

        public static ImageEncodingInfo GetEncodingInfo(EncodingType encodingType)
        {
            if (!EncodingInfos.TryGetValue(encodingType, out var info))
                throw new ArgumentException($"Unsupported encoding type: {encodingType}", nameof(encodingType));
            return info;
        }

        /// <summary>
        /// Produces a tightly packed buffer (no stride padding) in the target pixel format.
        /// System.Drawing memory ordering for 24/32 bpp is BGR/BGRA; we treat ordering as opaque.
        /// </summary>
        public static byte[] ExtractPackedBuffer(Bitmap bitmap, PixelFormat targetFormat,
                                                 out int width, out int height, out int channels)
        {
            width = bitmap.Width;
            height = bitmap.Height;

            Bitmap working = bitmap;
            if (bitmap.PixelFormat != targetFormat)
            {
                working = new Bitmap(width, height, targetFormat);
                using var g = Graphics.FromImage(working);
                g.DrawImage(bitmap, new Rectangle(0, 0, width, height));
            }

            channels = targetFormat switch
            {
                PixelFormat.Format24bppRgb => 3,
                PixelFormat.Format32bppArgb => 4,
                PixelFormat.Format32bppRgb => 4,
                PixelFormat.Format32bppPArgb => 4,
                _ => throw new NotSupportedException($"Unsupported pixel format: {targetFormat}")
            };

            int packedLength = width * height * channels;
            byte[] packed = new byte[packedLength];

            Rectangle rect = new(0, 0, width, height);
            BitmapData data = working.LockBits(rect, ImageLockMode.ReadOnly, targetFormat);
            try
            {
                int srcStride = data.Stride;
                int rowBytes = width * channels;

                if (srcStride == rowBytes)
                {
                    Marshal.Copy(data.Scan0, packed, 0, packedLength);
                }
                else
                {
                    IntPtr rowPtr = data.Scan0;
                    for (int y = 0; y < height; y++)
                    {
                        int destOffset = y * rowBytes;
                        Marshal.Copy(rowPtr, packed, destOffset, rowBytes);
                        rowPtr = IntPtr.Add(rowPtr, srcStride);
                    }
                }
            }
            finally
            {
                working.UnlockBits(data);
                if (!ReferenceEquals(working, bitmap))
                    working.Dispose();
            }

            return packed;
        }

        /// <summary>
        /// Creates a Bitmap from a tightly packed buffer (no stride padding).
        /// </summary>
        public static Bitmap CreateBitmapFromPacked(byte[] buffer, int width, int height, PixelFormat format, int channels)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (buffer.Length != width * height * channels)
                throw new ArgumentException($"Buffer length {buffer.Length} != width*height*channels {width * height * channels}");

            Bitmap bmp = new(width, height, format);
            Rectangle rect = new(0, 0, width, height);
            BitmapData data = bmp.LockBits(rect, ImageLockMode.WriteOnly, format);
            try
            {
                int destStride = data.Stride;
                int rowBytes = width * channels;
                IntPtr destPtr = data.Scan0;

                if (destStride == rowBytes)
                {
                    Marshal.Copy(buffer, 0, destPtr, buffer.Length);
                }
                else
                {
                    for (int y = 0; y < height; y++)
                    {
                        int srcOffset = y * rowBytes;
                        Marshal.Copy(buffer, srcOffset, destPtr, rowBytes);
                        destPtr = IntPtr.Add(destPtr, destStride);
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(data);
            }
            return bmp;
        }

        /// <summary>
        /// Encodes a packed buffer in the specified encoding format.
        /// </summary>
        public static byte[] EncodePackedBuffer(byte[] buffer, int width, int height, ImageEncodingInfo encodingInfo)
        {
            using var bmp = CreateBitmapFromPacked(buffer, width, height, encodingInfo.PixelFormat, encodingInfo.Channels);
            using var ms = new MemoryStream();
            bmp.Save(ms, encodingInfo.ImageFormat);
            return ms.ToArray();
        }
    }
}