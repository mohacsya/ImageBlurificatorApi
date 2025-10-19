using System.Drawing;
using ImageBlurificatorApi.Exceptions;
using ImageBlurificatorApi.Models.Internal;
using ImageBlurificatorApi.Models.Requests;
using ImageBlurificatorApi.Services.Interfaces;
using ImageBlurificatorApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ImageBlurificatorApi.Controllers
{
    /// <summary>
    /// API controller for image processing operations.
    /// </summary>
    public class ImageProcessorController : Controller
    {
        private readonly IImageProcessor _processor;
        private readonly IImageInputValidator _validator;
        private readonly ImageProcessingValidationOptions _limits;

        public ImageProcessorController(IImageProcessor processor, IImageInputValidator imageInputValidator, IOptions<ImageProcessingValidationOptions> options)
        {
            _processor = processor;
            _validator = imageInputValidator;
            _limits = options.Value;
        }

        /// <summary>
        /// Processes the provided base64 image data and returns the processed image in the specified encoding format.
        /// </summary>
        /// <param name="request">The ImageProcessingRequest object encapsulating the image data and encoding.</param>
        /// <param name="token">Cancellation token for the request.</param>
        /// <returns>The processed image as a file in the specified encoding.</returns>
        [HttpPost]
        [Route("image_processing")]
        public async Task<IActionResult> ImageProcessing([FromForm] ImageProcessingRequest request, CancellationToken token)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Image))
                return BadRequest("Image data is required.");

            try
            {
                _validator.ValidateRequestedEncoding(request.OutputEncoding.ToString(), _limits.AllowedEncodings);
                _validator.ValidateBase64Envelope(request.Image, _limits.MaxDecodedBytes);
                ImageEncodingInfo encodingInfo = ImageHelper.GetEncodingInfo(request.OutputEncoding);

                // Convert base64 string to Bitmap and ensure disposal
                byte[] processedImageBytes;
                using (Bitmap imageBmp = ImageHelper.ConvertBase64ToBitmap(request.Image))
                {
                    using (var ms = new MemoryStream())
                    {
                        imageBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        _validator.ValidateDecodedLength(ms.Length, _limits.MaxDecodedBytes);
                    }
                    _validator.ValidateDimensions(imageBmp.Width, imageBmp.Height, _limits.MaxPixels);

                    processedImageBytes = await _processor.ProcessAsync(imageBmp, encodingInfo, token);
                }

                return File(processedImageBytes, encodingInfo.MimeType, enableRangeProcessing: false);
            }
            catch (ImageSizeLimitExceededException ex)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge, new { error = ex.Message });
            }
            catch (UnsupportedImageFormatException ex)
            {
                return StatusCode(StatusCodes.Status415UnsupportedMediaType, new { error = ex.Message });
            }
            catch (ImageValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                // Log exception (not shown here)
                return Problem(title: "An unexpected error occurred while processing the image.", statusCode: 500);
            }
        }

        /// <summary>
        /// Processes the provided image file and returns the processed image in the specified encoding format.
        /// </summary>
        /// <param name="request">The ImageFileProcessingRequest object encapsulating the image and encoding.</param>
        /// <param name="token">Cancellation token for the request.</param>
        /// <returns>The processed image as a file in the specified encoding.</returns>
        [HttpPost]
        [Route("image_file_processing")]
        public async Task<IActionResult> ImageFileProcessing([FromForm] ImageFileProcessingRequest request, CancellationToken token)
        {
            if (request?.Image == null || request.Image.Length == 0)
                return BadRequest("No image file uploaded.");

            try
            {
                _validator.ValidateRequestedEncoding(request.OutputEncoding.ToString(), _limits.AllowedEncodings);
                _validator.ValidateFileEnvelope(request.Image, _limits.MaxFileBytes, _limits.AllowedContentTypes);

                ImageEncodingInfo encodingInfo = ImageHelper.GetEncodingInfo(request.OutputEncoding);

                byte[] processedImageBytes;
                // Open image stream and ensure Bitmap disposal
                using (var stream = request.Image.OpenReadStream())
                using (var imageBmp = new Bitmap(stream))
                {
                    _validator.ValidateDimensions(imageBmp.Width, imageBmp.Height, _limits.MaxPixels);
                    processedImageBytes = await _processor.ProcessAsync(imageBmp, encodingInfo, token);
                }

                return File(processedImageBytes, encodingInfo.MimeType);
            }
            catch (ImageSizeLimitExceededException ex)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge, new { error = ex.Message });
            }
            catch (UnsupportedImageFormatException ex)
            {
                return StatusCode(StatusCodes.Status415UnsupportedMediaType, new { error = ex.Message });
            }
            catch (ImageValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return Problem(title: "An unexpected error occurred while processing the image.", statusCode: 500);
            }
        }
    }
}
