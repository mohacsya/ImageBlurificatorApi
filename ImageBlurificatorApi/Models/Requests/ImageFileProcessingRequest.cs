using System.ComponentModel.DataAnnotations;
using ImageBlurificatorApi.Models.Internal;

namespace ImageBlurificatorApi.Models.Requests
{
    /// <summary>
    /// Simple DTO for image processing request.
    /// </summary>
    public class ImageFileProcessingRequest
    {
        /// <summary>
        /// The image to be processed.
        /// </summary>
        [Required]
        public IFormFile Image { get; set; }

        /// <summary>
        /// The desired output encoding format.
        /// </summary>
        [Required]
        public EncodingType OutputEncoding { get; set; }
    }
}
