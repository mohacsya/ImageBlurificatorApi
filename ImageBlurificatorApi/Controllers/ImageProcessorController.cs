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

        [HttpPost]
        [Route("process")]
        public async Task<IActionResult> ProcessImage([FromBody] ImageProcessingRequest request, CancellationToken token)
        {

            var processedImage = await _processor.ProcessAsync(request.Image, request.OutputEncoding, token);

            string mimeType = request.OutputEncoding == EncodingType.PNG ? "image/png" : "image/jpeg";
            return File(processedImage, mimeType);
        }
    }
}
