using ImageBlurificatorApi.Services.Interfaces;
using GaussianBlurCore;
using ImageBlurificatorApi.Utilities;
using System.Drawing;
using ImageBlurificatorApi.Models.Internal;

namespace ImageBlurificatorApi.Services.Implementations
{
    /// <summary>
    /// Provides image processing functionality using OpenCV-based Gaussian blur.
    /// </summary>
    public sealed class OpenCvImageProcessor : IImageProcessor
    {
        private readonly GaussianBlurCoreProcessor _native;

        public OpenCvImageProcessor(GaussianBlurCoreProcessor nativeProcessor)
        {
            _native = nativeProcessor;
        }
        public Task<byte[]> ProcessAsync(Bitmap inputBmp, ImageEncodingInfo encodingInfo, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // Normalize & extract packed buffer in output format
            byte[] packedInput = ImageHelper.ExtractPackedBuffer(inputBmp, encodingInfo.PixelFormat,
                                                                 out int width, out int height, out int channels);

            int expected = width * height * channels;
            if (packedInput.Length != expected)
                throw new InvalidOperationException("Packed input length mismatch.");

            byte[] packedBlurred = _native.ApplyGaussianBlur(packedInput, width, height, channels);

            if (packedBlurred.Length != expected)
                throw new InvalidOperationException("Native output length mismatch.");

            byte[] encoded = ImageHelper.EncodePackedBuffer(packedBlurred, width, height, encodingInfo);

            return Task.FromResult(encoded);
        }
    }
}