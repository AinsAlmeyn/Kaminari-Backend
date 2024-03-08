namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects.AnimeAPIs
{
    public class SearchAnimeInfo
    {
        public PaginationInfo? Pagination { get; set; }
        public List<AnimeData>? Data { get; set; }
    }

    public class IdAnimeInfo
    {
        public AnimeData? Data { get; set; }
    }

    public class AnimeData
    {
        public string? my_status { get; set; }
        public int? my_score { get; set; }
        public int mal_id { get; set; }
        public string? Url { get; set; }
        public bool Approved { get; set; }
        public string? Title { get; set; }
        public string? title_english { get; set; }
        public string? title_japanese { get; set; }
        public string? Type { get; set; }
        public string? Source { get; set; }
        public int? Episodes { get; set; }
        public string? Status { get; set; }
        public bool Airing { get; set; }
        public string? Duration { get; set; }
        public string? Rating { get; set; }
        public decimal? Score { get; set; }
        public int? scored_by { get; set; }
        public int? Rank { get; set; }
        public int? Popularity { get; set; }
        public int? Members { get; set; }
        public int? Favorites { get; set; }
        public string? Synopsis { get; set; }
        public string? Background { get; set; }
        public string? Season { get; set; }
        public int? Year { get; set; }

        public ImageInfo? Images { get; set; }
        public TrailerInfo? Trailer { get; set; }
        public List<AnimeTitleInfo>? Titles { get; set; }
        public List<string>? title_synonyms { get; set; }
        public AirInfo? Aired { get; set; }
        public BroadcastInfo? Broadcast { get; set; }
        public List<Producer>? Producers { get; set; }
        public List<Licensor>? Licensors { get; set; }
        public List<Studio>? Studios { get; set; }
        public List<Genre>? Genres { get; set; }
        List<object>? explicit_genres { get; set; }
        public List<Theme>? Themes { get; set; }
        public List<Demographic>? Demographics { get; set; }
    }
}