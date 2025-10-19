using System.Drawing.Imaging;

namespace ImageBlurificatorApi.Models.Internal
{
    /// <summary>
    /// Holds information about the image encoding format.
    /// </summary>
    public class ImageEncodingInfo
    {
        public PixelFormat PixelFormat { get; }
        public int Channels { get; }
        public string MimeType { get; }

        public ImageFormat ImageFormat{ get;}
        public ImageEncodingInfo(PixelFormat pixelFormat, int channels, string mimeType, ImageFormat imageFormat )
        {
            PixelFormat = pixelFormat;
            Channels = channels;
            MimeType = mimeType;
            ImageFormat = imageFormat;
        }
    }
    /// <summary>
    /// Specifies the encoding format for the output image.
    /// </summary>
    public enum EncodingType
    {
        PNG,
        JPG
    }
   
}
