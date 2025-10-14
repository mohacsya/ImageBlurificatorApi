using ImageBlurificatorApi.Models;

namespace ImageBlurificatorApi.Services.Interfaces
{
    public interface IImageProcessor
    {
        Task<byte[]> ProcessAsync(string imageBase64, EncodingType encoding, CancellationToken token);
    }
}
