using ImageBlurificatorApi.Models;
using ImageBlurificatorApi.Services.Interfaces;
using GaussianBlurCore;

namespace ImageBlurificatorApi.Services.Implementations
{
    public class OpenCvImageProcessor : IImageProcessor
    {

        public Task<byte[]> ProcessAsync(string imageBase64, EncodingType encoding, CancellationToken token)
        {
            var processor = new GaussianBlurCoreProcessor();
            return null;
        }

    }
}
