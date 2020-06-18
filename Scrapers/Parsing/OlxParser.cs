using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Scrapers.Extensions;
using Scrapers.Model;
using ScrapySharp.Extensions;

namespace Scrapers.Parsing
{
    /// <summary>
    /// Parser used to parse olx.pl announcements
    /// </summary>
    public class OlxParser : IAnnouncementParser
    {
        /// <inheritdoc cref="IAnnouncementParser.ParseOffer" />
        public Announcement ParseOffer(string uri, HtmlNode html)
        {
            // Nodes
            var titleboxNode = html.CssSelect(".offer-titlebox").First();
            var titleNode = titleboxNode.CssSelect("h1").First();
            var bottomBarItemNodes = html.CssSelect(".offer-bottombar .offer-bottombar__item").ToList();
            var idNode = bottomBarItemNodes[2].CssSelect("strong").First();
            var priceNode = titleboxNode.CssSelect(".pricelabel__value").First();
            var createdAtNode = bottomBarItemNodes[0];
            var locationNode = html.CssSelect(".offer-user__address address").First();
            var detailsNodes = html.CssSelect(".offer-details .offer-details__item");
            var imagesListNode = html.CssSelect("#descGallery li a").ToList();

            // Text
            var details = ParseDetails(detailsNodes);
            var priceText = priceNode.InnerText.RemoveWhitespace().TrimFromEnd(2);
            var livingSpaceText = details["Powierzchnia"].RemoveWhitespace().TrimFromEnd(2);
            var locationText = locationNode.InnerText.Trim();
            
            var pricePsmText = details.ContainsKey("Cena za m²") 
                ? details["Cena za m²"].RemoveWhitespace().TrimFromEnd(5)
                : "0";
            var rentText = details.ContainsKey("Czynsz (dodatkowo)") 
                ? details["Czynsz (dodatkowo)"].RemoveWhitespace().TrimFromEnd(2)
                : "0";

            // Values
            var id = idNode.InnerText.Trim();
            var title = titleNode.InnerText.Trim();
            var (voivodeship, city, district) = ParseLocation(locationText);
            var images = imagesListNode
                .Select(node =>
                {
                    var href = node.GetAttributeValue("href");
                    var cleanUrlParts = href.Split(";s=");        // Remove image resizing
                    return cleanUrlParts[0];
                })
                .ToList();

            var basePrice = priceText.ToFloatWithPolishCulture();
            var rent = rentText.ToFloatWithPolishCulture();
            var pricePsm = pricePsmText.ToFloatWithPolishCulture();

            var isFromDeveloper = details["Oferta od"].Contains("Deweloper");
            var includesFurniture = details["Umeblowane"] == "Tak";
            var livingSpace = livingSpaceText.ToFloatWithPolishCulture();
            var buildingType = details.ContainsKey("Rodzaj zabudowy") ? details["Rodzaj zabudowy"] : null;
            var rooms = details.ContainsKey("Liczba pokoi") ? details["Liczba pokoi"] : null;
            var floor = details.ContainsKey("Poziom") ? details["Poziom"] : null;

            var createdAt = ParseCreatedAt(createdAtNode.InnerText);
            
            return new Announcement
            {
                Id = id,
                Title = title,
                Images = images,
                
                Voivodeship = voivodeship,
                City = city,
                District = district,
                // Street = ,
                
                BasePrice = basePrice,
                Rent = rent,
                PricePerSquareMeter = pricePsm,
                
                IsFromDeveloper = isFromDeveloper,
                IncludesFurniture = includesFurniture,
                LivingSpace = livingSpace,
                BuildingType = buildingType,
                Rooms = rooms,
                Floor = floor,
                
                CreatedAt = createdAt
            };
        }
        
        /// <summary>
        /// Parse tabular details data that olx provides to simple dictionary
        /// </summary>
        /// <param name="detailsNodes">Nodes containing details</param>
        /// <returns>Dictionary containing details</returns>
        private static Dictionary<string, string> ParseDetails(IEnumerable<HtmlNode> detailsNodes)
        {
            var details = new Dictionary<string, string>();

            foreach (var node in detailsNodes)
            {
                var param = node.CssSelect(".offer-details__name").First().InnerText.Trim();
                var value = node.CssSelect(".offer-details__value").First().InnerText.Trim();
                details.Add(param, value);
            }

            return details;
        }

        /// <summary>
        /// Parse location string that olx provides to tuple consisting of three elements:
        /// 0 - Voivodeship
        /// 1 - City
        /// 2 - District (if present) or empty string
        /// </summary>
        /// <param name="locationString"></param>
        /// <returns></returns>
        private static Tuple<string, string, string> ParseLocation(string locationString)
        {
            var parts = locationString.Split(", ");

            return parts.Length switch
            {
                3 => new Tuple<string, string, string>(parts[1], parts[0], parts[2]),
                2 => new Tuple<string, string, string>(parts[1], parts[0], string.Empty),
                _ => new Tuple<string, string, string>(string.Empty, parts[0], string.Empty)
            };
        }

        /// <summary>
        /// Parse titlebox string that olx provides and extract announcement creation date
        /// </summary>
        /// <param name="titleboxString">Titlebox inner text</param>
        /// <returns>Announcement creation date</returns>
        private static DateTime ParseCreatedAt(string titleboxString)
        {
            const string pattern = @"(Dodane o (.+))|(Dodane z telefonu o (.+))";
            var match = Regex.Match(titleboxString.CleanInnerText(), pattern, RegexOptions.Multiline);
            var date = match.Groups[2].Value == "" ? match.Groups[4].Value : match.Groups[2].Value;
            return Convert.ToDateTime(date, new CultureInfo("pl"));
        }
    }
}