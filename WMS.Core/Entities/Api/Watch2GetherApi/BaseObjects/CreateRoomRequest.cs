using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.Watch2GetherApi.BaseObjects
{
    public class CreateRoomRequest
    {
        public string? w2g_api_key { get; set; }
        public string? share { get; set; }
        public string? bg_color { get; set; }
        public string? bg_opacity { get; set; }
    }
}