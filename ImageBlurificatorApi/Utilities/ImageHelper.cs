using ImageBlurificatorApi.Models.Internal;
using System.Drawing;
using System.Drawing.Imaging;

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
            using (var ms = new MemoryStream(bytes))
            using (var img = Image.FromStream(ms, useEmbeddedColorManagement: true, validateImageData: true))
            {
                // Create a decoupled bitmap so ms can be disposed
                return new Bitmap(img);
            }
        }
        public static byte[] ConvertBitmapToRgbBytes(Bitmap bitmap, ImageEncodingInfo encodingInfo)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, encodingInfo.PixelFormat);
            byte[] imageBytes;
            try
            {
                int bytes = bitmap.Width * bitmap.Height * encodingInfo.Channels;
                imageBytes = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, imageBytes, 0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }
            return imageBytes;
        }

        public static Bitmap ConvertRawBytesToBitmap(byte[] rawBytes, int width, int height, ImageEncodingInfo encodingInfo)
        {

            
            Bitmap bitmap = new Bitmap(width, height, encodingInfo.PixelFormat);
            Rectangle rect = new Rectangle(0, 0, width, height);
            System.Drawing.Imaging.BitmapData imageData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, encodingInfo.PixelFormat);
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

        public static ImageEncodingInfo GetEncodingInfo(EncodingType encodingType)
        {   
            if(!EncodingInfos.TryGetValue(encodingType, out ImageEncodingInfo? encodingInfo))
            {
                throw new ArgumentException($"Unsupported encoding type: {encodingType}", nameof(encodingType));
            }
            return encodingInfo;
        }

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
