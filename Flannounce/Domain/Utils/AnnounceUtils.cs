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
                filter &= filterBuilder.Eq(x => x.City, announceFilter.City);
            }

            if (!string.IsNullOrEmpty(announceFilter?.District))
            {
                filter &= filterBuilder.Eq(x => x.District, announceFilter.District);
            }
            
            if (!string.IsNullOrEmpty(announceFilter?.Floor))
            {
                filter &= filterBuilder.Eq(x => x.Floor, announceFilter.Floor);
            }

            if (announceFilter?.BuildingType != null)
            {
                filter &= filterBuilder.Eq(x => x.BuildingType, announceFilter.BuildingType);
            }

            if (announceFilter?.IncludesFurniture != null)
            {
                filter &= filterBuilder.Eq(x => x.IncludesFurniture, (bool) announceFilter.IncludesFurniture);
            }   
            
            if (announceFilter?.OfferedBy != null)
            {
                filter &= filterBuilder.Eq(x => x.OfferedBy, announceFilter.OfferedBy);
            }

            if (!string.IsNullOrEmpty(announceFilter?.Rooms))
            {
                filter &= filterBuilder.Eq(x => x.Rooms, announceFilter.Rooms);
            }
            if (announceFilter?.PriceMax != null)
            {
                filter &= filterBuilder.Lte(x => x.Price, announceFilter.PriceMax);
            }     
            
            if (announceFilter?.PriceMin != null)
            {
                filter &= filterBuilder.Gte(x => x.Price, announceFilter.PriceMin);
            }          
            
            if (announceFilter?.PricePerSquareMeterMax != null)
            {
                filter &= filterBuilder.Lte(x => x.PricePerSquareMeter, announceFilter.PricePerSquareMeterMax);
            }     
            
            if (announceFilter?.PricePerSquareMeterMin != null)
            {
                filter &= filterBuilder.Gte(x => x.PricePerSquareMeter, announceFilter.PricePerSquareMeterMin);
            } 
            
            if (announceFilter?.LivingSpaceMax != null)
            {
                filter &= filterBuilder.Lte(x => x.LivingSpace, announceFilter.LivingSpaceMax);
            }     
            
            if (announceFilter?.LivingSpaceMin != null)
            {
                filter &= filterBuilder.Gte(x => x.LivingSpace, announceFilter.LivingSpaceMin);
            } 
            
            if (announceFilter?.CreatedAtMax != null)
            {
                filter &= filterBuilder.Lte(x => x.CreatedAt, announceFilter.CreatedAtMax);
            }     
            
            if (announceFilter?.CreatedAtMin != null)
            {
                filter &= filterBuilder.Gte(x => x.CreatedAt, announceFilter.CreatedAtMin);
            } 
                        
            if (announceFilter?.WithImages != null)
            {
                if ((bool)announceFilter.WithImages)
                {
                    filter &= filterBuilder.Exists(x => x.Images);
                }
                else
                {
                    filter &= filterBuilder.Eq(x => x.Images,null);
                }
            } 
            
            if (announceFilter?.HasCoordinates != null)
            {
                if ((bool) announceFilter?.HasCoordinates)
                {
                    filter &= filterBuilder.Exists(x => x.Latitude);
                    filter &= filterBuilder.Exists(x => x.Longitude);   
                }
                else
                {
                    filter &= filterBuilder.Eq(x => x.Latitude,null);
                    filter &= filterBuilder.Eq(x => x.Longitude,null);   
                }
            }

            return queryable.Find(filter);
        }

        public static IEnumerable<AveragePricePerCity> ConvertToAveragePricePerCities(this List<Announce> announces,bool sortAsc)
        {
            var avgPricesPerCity = announces?.GroupBy(
                    a => a.City,
                    a => a.PricePerSquareMeter)
                .Select(ac => new AveragePricePerCity()
                {
                    City = ac.Key,
                    AveragePrice = ac.Average() ?? 0
                })
                .Sort(sortAsc);

            return avgPricesPerCity;
        }

        private static IOrderedEnumerable<AveragePricePerCity> Sort(this IEnumerable<AveragePricePerCity> avgPricePerCities,
            bool sortAsc)
        {
            return sortAsc ? avgPricePerCities.OrderBy(x => x.AveragePrice) : avgPricePerCities.OrderByDescending(x => x.AveragePrice);
        }
    }
}