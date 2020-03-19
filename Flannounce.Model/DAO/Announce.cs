using System;
using Flannounce.Model.DAO.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Flannounce.Model.DAO
{
    public class Announce
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string AnnounceId { get; set; }

        public string Title { get; set; }
        
        public string Url { get; set; }
        
        public string City { get; set; }
        
        public string District { get; set; }
        
        public string Description { get; set; } 
        
        public string Rooms { get; set; }
        
        public string Floor { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public BuildingType? BuildingType { get; set; } 
        
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.Boolean)]
        public bool IsFromDeveloper { get; set; }
        
        public bool IncludesFurniture { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public OfferedBy? OfferedBy { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Price {get; set;}
        
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? PricePerSquareMeter {get; set;}
        
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? LivingSpace {get; set;}
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? ScrapedAt { get; set; }

        public byte[] Image { get; set; }
    }
}