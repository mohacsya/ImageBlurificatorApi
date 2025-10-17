using ImageBlurificatorApi.Models;
using ImageBlurificatorApi.Services.Interfaces;
using GaussianBlurCore;
using ImageBlurificatorApi.Utilities;
using System.Drawing;

namespace ImageBlurificatorApi.Services.Implementations
{
    public class OpenCvImageProcessor : IImageProcessor
    {

        public Task<byte[]> ProcessAsync(string imageBase64, EncodingType encoding, CancellationToken token)
        {
            Console.WriteLine(File.Exists("GaussianBlurCore.dll"));
            int width;
            int height;
            int channels = 3; 
            byte[] inputBytes;
            using (Bitmap inputBmp = ImageHelper.ConvertBase64ToBitmap(imageBase64))
            {
                inputBytes = ImageHelper.GetRawImageBytes(inputBmp);
                width = inputBmp.Width; 
                height = inputBmp.Height;
            }
            var processor = new GaussianBlurCoreProcessor();           
            byte[] resultBytes = processor.ApplyGaussianBlur(inputBytes, width, height, channels);
            byte[] finalBytes;
            using (Bitmap outputBmp = ImageHelper.ConvertRawBytesToBitmap(resultBytes, width, height))
            {
                finalBytes = ImageHelper.ConvertBitmapToBytes(outputBmp, encoding);
                outputBmp.Save("blurred_output.png", System.Drawing.Imaging.ImageFormat.Png);
            }
            return Task.FromResult(finalBytes);
        }

    }
}
