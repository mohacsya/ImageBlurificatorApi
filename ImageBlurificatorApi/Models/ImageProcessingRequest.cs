namespace ImageBlurificatorApi.Models
{
    /// <summary>
    /// Simple DTO for image processing request.
    /// </summary>
    public class ImageProcessingRequest
    {
        /// <summary>
        /// Base64 raw string of the image to be processed.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// The desired output encoding format.
        /// </summary>
        public EncodingType OutputEncoding { get; set; }
    }
}
