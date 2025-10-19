using System.Drawing;
using ImageBlurificatorApi.Models.Internal;
using ImageBlurificatorApi.Services.Interfaces;
using ImageBlurificatorApi.Utilities;

namespace ImageBlurificatorApi.Services.Implementations
{
    /// <summary>
    /// A dummy implementation of IImageProcessor that simply  returns the original image as a byte array.
    /// </summary>
    public class DummyImageProcessor : IImageProcessor
    {

        public Task<byte[]> ProcessAsync(Bitmap imageBmp, ImageEncodingInfo encodingInfo, CancellationToken token)
        {
            // Optionally check for cancellation
            token.ThrowIfCancellationRequested();

            // Decode the Bitmap string to a byte array
            byte[] imageBytes = ImageHelper.ExtractPackedBuffer(imageBmp, encodingInfo.PixelFormat, out int width, out int height, out int channels);

            // Return as a completed Task
            return Task.FromResult(imageBytes);
        }
    }
}
