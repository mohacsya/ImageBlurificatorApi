using ImageBlurificatorApi.Services.Interfaces;
using GaussianBlurCore;
using ImageBlurificatorApi.Utilities;
using System.Drawing;
using ImageBlurificatorApi.Models.Internal;

namespace ImageBlurificatorApi.Services.Implementations
{
    /// <summary>
    /// Implements image processing using OpenCV for Gaussian blur.
    /// </summary>
    public class OpenCvImageProcessor : IImageProcessor
    {
        public Task<byte[]> ProcessAsync(Bitmap inputBmp, ImageEncodingInfo encodingInfo, CancellationToken token)
        {
            int width = inputBmp.Width; ;
            int height = inputBmp.Height; ;
            int channels = encodingInfo.Channels;
            
            byte[] inputBytes = ImageHelper.ConvertBitmapToRgbBytes(inputBmp, encodingInfo);

            byte[] resultBytes = new GaussianBlurCoreProcessor().ApplyGaussianBlur(inputBytes, width, height, channels);

            byte[] finalBytes;
            using (Bitmap outputBmp = ImageHelper.ConvertRawBytesToBitmap(resultBytes, width, height, encodingInfo))
            {
                finalBytes = ImageHelper.ConvertBitmapToBytes(outputBmp, encodingInfo);
            }
            
            return Task.FromResult(finalBytes);
        }

    }
}
