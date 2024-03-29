using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.TmdbV3Api.BaseObjects
{
    //! This class is used to deserialize the response from the TMDb API
    //! SEARCH: BaseMovie
    //! DISCOVER MOVIE : BaseMovie
    //! RECOMMENDATIONS : BaseMovie
    public class BaseMovie
    {
        public int? page { get; set; }
        public List<Movie>? results { get; set; }
        public int? total_pages { get; set; }
        public int? total_results { get; set; }
    }

    public class Movie
    {
        public bool? adult { get; set; }
        public string? backdrop_path { get; set; }
        public List<int>? genre_ids { get; set; }
        public int? id { get; set; }
        public string? original_language { get; set; }
        public string? original_title { get; set; }
        public string? overview { get; set; }
        public double? popularity { get; set; }
        public string? poster_path { get; set; }
        public string? release_date { get; set; }
        public string? title { get; set; }
        public bool? video { get; set; }
        public double? vote_average { get; set; }
        public int? vote_count { get; set; }
        public string? media_type { get; set; }
    }
}