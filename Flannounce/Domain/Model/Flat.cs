using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Flannounce.Domain.Model
{
    public class Flat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FlatId { get; set; }
        
        public string Title { get; set; }
    }
}