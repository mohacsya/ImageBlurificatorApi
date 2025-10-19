using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml.Linq;
using ImageBlurificatorApi.Models.Internal;
using ImageBlurificatorApi.Models.Requests;
using ImageBlurificatorApi.Services.Interfaces;
using ImageBlurificatorApi.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace ImageBlurificatorApi.Controllers
{
    public class ImageProcessorController : Controller
    {
        private readonly IImageProcessor _processor;

        public ImageProcessorController(IImageProcessor processor)
        {
            _processor = processor;
        }

        /// <summary>
        /// Processes the provided base64 image data and returns the processed image in the specified encoding format.
        /// </summary>
        /// <param name="request">The ImageProcessingRequest object encapsulating the image data and encoding.</param>
        /// <param name="token"></param>
        /// <returns> The processed image as file in the specified encoding.</returns>
        [HttpPost]
        [Route("image_processing")]
        public async Task<IActionResult> ImageProcessing([FromForm] ImageProcessingRequest request, CancellationToken token)
        {
            ImageEncodingInfo encodingInfo = ImageHelper.GetEncodingInfo(request.OutputEncoding);
            byte[] processedImageBytes;
            using (Bitmap imageBmp = ImageHelper.ConvertBase64ToBitmap(request.Image))
            {
                processedImageBytes = await _processor.ProcessAsync(imageBmp, encodingInfo, token);
            }
            return File(processedImageBytes, encodingInfo.MimeType, enableRangeProcessing: false);
        }

        /// <summary>
        /// Processes the provided image and returns the processed image in the specified encoding format.
        /// </summary>
        /// <param name="request">The ImageFileProcessingRequest object encapsulating the image and encoding.</param>
        /// <param name="token"></param>
        /// <returns> The processed image as file in the specified encoding.</returns>
        [HttpPost]
        [Route("image_file_processing")]
        public async Task<IActionResult> ImageFileProcessing([FromForm] ImageFileProcessingRequest request, CancellationToken token)
        {
            ImageEncodingInfo encodingInfo = ImageHelper.GetEncodingInfo(request.OutputEncoding);
            if (request.Image == null || request.Image.Length == 0)
                return BadRequest("No image file uploaded.");

            byte[] processedImageBytes;
            using (var stream = request.Image.OpenReadStream())
            using (var imageBmp = new Bitmap(stream))
            {
                processedImageBytes = await _processor.ProcessAsync(imageBmp, encodingInfo, token);
            }
            return File(processedImageBytes, encodingInfo.MimeType);
        }
    }
}
