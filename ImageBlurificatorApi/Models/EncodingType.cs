using System.Runtime.Serialization;

namespace ImageBlurificatorApi.Models
{
    /// <summary>
    /// Specifies the encoding format for the output image.
    /// </summary>
    public enum EncodingType
    {
        [EnumMember(Value = "Portable")]
        PNG,
        [EnumMember(Value = "JPEG")]
        JPG
    }
}
