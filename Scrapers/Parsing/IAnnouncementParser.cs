using HtmlAgilityPack;
using Scrapers.Model;

namespace Scrapers.Parsing
{
    public interface IAnnouncementParser
    {
        /// <summary>
        /// Parse html contents of an offer
        /// </summary>
        /// <param name="html">Page HTML contents</param>
        /// <returns>Announcement</returns>
        Announcement ParseOffer(HtmlNode html);
    }
}