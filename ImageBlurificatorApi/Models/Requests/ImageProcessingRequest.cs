using System.ComponentModel.DataAnnotations;
using ImageBlurificatorApi.Models.Internal;

namespace ImageBlurificatorApi.Models.Requests
{
    /// <summary>
    /// Simple DTO for image processing request.
    /// </summary>
    public class ImageProcessingRequest
    {
        /// <summary>
        /// Base64 raw string of the image to be processed.
        /// </summary>
        [Required]
        public string Image { get; set; }

        /// <summary>
        /// The desired output encoding format.
        /// </summary>
        [Required]
        public EncodingType OutputEncoding { get; set; }
    }
}
