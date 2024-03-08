namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects
{
    public class TrailerImageDetails
    {
        public string? image_url { get; set; }
        public string? small_image_url { get; set; }
        public string? medium_image_url { get; set; }
        public string? large_image_url { get; set; }
        public string? maximum_image_url { get; set; }
    }

    public class TrailerInfo
    {
        public string? youtube_id { get; set; }
        public string? Url { get; set; }
        public string? embed_url { get; set; }
        public TrailerImageDetails? Images { get; set; }
    }
}