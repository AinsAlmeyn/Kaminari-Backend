using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Core.Entities.Database.MongoDb;

namespace WMS.Core.Entities.Api.Watch2GetherApi.BaseObjects
{
    public class TogetherRoom : MongoDbBaseEntity
    {
        public string? RoomConnectionString { get; set; }
        public DateTime? CreateDate { get; set; }
        public List<string>? ActiveUsers { get; set; }
    }
}