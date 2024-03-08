using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.Watch2GetherApi.BaseObjects
{
    public class EnterRoomRequest
    {
        public string? userName { get; set; }
        public string? roomConnectionString { get; set; }
    }
}