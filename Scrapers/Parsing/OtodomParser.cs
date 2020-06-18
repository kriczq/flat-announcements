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
            var imagesListNode = html.CssSelect(".css-d8fgxy .slick-track img").ToList();
            
            // Text
            var details = ParseDetails(detailsNodes);

            var priceText = GetPriceText(priceNode);
            var rentText = GetRentText(details);
            var pricePsmText = GetPricePsmText(pricePsmNode);

            var livingSpaceText = details["Powierzchnia"].RemoveWhitespace().TrimFromEnd(2);

            var (voivodeship, city, district, street) = ParseLocation(breadcrumbsNode);
            var images = imagesListNode
                .Select(node =>
                {
                    var src = node.GetAttributeValue("src");
                    var cleanUrlParts = src.Split(";s=");        // Remove image resizing
                    return cleanUrlParts[0];
                })
                .ToList();

            // Values
            var id = idNode.InnerText.Split(": ")[1].Split("Nr")[0];
            var title = titleNode.InnerText.Trim();

            var basePrice = priceText.ToFloatWithPolishCulture();
            var rent = rentText.ToFloatWithPolishCulture();
            var pricePsm = pricePsmText.ToFloatWithPolishCulture();

            var isFromDeveloper = details.ContainsKey("Rynek") &&  details["Rynek"] != "Wtórny";
            var includesFurniture = details.ContainsKey("Stan wykończenia") && details["Stan wykończenia"] == "do zamieszkania";
            var livingSpace = livingSpaceText.ToFloatWithPolishCulture();
            var buildingType = details.ContainsKey("Rodzaj zabudowy") ? details["Rodzaj zabudowy"] : "";
            var rooms = details.ContainsKey("Liczba pokoi") ? details["Liczba pokoi"] : "0";
            var floor = details.ContainsKey("Piętro") ? details["Piętro"] : "Nieznane";

            var createdAt = ParseCreatedAt(html.InnerText);
            
            return new Announcement
            {
                Id = id,
                Title = title,
                Images = images,
                
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

        /// <summary>
        /// Returns rent text or empty string if data not present
        /// </summary>
        /// <param name="details">Details dictionary</param>
        private static string GetRentText(IReadOnlyDictionary<string, string> details)
        {
            if (details.ContainsKey("Czynsz"))
                return details["Czynsz"].RemoveWhitespace().TrimFromEnd(2);

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (details.ContainsKey("Czynsz - dodatkowo"))
                return details["Czynsz - dodatkowo"].RemoveWhitespace().TrimFromEnd(2);

            return "0";
        }

        /// <summary>
        /// Parse price node and return price if present
        /// </summary>
        /// <param name="node">Price HTML node</param>
        private static string GetPriceText(HtmlNode node)
        {
            if (node == null)
                return "0";

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (node.InnerText.Contains("miesiąc"))
                return node.InnerText.RemoveWhitespace().TrimFromEnd(10);

            return node.InnerText.RemoveWhitespace().TrimFromEnd(2);
        }

        /// <summary>
        /// Parse prise per square meter node and return price if present
        /// </summary>
        /// <param name="node">Price per square meter node</param>
        private static string GetPricePsmText(HtmlNode node)
        {
            return node == null || node.InnerText == ""
                ? "0" 
                : node.InnerText.RemoveWhitespace().TrimFromEnd(5);
        }
    }
}