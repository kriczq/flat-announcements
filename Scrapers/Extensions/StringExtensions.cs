﻿using System.Linq;

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
    }
}