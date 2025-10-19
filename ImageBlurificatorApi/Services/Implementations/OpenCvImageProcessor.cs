using ImageBlurificatorApi.Services.Interfaces;
using GaussianBlurCore;
using ImageBlurificatorApi.Utilities;
using System.Drawing;
using ImageBlurificatorApi.Models.Internal;

namespace ImageBlurificatorApi.Services.Implementations
{
    /// <summary>
    /// Provides image processing functionality using OpenCV-based Gaussian blur.
    /// </summary>
    public class OpenCvImageProcessor : IImageProcessor
    {
        /// <summary>
        /// Applies a Gaussian blur to the provided bitmap using native OpenCV interop, and returns the processed image as a byte array in the specified encoding.
        /// </summary>
        /// <param name="inputBmp">The input image as a Bitmap</param>
        /// <param name="encodingInfo">Encoding information for the output image.</param>
        /// <param name="token">Cancellation token for cooperative cancellation.</param>
        /// <returns>A task containing the processed image as a byte array in the requested encoding.</returns>
        public Task<byte[]> ProcessAsync(Bitmap inputBmp, ImageEncodingInfo encodingInfo, CancellationToken token)
        {
            // Check for cancellation before starting processing
            token.ThrowIfCancellationRequested();

            int width = inputBmp.Width;
            int height = inputBmp.Height;
            int channels = encodingInfo.Channels;

            // Bitmap -> Raw RGB byte array
            byte[] inputBytes = ImageHelper.ConvertBitmapToRgbBytes(inputBmp, encodingInfo);
            token.ThrowIfCancellationRequested();

            // Apply Gaussian blur using the native processor (interop with C++/OpenCV)
            byte[] resultBytes = new GaussianBlurCoreProcessor().ApplyGaussianBlur(inputBytes, width, height, channels);
            token.ThrowIfCancellationRequested();

            // RGB byte array -> Bitmap -> Encoded byte array
            byte[] finalBytes;
            using (Bitmap outputBmp = ImageHelper.ConvertRgbBytesToBitmap(resultBytes, width, height, encodingInfo))
            {
                finalBytes = ImageHelper.ConvertBitmapToBytes(outputBmp, encodingInfo);
            }
            token.ThrowIfCancellationRequested();

            return Task.FromResult(finalBytes);
        }
    }
}
