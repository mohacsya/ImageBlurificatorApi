using Microsoft.AspNetCore.Mvc;

namespace ImageBlurificatorApi.Controllers
{
    public class ImageProcessorController : Controller
    {
        [HttpPost]
        [Route("process")]
        public async Task<IActionResult> ProcessImage()
        {
            throw new NotImplementedException();
        }
    }
}
