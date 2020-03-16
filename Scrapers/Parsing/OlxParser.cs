using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Scrapers.Extensions;
using Scrapers.Model;
using ScrapySharp.Extensions;

namespace Scrapers.Parsing
{
    public class OlxParser : IAnnouncementParser
    {
        /// <inheritdoc cref="IAnnouncementParser.ParseOffer" />
        public Announcement ParseOffer(HtmlNode html)
        {
            var id = html.CssSelect(".offer-titlebox__details > em > small").First().InnerText.Trim();
            var title = html.CssSelect(".offer-titlebox > h1").First().InnerText.Trim();
            var locationNode = html.CssSelect(".offer-titlebox__details > .show-map-link > strong").First();
            var price = html.CssSelect(".price-label > strong").First().InnerText.Trim();
            var details = ParseDetails(
                html.CssSelect(".descriptioncontent > table.details .item"));

            var livingSpace = details["Powierzchnia"]
                .RemoveWhitespace();
            livingSpace = livingSpace.Substring(0, livingSpace.Length - 2);

            return new Announcement
            {
                Id = id.Split(":").Last().Trim(),
                Title = title,
                
                Voivodeship = locationNode.InnerText.Trim(),
                City = locationNode.InnerText.Trim(),
                
                BasePrice = int.Parse(price.Substring(0, price.Length - 3).RemoveWhitespace()),
                
                Rent = details.ContainsKey("Czynsz (dodatkowo") ? 1 : 0,
                
                IsFromDeveloper = details["Oferta od"].Contains("Deweloper"),
                IncludesFurniture = details["Umeblowane"] == "Tak",
                LivingSpace = float.Parse(livingSpace),
                BuildingType = details["Rodzaj zabudowy"],
                Rooms = details["Liczba pokoi"],
                Floor = details["Poziom"],
                
                CreatedAt = ""
            };
        }
        
        private Dictionary<string, string> ParseDetails(IEnumerable<HtmlNode> detailsNodes)
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
    }
}