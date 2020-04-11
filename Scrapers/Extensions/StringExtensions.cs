using System.Linq;

namespace Scrapers.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Remove all whitespace characters from string
        /// </summary>
        public static string RemoveWhitespace(this string str)
        {
            return new string(str.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        /// <summary>
        /// Trim characters from end of string
        /// </summary>
        public static string TrimFromEnd(this string str, int length)
        {
            return str.Length <= length 
                ? string.Empty 
                : str.Substring(0, str.Length - length);
        }
    }
}