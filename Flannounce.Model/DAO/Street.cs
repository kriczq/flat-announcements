using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Flannounce.Model.DAO
{
    public class Street 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string City { get; set; }
        
        public string Name { get; set; }
    }
}