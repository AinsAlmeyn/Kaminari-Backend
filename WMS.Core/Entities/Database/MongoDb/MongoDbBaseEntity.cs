using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WMS.Core.Entities.Database.MongoDb
{
    public abstract class MongoDbBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        protected MongoDbBaseEntity()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }
    }
}
