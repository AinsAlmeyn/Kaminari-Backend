namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects.AnimeAPIs
{
    public class SearchFilter
    {
        public string? userId { get; set; }
        public string? q { get; set; }
        public bool? sfw { get; set; }
        public bool? unapproved { get; set; }
        public int? page { get; set; }
        public int? limit { get; set; }
        public string? type { get; set; }
        public double? score { get; set; }
        public double? min_score { get; set; }
        public double? max_score { get; set; }
        public string? status { get; set; }
        public string? rating { get; set; }
        public string? genres { get; set; }
        public string? genres_exclude { get; set; }
        public string? order_by { get; set; }
        public string? sort { get; set; }
        public string? letter { get; set; }
        public string? producers { get; set; }
        public string? start_date { get; set; }
        public string? end_date { get; set; }
    }
}