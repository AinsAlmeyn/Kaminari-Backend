using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.YoutubeV3Api
{
    public class YTVideoDetailResponse
    {
        public string? kind { get; set; }
        public string? etag { get; set; }
        public List<YTVideoDetailItem>? items { get; set; }
        public YTVideoDetailPageInfo? pageInfo { get; set; }
        public string? nextPageInformation { get; set; }
        public string? prevPageToken { get; set; }
    }

    public class YTVideoDetailItem
    {
        public string? kind { get; set; }
        public string? etag { get; set; }
        public string? id { get; set; }
        public YTVideoDetailSnippet? snippet { get; set; }
        public YTVideoDetailContentDetails? contentDetails { get; set; }
        public YTVideoDetailStatistics? statistics { get; set; }
    }

    public class YTVideoDetailSnippet
    {
        public DateTime? publishedAt { get; set; }
        public string? channelId { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public YTVideoDetailThumbnails? thumbnails { get; set; }
        public string? channelTitle { get; set; }
        public string? categoryId { get; set; }
        public string? liveBroadcastContent { get; set; }
        public string? defaultLanguage { get; set; }
        public YTVideoDetailLocalized? localized { get; set; }
        public string? defaultAudioLanguage { get; set; }
    }

    public class YTVideoDetailThumbnails
    {
        public YTVideoDetailThumbnail? @default { get; set; }
        public YTVideoDetailThumbnail? medium { get; set; }
        public YTVideoDetailThumbnail? high { get; set; }
        public YTVideoDetailThumbnail? standard { get; set; }
        public YTVideoDetailThumbnail? maxres { get; set; }
    }

    public class YTVideoDetailThumbnail
    {
        public string? url { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
    }

    public class YTVideoDetailLocalized
    {
        public string? title { get; set; }
        public string? description { get; set; }
    }

    public class YTVideoDetailContentDetails
    {
        public string? duration { get; set; }
        public string? dimension { get; set; }
        public string? definition { get; set; }
        public string? caption { get; set; }
        public bool? licensedContent { get; set; }
        public YTVideoDetailContentRating? contentRating { get; set; }
        public string? projection { get; set; }
    }

    public class YTVideoDetailContentRating
    {
        // This class can be expanded based on specific content rating information.
    }

    public class YTVideoDetailStatistics
    {
        public string? viewCount { get; set; }
        public string? likeCount { get; set; }
        public string? favoriteCount { get; set; }
        public string? commentCount { get; set; }
    }

    public class YTVideoDetailPageInfo
    {
        public int? totalResults { get; set; }
        public int? resultsPerPage { get; set; }
    }

}