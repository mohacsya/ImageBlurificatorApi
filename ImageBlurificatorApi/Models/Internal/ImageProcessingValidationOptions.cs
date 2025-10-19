namespace ImageBlurificatorApi.Models.Internal
{
    /// <summary>
    /// Configuration-bound validation limits for image processing.
    /// </summary>
    public sealed class ImageProcessingValidationOptions
    {
        /// <summary>
        /// Maximum total pixels (width * height). Default 20,000,000 (~20 MP).
        /// </summary>
        public long MaxPixels { get; set; } = 20_000_000;

        /// <summary>
        /// Maximum allowed uploaded file size in bytes. Default 40 MB.
        /// </summary>
        public long MaxFileBytes { get; set; } = 40_000_000;

        /// <summary>
        /// Maximum decoded base64 image bytes. Default 40 MB.
        /// </summary>
        public long MaxDecodedBytes { get; set; } = 40_000_000;

        /// <summary>
        /// Allowed output encodings (enum names) - if empty, all currently supported encodings allowed.
        /// </summary>
        public string[] AllowedEncodings { get; set; } = new[] { "PNG", "JPG" };

        /// <summary>
        /// Allowed MIME content types for uploaded files.
        /// </summary>
        public string[] AllowedContentTypes { get; set; } = new[] { "image/png", "image/jpeg" };
    }
}