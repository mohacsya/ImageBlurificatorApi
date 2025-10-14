using System.Text;
using System.Xml.Linq;
using ImageBlurificatorApi.Models;
using ImageBlurificatorApi.Services.Interfaces;
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
        /// Processes the provided image and returns the processed image in the specified encoding format.
        /// </summary>
        /// <param name="request">The ImageProcessingRequest object.</param>
        /// <param name="token"></param>
        /// <returns> The processed image as file in the specified encoding.</returns>
        [HttpPost]
        [Route("process")]
        public async Task<IActionResult> ProcessImage([FromForm] ImageProcessingRequest request, CancellationToken token)
        {

            var processedImage = await _processor.ProcessAsync(request.Image, request.OutputEncoding, token);

            string mimeType = request.OutputEncoding == EncodingType.PNG ? "image/png" : "image/jpeg";
            return File(processedImage, mimeType);
        }
    }
}
