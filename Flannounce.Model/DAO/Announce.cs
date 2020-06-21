using System;
using System.Collections.Generic;
using Flannounce.Model.DAO.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Flannounce.Model.DAO
{
    public class Announce
    {
        private sealed class AnnounceEqualityComparer : IEqualityComparer<Announce>
        {
            public bool Equals(Announce x, Announce y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Equals(y);
            }

            public int GetHashCode(Announce obj)
            {
                unchecked
                {
                    var hashCode = (obj.AnnounceId != null ? obj.AnnounceId.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Title != null ? obj.Title.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Url != null ? obj.Url.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.City != null ? obj.City.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.District != null ? obj.District.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Rooms != null ? obj.Rooms.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Floor != null ? obj.Floor.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ obj.BuildingType.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.IsFromDeveloper.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.IncludesFurniture.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.OfferedBy.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.Price.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.PricePerSquareMeter.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.LivingSpace.GetHashCode();
                    return hashCode;
                }
            }
        }

        public static IEqualityComparer<Announce> AnnounceComparer { get; } = new AnnounceEqualityComparer();

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string AnnounceType { get; set; }
        public string AnnounceId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string City { get; set; }

        public string Street { get; set; }
        
        public string District { get; set; }

        public string Description { get; set; }

        public string Rooms { get; set; }

        public string Floor { get; set; }
        
        public string Latitude { get; set; }
        
        public string Longitude { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public OfferedBy? OfferedBy { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public BuildingType? BuildingType { get; set; }

        [BsonRepresentation(BsonType.Boolean)]
        public bool IsFromDeveloper { get; set; }

        [BsonRepresentation(BsonType.Boolean)]
        public bool IncludesFurniture { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Price { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? PricePerSquareMeter { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? LivingSpace { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ScrapedAt { get; set; }

        public byte[] Image { get; set; }

        public List<string> Images { get; set; }

        public virtual bool Equals(object other)
        {
            if ((other == null) || GetType() != other.GetType())
            {
                return false;
            }

            var announce = (Announce) other;

            if (Url == announce.Url || AnnounceId == announce.AnnounceId)
            {
                return true;
            }

            if (Price != announce.Price || PricePerSquareMeter != announce.PricePerSquareMeter)
            {
                return false;
            }

            return City == announce.City &&
                   Rooms == announce.Rooms &&
                   Floor == announce.Floor &&
                   BuildingType == announce.BuildingType;
        }
    }
}