using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.YoutubeV3Api
{
    public class YTSearchResponse
    {
        public string? kind { get; set; }
        public string? etag { get; set; }
        public string? nextPageToken { get; set; }
        public string? prevPageToken { get; set; }
        public string? regionCode { get; set; }
        public YTSearchPageInfo? pageInfo { get; set; }
        public List<YTSearchItem>? items { get; set; }
    }
    public class YTSearchPageInfo
    {
        public int? totalResults { get; set; }
        public int? resultsPerPage { get; set; }
    }

    public class YTSearchItem
    {
        public string? kind { get; set; }
        public string? etag { get; set; }
        public YTSearchId? id { get; set; }
    }

    public class YTSearchId
    {
        public string? kind { get; set; }
        public string? videoId { get; set; }
    }

}