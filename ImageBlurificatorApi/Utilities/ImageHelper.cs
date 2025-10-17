using System.Drawing;

namespace ImageBlurificatorApi.Utilities
{
    public static class ImageHelper
    {
        public static Bitmap ConvertBase64ToBitmap(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes))
            {
                return new Bitmap(ms);
            }
        }
        public static byte[] GetRawImageBytes(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            byte[] imageBytes;
            try
            {
                int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
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
    }
}
