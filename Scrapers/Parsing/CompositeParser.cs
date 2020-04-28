using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Scrapers.Model;

namespace Scrapers.Parsing
{
    /// <summary>
    /// Parser that should be used to compose multiple parsers into one.
    /// E. g. OLX includes offers from Otodom, so CompositeParser can be created to match
    /// urls from olx to the OlxParser and urls from otodom to OtodomParser
    /// </summary>
    public class CompositeParser : IAnnouncementParser
    {
        /// <summary>
        /// Dictionary where key is the rule when parser will be applied
        /// and the value is actual parser to be used
        /// </summary>
        public readonly Dictionary<string, IAnnouncementParser> Parsers = new Dictionary<string, IAnnouncementParser>();
        
        /// <inheritdoc cref="IAnnouncementParser.ParseOffer" />
        public Announcement ParseOffer(string url, HtmlNode html)
        {
            foreach (var rule in Parsers.Keys.Where(url.StartsWith))
            {
                return Parsers[rule].ParseOffer(url, html);
            }

            throw new ArgumentException($"Url {url} doesn't match any of rules");
        }
    }
}