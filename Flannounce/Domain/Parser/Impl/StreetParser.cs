using System;
using System.Collections.Generic;
using System.Linq;
using Flannounce.Domain.Parser.Utils;
using Flannounce.Domain.Services.Implementation;
using Flannounce.Model.DAO;

namespace Flannounce.Domain.Parser
{
    public class StreetParser : IStreetParser
    {
        private readonly IStreetService _streetService;
        private Lazy<IDictionary<string, List<StreetToCompare>>> _cityToStreets;

        public StreetParser(IStreetService streetService)
        {
            _streetService = streetService;
            _cityToStreets = new Lazy<IDictionary<string, List<StreetToCompare>>>(ConvertToDictionary(_streetService.Get()));
        }

        private static IDictionary<string, List<StreetToCompare>> ConvertToDictionary(List<Street> streets)
        {
            var cityToStreets = new Dictionary<string, List<StreetToCompare>>();
            var lookup = streets.ToLookup(s => s.City);

            foreach (var city in streets.Select(s => s.City).Distinct())
            {
                cityToStreets[city] = lookup[city]
                    .Select(s =>new StreetToCompare(s)).ToList();
            }

            return cityToStreets;
        }

        public IEnumerable<Announce> ParseStreet(IEnumerable<Announce> announces)
        {
            foreach (var announce in announces)
            {
                ParseStreetFromTitle(announce);

                yield return announce;
            }
        }

        private void ParseStreetFromTitle(Announce announce)
        {
            if (!_cityToStreets.Value.ContainsKey(announce.City)) return;
            var streets = _cityToStreets.Value[announce.City];
            var title = announce.Title.ToLower().CleanCharacters();
            foreach (var street in streets.Where(street => title.Contains(street.CleanedName)))
            {
                announce.Street = street.Name;
                break;
            }
        }
    }
}