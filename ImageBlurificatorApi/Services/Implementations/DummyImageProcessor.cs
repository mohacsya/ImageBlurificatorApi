using ImageBlurificatorApi.Models;
using ImageBlurificatorApi.Services.Interfaces;

namespace ImageBlurificatorApi.Services.Implementations
{
    public class DummyImageProcessor : IImageProcessor
    {
        public Task<byte[]> ProcessAsync(string imageBase64, EncodingType encoding, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(imageBase64))
                throw new ArgumentException("Image data must not be null or empty.", nameof(imageBase64));

            // Optionally check for cancellation
            token.ThrowIfCancellationRequested();

            // Decode the base64 string to a byte array
            byte[] imageBytes = Convert.FromBase64String(imageBase64);

            // Return as a completed Task
            return Task.FromResult(imageBytes);
        }
    }
}
