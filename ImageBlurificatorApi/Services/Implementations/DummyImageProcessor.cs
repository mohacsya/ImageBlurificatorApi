using System.Drawing;
using ImageBlurificatorApi.Models.Internal;
using ImageBlurificatorApi.Services.Interfaces;
using ImageBlurificatorApi.Utilities;

namespace ImageBlurificatorApi.Services.Implementations
{
    /// <summary>
    /// A dummy implementation of IImageProcessor that simply decodes the base64 image string and returns it as a byte array.
    /// </summary>
    public class DummyImageProcessor : IImageProcessor
    {

        public Task<byte[]> ProcessAsync(Bitmap imageBmp, ImageEncodingInfo encodingInfo, CancellationToken token)
        {
            // Optionally check for cancellation
            token.ThrowIfCancellationRequested();

            // Decode the base64 string to a byte array
            byte[] imageBytes = ImageHelper.ConvertBitmapToRgbBytes(imageBmp, encodingInfo);

            // Return as a completed Task
            return Task.FromResult(imageBytes);
        }
    }
}
