using ImageBlurificatorApi.Models;

namespace ImageBlurificatorApi.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for image processing services.       
    /// </summary>
    public interface IImageProcessor
    {
        /// <summary>
        /// Processes the given base64-encoded image string and returns the processed image as a byte array in the specified encoding format.
        /// </summary>
        /// <param name="imageBase64">  The image data converted to base64 raw string.</param>
        /// <param name="encoding">     The output encoding.</param>
        /// <param name="token">        Cancellation token for the image processing.</param>
        /// <returns></returns>
        Task<byte[]> ProcessAsync(string imageBase64, EncodingType encoding, CancellationToken token);
    }
}
