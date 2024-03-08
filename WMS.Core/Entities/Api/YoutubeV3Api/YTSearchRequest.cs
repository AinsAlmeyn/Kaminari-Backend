using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.YoutubeV3Api
{
    public class YTSearchRequest
    {
        public string? type { get; set; }
        public bool? videoEmbeddable { get; set; }
        public string? pageToken { get; set; }
        public string? q { get; set; }
    }
}