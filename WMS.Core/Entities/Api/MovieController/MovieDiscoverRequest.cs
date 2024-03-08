using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.MovieController
{
    public class MovieDiscoverRequest
    {
        public bool? include_adult { get; set; } = false;
        public string? language { get; set; } = "tr-TR";
        public int? page { get; set; } = 1;
    }

    public class MovieRecommendationRequest
    {
        public string? id { get; set; }
        public string? language { get; set; } = "tr-TR";
        public int? page { get; set; } = 1;
    }

    public class MovieDetailRequest
    {
        public string? id { get; set; }
        public string? language { get; set; } = "tr-TR";
        public string? append_to_response { get; set; } = "videos,images";
    }

    public class MovieSearchRequest
    {
        public string? query { get; set; }
        public string? language { get; set; } = "tr-TR";
        public int? page { get; set; } = 1;
    }
}