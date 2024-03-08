using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.Watch2GetherApi.BaseObjects
{
    public class UpdateRoomRequest
    {
        public string? w2g_api_key { get; set; }
        public string? item_url { get; set; }
        public string? streamkey { get; set; }
    }

    public class UpdateRoomTogetherRequest
    {
        public string? w2g_api_key { get; set; }
        public string? item_url { get; set; }
    }
}