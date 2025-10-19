using System.Drawing;
using ImageBlurificatorApi.Models.Internal;

namespace ImageBlurificatorApi.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for image processing services.       
    /// </summary>
    public interface IImageProcessor
    {
        /// <summary>
        /// Processes the provided Bitmap and returns the processed image as a byte array in the specified encoding format.
        /// </summary>
        /// <param name="imageBmp">  The image data converted to base64 raw string.</param>
        /// <param name="encodingInfo">     The encoding info associated with the request.</param>
        /// <param name="token">        Cancellation token for the image processing.</param>
        /// <returns></returns>
        Task<byte[]> ProcessAsync(Bitmap imageBmp, ImageEncodingInfo encodingInfo, CancellationToken token);
    }
}
