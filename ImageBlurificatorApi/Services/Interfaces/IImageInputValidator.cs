using Microsoft.AspNetCore.Http;

namespace ImageBlurificatorApi.Services.Interfaces
{
    public interface IImageInputValidator
    {
        /// <summary>
        /// Validate base64 string length and (optionally) preflight size estimates.
        /// </summary>
        void ValidateBase64Envelope(string base64, long maxDecodedBytes);

        /// <summary>
        /// Validate decoded raw bytes length.
        /// </summary>
        void ValidateDecodedLength(long decodedLength, long maxDecodedBytes);

        /// <summary>
        /// Validate image dimensions and pixel count.
        /// </summary>
        void ValidateDimensions(int width, int height, long maxPixels);

        /// <summary>
        /// Validate uploaded file envelope (content type + length).
        /// </summary>
        void ValidateFileEnvelope(IFormFile file, long maxFileBytes, IReadOnlyCollection<string> allowedContentTypes);

        /// <summary>
        /// Validate requested encoding.
        /// </summary>
        void ValidateRequestedEncoding(string encodingName, IReadOnlyCollection<string> allowedEncodings);
    }
}