using System;
using Flannounce.Model.DAO.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Flannounce.Model.DAO
{
    public class Flat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FlatId { get; set; }
        
        public string Title { get; set; }
        
        public string Url { get; set; }
        
        public string City { get; set; }
        
        public string District { get; set; }
        
        public string Description { get; set; } 
        
        public int? RoomCount { get; set; }
        
        public int? Floor { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public BuildingType? BuildingType { get; set; } 
        
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Market? Market { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public OfferedBy? OfferedBy { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Price {get; set;}
        
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Area {get; set;}
        
        public DateTime? AdditionDate { get; set; }
        
        public DateTime? ScrapedDate { get; set; }

        public byte[] Image { get; set; }
    }
}