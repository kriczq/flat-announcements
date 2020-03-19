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
    public class OlxParser : IAnnouncementParser
    {
        /// <inheritdoc cref="IAnnouncementParser.ParseOffer" />
        public OlxAnnouncement ParseOffer(HtmlNode html)
        {
            // Nodes
            var titleboxDetailsNode = html.CssSelect(".offer-titlebox__details > em").First();
            var idNode = titleboxDetailsNode.CssSelect("small").First();
            var titleNode = html.CssSelect(".offer-titlebox > h1").First();
            var locationNode = html.CssSelect(".offer-titlebox__details > .show-map-link > strong").First();
            var priceNode = html.CssSelect(".price-label > strong").First();
            var detailsNodes = html.CssSelect(".descriptioncontent > table.details .item");

            // Text
            var details = ParseDetails(detailsNodes);
            var priceText = priceNode.InnerText.RemoveWhitespace();
            priceText = priceText.Substring(0, priceText.Length - 2);
            var livingSpaceText = details["Powierzchnia"].RemoveWhitespace();
            livingSpaceText = livingSpaceText.Substring(0, livingSpaceText.Length - 2);
            
            var pricePsmText = details.ContainsKey("Cena za m²") 
                ? details["Cena za m²"].RemoveWhitespace()
                : "0";
            if (pricePsmText != "0")
                pricePsmText = pricePsmText.Substring(0, pricePsmText.Length - 6)
                    .Replace(".", ",");

            var rentText = details.ContainsKey("Czynsz (dodatkowo)") 
                ? details["Czynsz (dodatkowo)"].RemoveWhitespace()
                : "0";
            if (rentText != "0")
                rentText = rentText.Substring(0, rentText.Length - 2);

            // Values
            var id = idNode.InnerText.Trim().Split(":").Last();
            var title = titleNode.InnerText.Trim();
            var (voivodeship, city, district) = ParseLocation(locationNode.InnerText.Trim());

            var basePrice = int.Parse(priceText);
            var rent = float.Parse(rentText);
            var pricePsm = float.Parse(pricePsmText);

            var isFromDeveloper = details["Oferta od"].Contains("Deweloper");
            var includesFurniture = details["Umeblowane"] == "Tak";
            var livingSpace = float.Parse(livingSpaceText);
            var buildingType = details["Rodzaj zabudowy"];
            var rooms = details["Liczba pokoi"];
            var floor = details.ContainsKey("Poziom") ? details["Poziom"] : null;

            var createdAt = ParseCreatedAt(titleboxDetailsNode.InnerText);
            
            return new OlxAnnouncement
            {
                Id = id,
                Title = title,
                
                Voivodeship = voivodeship,
                City = city,
                District = district,
                
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
                var header = node.CssSelect("th").First().InnerText.Trim();
                var value = node.CssSelect(".value").First().InnerText.Trim();
                details.Add(header, value);
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
            const string pattern = @"(Dodane o (.+), ID)|(Dodane z telefonu o (.+),)";
            var match = Regex.Match(titleboxString.CleanInnerText(), pattern, RegexOptions.Multiline);
            var date = match.Groups[2].Value == "" ? match.Groups[4].Value : match.Groups[2].Value;
            return Convert.ToDateTime(date, new CultureInfo("pl"));
        }
    }
}