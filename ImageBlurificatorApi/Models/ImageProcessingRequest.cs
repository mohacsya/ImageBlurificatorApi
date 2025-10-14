namespace ImageBlurificatorApi.Models
{
    public class ImageProcessingRequest
    {
        public string Image { get; set; }
        public EncodingType OutputEncoding { get; set; }
    }
}
