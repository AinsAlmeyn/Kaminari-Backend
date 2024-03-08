using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Core.Entities.Api.JukanV4Api.BaseObjects;

namespace WMS.Core.Entities.Database.MongoDb
{

    public class FetchMyList
    {
        public MyAnimeList? myanimelist { get; set; }
        public string? username { get; set; }
        public string? userId { get; set; }
    }

    public class SelectUserId
    {
        public string? userId { get; set; }
    }

    public class SelectAnimeId
    {
        public int? series_animedb_id { get; set; }
    }

    public class MyAnimeList
    {
        public UserAnimeProfile? myinfo { get; set; }
        public List<UserAnime>? anime { get; set; }
    }

    public class UserAnimeProfile : MongoDbBaseEntity
    {
        public string? user_id { get; set; }
        public string? user_name { get; set; }
        public int? user_export_type { get; set; }
        public int? user_total_anime { get; set; }
        public int? user_total_watching { get; set; }
        public int? user_total_completed { get; set; }
        public int? user_total_onhold { get; set; }
        public int? user_total_dropped { get; set; }
        public int? user_total_plantowatch { get; set; }
    }

    public class UserAnime : MongoDbBaseEntity
    {
        public int? series_animedb_id { get; set; }
        public string? series_title { get; set; }
        public string? series_type { get; set; }
        public int? series_episodes { get; set; }
        public int? my_id { get; set; }
        public int? my_watched_episodes { get; set; }
        public string? my_start_date { get; set; }
        public string? my_finish_date { get; set; }
        public string? my_rated { get; set; }
        public int? my_score { get; set; }
        public string? my_storage { get; set; }
        public double? my_storage_value { get; set; }
        public string? my_status { get; set; }
        public string? my_old_status { get; set; }
        public string? my_comments { get; set; }
        public int? my_times_watched { get; set; }
        public string? my_rewatch_value { get; set; }
        public string? my_priority { get; set; }
        public string? my_tags { get; set; }
        public int? my_rewatching { get; set; }
        public int? my_rewatching_ep { get; set; }
        public int? my_discuss { get; set; }
        public string? my_sns { get; set; }
        public int? update_on_import { get; set; }
        public string? UserId { get; set; }
    }

}