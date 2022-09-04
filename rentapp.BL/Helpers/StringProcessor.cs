using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace rentapp.BL.Helpers
{
    public class StringProcessor
    {
        private static Dictionary<string, string> searchCharactersToCleanUps = new Dictionary<string, string>()
        {
            {"á", "a" },
            {"é", "e" },
            {"í", "i" },
            {"ó", "o" },
            {"ú", "u" },
            {"ñ", "n" },
            {"ä", "a" },
            {"ë", "e" },
            {"ï", "i" },
            {"ö", "o"},
            {"ü", "u"},
            {"ÿ", "y" },
            {"'", string.Empty  },
            { "`", string.Empty },
            { ".", string.Empty },
            { ",", string.Empty },
            { "-", string.Empty },
            { "’", string.Empty }
        };

        private static Regex regexSpecialCharacters = new Regex("^[a-zA-Z0-9 ]*$");

        public static string CleanAsNumber(string value)
        {
            if (value == null)
                value = "0";

            value = value.Replace("$", string.Empty);
            value = value.Replace("U$S", string.Empty);
            value = value.Replace("$", string.Empty);
            value = value.Replace("-", string.Empty);
            value = value.Trim();

            if (value.Length == 1 && !(value[0] >= (int)'0' && value[0] <= (int)'9'))
                value = "0";

            return value;
        }

        public static string EmptyIfNull(string value)
        {
            if (value == null)
                value = string.Empty;

            return value;
        }

        public static string ClearAsPhone(string phone)
        {
            string plainNumber = phone.Replace(",", string.Empty).Replace(".", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);

            return plainNumber;
        }

        public static string TruncateIfNeeded(string value, int maxLength)
        {
            if (value != null)
            {
                if (value.Length > maxLength)
                {
                    value = value.Substring(0, maxLength);
                }
            }

            return value;
        }

        public static List<string> SplitTextInLinesConsideringWords(string text, int maxLengthPerLine)
        {
            List<string> lines = new List<string>();

            string[] addressWords = text.Trim().Split(' ');
            string currentLine = string.Empty;
            foreach (string addressWord in addressWords)
            {
                if (currentLine.Length + addressWord.Length < maxLengthPerLine)
                {
                    if (currentLine == string.Empty)
                    {
                        currentLine = addressWord;
                    }
                    else
                    {
                        currentLine += " " + addressWord;
                    }
                }
                else
                {
                    if (currentLine == string.Empty)
                    {
                        lines.Add(addressWord.Substring(0, maxLengthPerLine));
                        currentLine = addressWord.Substring(maxLengthPerLine);
                    }
                    else
                    {
                        lines.Add(currentLine);
                        currentLine = addressWord;
                    }
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine);
            }

            return lines;
        }

        public static string FormatAsCUIT(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length != 11)
            {
                return text;
            }

            return text.Insert(2, "-").Insert(11, "-");
        }

        public static bool EqualsCaseInsensitive(string text, string toCompare)
        {
            if (text == null || toCompare == null)
            {
                return false;
            }

            return text.Equals(toCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool ContainsCaseInsensitive(string text, string toContain)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(text, toContain, CompareOptions.IgnoreCase) >= 0;
        }

        public static string CleanForSearch(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.ToLower();

            if (text.Any(p => searchCharactersToCleanUps.Keys.Any(q => q == p.ToString())))
            {
                foreach (var character in searchCharactersToCleanUps.Keys)
                {
                    text = text.Replace(character, searchCharactersToCleanUps[character]);
                }
            }

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            text = regex.Replace(text, " ");

            return text;
        }

        public static bool ValidateIfHasSpecialCharacters(string text)
        {
            if (!regexSpecialCharacters.IsMatch(text) || text.Contains(" "))
            {
                return false;
            }
            return true;
        }
    }
}
