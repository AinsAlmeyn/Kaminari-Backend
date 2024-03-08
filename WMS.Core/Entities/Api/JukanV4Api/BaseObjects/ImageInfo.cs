namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects
{
    public class ImageInfo
    {
        public ImageDetails? Jpg { get; set; }
        public ImageDetails? Webp { get; set; }
    }

    public class ImageDetails
    {
        public string? image_url { get; set; }
        public string? small_image_url { get; set; }
        public string? large_image_url { get; set; }
    }

    public class AnimeImages
    {
        public List<ImageInfo> data { get; set; }
    }

}