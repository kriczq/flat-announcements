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
    /// Parser used to parse otodom.pl announcements.
    /// We're not parsing otodom.pl directly right now but offers
    /// from otodom.pl appears on olx.pl so parser is required.
    /// </summary>
    public class OtodomParser : IAnnouncementParser
    {
        /// <inheritdoc cref="IAnnouncementParser.ParseOffer" />
        public Announcement ParseOffer(string url, HtmlNode html)
        {
            // Nodes
            var idNode = html.CssSelect(".css-kos6vh").First();
            var titleNode = html.CssSelect("h1.css-1ld8fwi").First();
            var breadcrumbsNode = html.CssSelect("ul.breadcrumb").First();
            var priceNode = html.CssSelect(".css-1vr19r7").First();
            var pricePsmNode = html.CssSelect(".css-zdpt2t").First();
            var detailsNodes = html.CssSelect(".css-1ci0qpi li");
            
            // Text
            var details = ParseDetails(detailsNodes);

            var priceText = priceNode.InnerText.RemoveWhitespace().TrimFromEnd(2);
            var rentText = details["Czynsz"].RemoveWhitespace().TrimFromEnd(2);
            var pricePsmText = pricePsmNode.InnerText.RemoveWhitespace().TrimFromEnd(5);

            var livingSpaceText = details["Powierzchnia"].RemoveWhitespace().TrimFromEnd(2);

            var (voivodeship, city, district, street) = ParseLocation(breadcrumbsNode);

            // Values
            var id = idNode.InnerText.Split(": ")[1].Split("Nr")[0];
            var title = titleNode.InnerText.Trim();

            var basePrice = int.Parse(priceText);
            var rent = float.Parse(rentText);
            var pricePsm = float.Parse(pricePsmText);

            var isFromDeveloper = details["Rynek"] != "Wtórny";
            var includesFurniture = details["Stan wykończenia"] == "do zamieszkania";
            var livingSpace = float.Parse(livingSpaceText);
            var buildingType = details["Rodzaj zabudowy"];
            var rooms = details["Liczba pokoi"];
            var floor = details["Piętro"];

            var createdAt = ParseCreatedAt(html.InnerText);
            
            return new Announcement
            {
                Id = id,
                Title = title,
                
                Voivodeship = voivodeship,
                City = city,
                District = district,
                Street = street,
                
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
        /// Parse location string that olx provides to tuple consisting of four elements:
        /// If data is not present empty string is provided
        /// 0 - Voivodeship
        /// 1 - City
        /// 2 - District
        /// 3 - Street
        /// </summary>
        /// <param name="breadcrumbs">Breadcrumbs node</param>
        /// <returns></returns>
        private static Tuple<string, string, string, string> ParseLocation(HtmlNode breadcrumbs)
        {
            var breadcrumbsList = breadcrumbs.CssSelect("li");
            var texts = breadcrumbsList.Select(breadcrumb => breadcrumb.InnerText.Trim())
                .ToArray();

            return texts.Length switch
            {
                6 => new Tuple<string, string, string, string>(texts[2], texts[3], texts[4], texts[5]),
                5 => new Tuple<string, string, string, string>(texts[2], texts[3], texts[4], string.Empty),
                4 => new Tuple<string, string, string, string>(texts[2], texts[3], string.Empty, string.Empty),
                3 => new Tuple<string, string, string, string>(texts[2], string.Empty, string.Empty, string.Empty),
                _ => new Tuple<string, string, string, string>(string.Empty, string.Empty, string.Empty, string.Empty)
            };
        }

        /// <summary>
        /// Parse tabular details data that otodom provides to simple dictionary
        /// </summary>
        /// <param name="detailsNodes">Nodes containing details</param>
        /// <returns>Dictionary containing details</returns>
        private static Dictionary<string, string> ParseDetails(IEnumerable<HtmlNode> detailsNodes)
        {
            return detailsNodes.Select(node => node.InnerText.Split(": "))
                .ToDictionary(data => data[0], data => data[1]);
        }

        /// <summary>
        /// Parse creation date from JSON available in source code
        /// </summary>
        /// <param name="html">Site source</param>
        /// <returns>Announcement creation date</returns>
        private static DateTime ParseCreatedAt(string html)
        {
            const string pattern = @"""dateCreated"":""([0-9- :]+)""";
            var match = Regex.Match(html, pattern);
            var date = match.Groups[1].Value;
            return Convert.ToDateTime(date, new CultureInfo("pl"));
        }
    }
}