using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flannounce.Domain.Filter;
using Flannounce.Model.DAO;
using MongoDB.Driver;

namespace Flannounce.Domain.Utils
{
    public static class AnnounceUtils
    {
        public static  IFindFluent<Announce, Announce> AddFiltersOnQuery(this IMongoCollection<Announce> queryable, GetAllAnnouncesFilter announceFilter )
        {
            var filterBuilder = Builders<Announce>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(announceFilter?.City))
            {
                filter = filter & filterBuilder.Eq(x => x.City, announceFilter.City);
            }

            if (!string.IsNullOrEmpty(announceFilter?.District))
            {
                filter = filter & filterBuilder.Eq(x => x.District, announceFilter.District);
            }
            
            if (!string.IsNullOrEmpty(announceFilter?.Floor))
            {
                filter = filter & filterBuilder.Eq(x => x.Floor, announceFilter.Floor);
            }

            if (announceFilter?.BuildingType != null)
            {
                filter = filter & filterBuilder.Eq(x => x.BuildingType, announceFilter.BuildingType);
            }

            if (announceFilter?.IncludesFurniture != null)
            {
                filter = filter & filterBuilder.Eq(x => x.IncludesFurniture, (bool) announceFilter.IncludesFurniture);
            }   
            
            if (announceFilter?.OfferedBy != null)
            {
                filter = filter & filterBuilder.Eq(x => x.OfferedBy, announceFilter.OfferedBy);
            }

            if (!string.IsNullOrEmpty(announceFilter?.Rooms))
            {
                filter = filter & filterBuilder.Eq(x => x.Rooms, announceFilter.Rooms);
            }
            if (announceFilter?.PriceMax != null)
            {
                filter = filter & filterBuilder.Lte(x => x.Price, announceFilter.PriceMax);
            }     
            
            if (announceFilter?.PriceMin != null)
            {
                filter = filter & filterBuilder.Gte(x => x.Price, announceFilter.PriceMin);
            }          
            
            if (announceFilter?.PricePerSquareMeterMax != null)
            {
                filter = filter & filterBuilder.Lte(x => x.PricePerSquareMeter, announceFilter.PricePerSquareMeterMax);
            }     
            
            if (announceFilter?.PricePerSquareMeterMin != null)
            {
                filter = filter & filterBuilder.Gte(x => x.PricePerSquareMeter, announceFilter.PricePerSquareMeterMin);
            } 
            
            if (announceFilter?.LivingSpaceMax != null)
            {
                filter = filter & filterBuilder.Lte(x => x.LivingSpace, announceFilter.LivingSpaceMax);
            }     
            
            if (announceFilter?.LivingSpaceMin != null)
            {
                filter = filter & filterBuilder.Gte(x => x.LivingSpace, announceFilter.LivingSpaceMin);
            } 
            
            if (announceFilter?.CreatedAtMax != null)
            {
                filter = filter & filterBuilder.Lte(x => x.CreatedAt, announceFilter.CreatedAtMax);
            }     
            
            if (announceFilter?.CreatedAtMin != null)
            {
                filter = filter & filterBuilder.Gte(x => x.CreatedAt, announceFilter.CreatedAtMin);
            } 
                        
            if (announceFilter?.WithImages ?? false)
            {
                filter = filter & filterBuilder.Exists(x => x.Images);
            } 
            
            return queryable.Find(filter);
        }

        public static IEnumerable<AveragePricePerCity> ConvertToAveragePricePerCities(this List<Announce> announces)
        {
            var avgPricesPerCity = announces?.GroupBy(
                    a => a.City,
                    a => a.PricePerSquareMeter)
                .Select(ac => new AveragePricePerCity()
                {
                    City = ac.Key,
                    AveragePrice = ac.Average() ?? 0
                });
            
            return avgPricesPerCity;
        }

    }
}