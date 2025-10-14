using ImageBlurificatorApi.Models;
using ImageBlurificatorApi.Services.Interfaces;

namespace ImageBlurificatorApi.Services.Implementations
{
    public class DummyImageProcessor : IImageProcessor
    {
        public Task<byte[]> ProcessAsync(string imageBase64, EncodingType encoding, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
