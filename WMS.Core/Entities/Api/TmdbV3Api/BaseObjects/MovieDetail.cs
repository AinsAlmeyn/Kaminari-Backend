using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.TmdbV3Api.BaseObjects
{
    //! This class is used to deserialize the response from the TMDb API
    //! MOVIE DETAIL: MovieDetail
    //! MOVIE VIDEOS: MovieDetail
    public class MovieDetail
    {
        public bool? adult { get; set; }
        public string? backdrop_path { get; set; }
        public Collection? belongs_to_collection { get; set; }
        public long? budget { get; set; }
        public List<Genre>? genres { get; set; }
        public string? homepage { get; set; }
        public int? id { get; set; }
        public string? imdb_id { get; set; }
        public string? original_language { get; set; }
        public string? original_title { get; set; }
        public string? overview { get; set; }
        public double? popularity { get; set; }
        public string? poster_path { get; set; }
        public List<ProductionCompany>? production_companies { get; set; }
        public List<ProductionCountry>? production_countries { get; set; }
        public string? release_date { get; set; }
        public long? revenue { get; set; }
        public int? runtime { get; set; }
        public List<SpokenLanguage>? spoken_languages { get; set; }
        public string? status { get; set; }
        public string? tagline { get; set; }
        public string? title { get; set; }
        public double? vote_average { get; set; }
        public int? vote_count { get; set; }
        public VideoCollection? videos { get; set; }
        public bool? video { get; set; }
    }
    public class VideoCollection
    {
        public List<Video>? results { get; set; }
    }
    public class Video
    {
        public string? iso_639_1 { get; set; }
        public string? iso_3166_1 { get; set; }
        public string? name { get; set; }
        public string? key { get; set; }
        public string? site { get; set; }
        public int? size { get; set; }
        public string? type { get; set; }
        public bool? official { get; set; }
        public string? published_at { get; set; }
        public string? id { get; set; }
    }
    public class Collection
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? poster_path { get; set; }
        public string? backdrop_path { get; set; }
    }
    public class Genre
    {
        public int? id { get; set; }
        public string? name { get; set; }
    }
    public class ProductionCompany
    {
        public int? id { get; set; }
        public string? logo_path { get; set; }
        public string? name { get; set; }
        public string? origin_country { get; set; }
    }
    public class ProductionCountry
    {
        public string? iso_3166_1 { get; set; }
        public string? name { get; set; }
    }
    public class SpokenLanguage
    {
        public string? english_name { get; set; }
        public string? iso_639_1 { get; set; }
        public string? name { get; set; }
    }
}