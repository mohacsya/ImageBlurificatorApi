using ImageBlurificatorApi.Exceptions;
using ImageBlurificatorApi.Services.Interfaces;
using System.Text.RegularExpressions;

namespace ImageBlurificatorApi.Services.Implementations
{
    public sealed class ImageInputValidator : IImageInputValidator
    {
        // Rough heuristic to filter obviously huge base64 before decode:
        // Base64 expands bytes by ~4/3.
        private static readonly Regex Base64DataUriPrefix = new("^data:image/[^;]+;base64,", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void ValidateBase64Envelope(string base64, long maxDecodedBytes)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new ImageValidationException("Empty base64 payload.");

            if (Base64DataUriPrefix.IsMatch(base64))
            {
                base64 = Base64DataUriPrefix.Replace(base64, string.Empty);
            }

            // Estimate decoded size: (length * 3 / 4) ignoring padding; add small margin.
            long estimatedDecoded = (long)(base64.Length * 0.75);
            if (estimatedDecoded > maxDecodedBytes * 1.1) // 10% cushion
                throw new ImageSizeLimitExceededException($"Estimated decoded size {estimatedDecoded:N0} exceeds limit {maxDecodedBytes:N0}.");
        }

        public void ValidateDecodedLength(long decodedLength, long maxDecodedBytes)
        {
            if (decodedLength <= 0)
                throw new ImageValidationException("Decoded image stream is empty.");
            if (decodedLength > maxDecodedBytes)
                throw new ImageSizeLimitExceededException($"Decoded image size {decodedLength:N0} exceeds limit {maxDecodedBytes:N0} bytes.");
        }

        public void ValidateDimensions(int width, int height, long maxPixels)
        {
            if (width <= 0 || height <= 0)
                throw new ImageValidationException("Image width/height must be positive.");
            long pixels = (long)width * height;
            if (pixels > maxPixels)
                throw new ImageSizeLimitExceededException($"Pixel count {pixels:N0} exceeds limit {maxPixels:N0}.");
        }

        public void ValidateFileEnvelope(IFormFile file, long maxFileBytes, IReadOnlyCollection<string> allowedContentTypes)
        {
            if (file == null)
                throw new ImageValidationException("No file provided.");
            if (file.Length == 0)
                throw new ImageValidationException("Uploaded file is empty.");
            if (file.Length > maxFileBytes)
                throw new ImageSizeLimitExceededException($"File size {file.Length:N0} exceeds limit {maxFileBytes:N0}.");
            if (allowedContentTypes.Count > 0 && !allowedContentTypes.Contains(file.ContentType))
                throw new UnsupportedImageFormatException($"Unsupported Content-Type '{file.ContentType}'.");
        }

        public void ValidateRequestedEncoding(string encodingName, IReadOnlyCollection<string> allowedEncodings)
        {
            if (allowedEncodings.Count == 0) return;
            if (!allowedEncodings.Contains(encodingName, StringComparer.OrdinalIgnoreCase))
                throw new UnsupportedImageFormatException($"Requested output encoding '{encodingName}' is not permitted.");
        }
    }
}