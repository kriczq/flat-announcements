using HtmlAgilityPack;
using Scrapers.Model;

namespace Scrapers.Parsing
{
    public interface IAnnouncementParser
    {
        /// <summary>
        /// Parse html contents of an offer
        /// </summary>
        /// <param name="url">Page url</param>
        /// <param name="html">Page HTML contents</param>
        /// <returns>Announcement</returns>
        Announcement ParseOffer(string url, HtmlNode html);
    }
}