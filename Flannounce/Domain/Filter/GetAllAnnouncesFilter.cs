using System;
using Flannounce.Model.DAO.Enums;

namespace Flannounce.Domain.Filter
{
    public class GetAllAnnouncesFilter
    {
        public string City { get; set; }
        
        public string District { get; set; }
        
        public string Rooms { get; set; }
        
        public string Floor { get; set; }
         
        public BuildingType? BuildingType { get; set; }
        
        public bool? IncludesFurniture { get; set; }
        
        public bool? HasCoordinates { get; set; }

        public bool? WithImages { get; set; }

        public OfferedBy? OfferedBy { get; set; }
        
        public decimal? PriceMin { get; set; }
         
        public decimal? PriceMax { get; set; }
        
        public decimal? PricePerSquareMeterMin { get; set; }
         
        public decimal? PricePerSquareMeterMax { get; set; }
        
        public decimal? LivingSpaceMin { get; set; }
         
        public decimal? LivingSpaceMax { get; set; }
        
        public DateTime? CreatedAtMin { get; set; }
         
        public DateTime? CreatedAtMax { get; set; } 
    }
}