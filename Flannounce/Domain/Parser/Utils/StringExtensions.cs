using System.Collections.Generic;
using System.Text;

namespace Flannounce.Domain.Parser.Utils
{
    public static class StringExtensions
    {
        private static readonly Dictionary<char, char> PolishCharToCorrectChar = new Dictionary<char, char>(new[]
        {
            new KeyValuePair<char, char>('ń', 'n'),
            new KeyValuePair<char, char>('ą', 'a'),
            new KeyValuePair<char, char>('ó', 'o'),
            new KeyValuePair<char, char>('ę', 'e'),
            new KeyValuePair<char, char>('ć', 'c'),
            new KeyValuePair<char, char>('ś', 's'),
            new KeyValuePair<char, char>('ź', 'z'),
            new KeyValuePair<char, char>('ż', 'z'),
            new KeyValuePair<char, char>('ł', 'l'),
        });


        public static string CleanCharacters(this string text)
        {
            return text.ToLower().RemovePolishDiacritics();
        }
        
        private static string RemovePolishDiacritics(this string text)
        {
            var newText = new StringBuilder();

            foreach (var character in text)
            {
                newText.Append(PolishCharToCorrectChar.ContainsKey(character)
                    ? PolishCharToCorrectChar[character]
                    : character);
            }

            return newText.ToString();
        }
    }
}