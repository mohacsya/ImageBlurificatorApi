using System.Buffers.Text;
using System.Drawing;
using ImageBlurificatorApi.Models;

namespace ImageBlurificatorApi.Utilities
{
    public static class ImageHelper
    {
        public static Bitmap ConvertBase64ToBitmap(string base64String)
        {
            int commaIndex = base64String.IndexOf(',');
            if (commaIndex >= 0)
                base64String = base64String[(commaIndex + 1)..];

            byte[] bytes = Convert.FromBase64String(base64String);
            using var ms = new MemoryStream(bytes);
            return new Bitmap(ms);
        }
        public static byte[] GetRawImageBytes(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            byte[] imageBytes;
            try
            {
                int bytes = bitmap.Width * bitmap.Height * 3;
                imageBytes = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, imageBytes, 0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }
            return imageBytes;
        }

        public static Bitmap ConvertRawBytesToBitmap(byte[] rawBytes, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(0, 0, width, height);
            System.Drawing.Imaging.BitmapData imageData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            try
            {
                System.Runtime.InteropServices.Marshal.Copy(rawBytes, 0, imageData.Scan0, rawBytes.Length);
            }
            finally
            {
                bitmap.UnlockBits(imageData);
            }
            return bitmap;
        }

        public static byte[] ConvertBitmapToBytes(Bitmap bitmap, EncodingType encodingType)
        {
            var outputEncoding = encodingType == EncodingType.PNG ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Jpeg;
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, outputEncoding);
                return ms.ToArray();
            }
        }
    }
}
