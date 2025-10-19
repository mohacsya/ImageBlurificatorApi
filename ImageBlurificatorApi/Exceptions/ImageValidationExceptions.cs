namespace ImageBlurificatorApi.Exceptions
{
    public class ImageValidationException : Exception
    {
        public ImageValidationException(string message) : base(message) { }
    }

    public class ImageSizeLimitExceededException : ImageValidationException
    {
        public ImageSizeLimitExceededException(string message) : base(message) { }
    }

    public class UnsupportedImageFormatException : ImageValidationException
    {
        public UnsupportedImageFormatException(string message) : base(message) { }
    }
}
