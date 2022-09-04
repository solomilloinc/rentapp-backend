using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace rentap.backend.Core.Helpers
{
    public static class StringHelper
    {
        private const string HTML_BREAK_TAG = "<br />";
        public const string REGEX_FOR_HTML_TAGS = "<.*?>";
        private static Regex emailRegex = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
        private static Regex ipRegex = new Regex("(\\:[0-9]*)");
        private static Regex amountRegex = new Regex("(\\$\\d)|(\\d+\\$)");
        private static Regex numberedListRegex = new Regex("^\\d{1,2}\\.+");
        private static Regex linkSyntaxRegex = new Regex(@"#link.+#");
        private static Regex multipleWhiteSpaceReplacedByOneWhiteSpaceRegex = new Regex(@"\s+");
        private const string HTML_UL_OPENING_TAG = "<ul>";
        private const string HTML_UL_CLOSING_TAG = "</ul>";
        private const string HTML_OL_OPENING_TAG = "<ol>";
        private const string HTML_OL_CLOSING_TAG = "</ol>";
        private const string HTML_LI_OPENING_TAG = "<li>";
        private const string HTML_LI_CLOSING_TAG = "</li>";
        private static readonly Regex urlFileNameCleanupRegex = new Regex(@"[^a-z0-9]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static string ELLIPSIS = "...";

        public static string ExtractValueFromJsonString(string json, string propertyName)
        {
            int propertyIndex = json.IndexOf("\"" + propertyName + "\"", StringComparison.InvariantCultureIgnoreCase);
            if (propertyIndex >= 0)
            {
                int valueIndex = json.IndexOf("\"", propertyIndex + propertyName.Length + 2);
                if (valueIndex > 0)
                {
                    int lastValueIndex = json.IndexOf("\"", valueIndex + 1);
                    if (lastValueIndex > 0)
                    {
                        return json.Substring(valueIndex + 1, lastValueIndex - valueIndex - 1);
                    }
                }
            }

            return null;
        }

        public static string AddHttpIfNecessary(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return url;
            }

            if (!url.StartsWithCaseInsensitive("http"))
            {
                url = $"http://{url}";
            }

            return url;
        }

        public static string BuildCampaignCode(int domainId, string campaignName)
        {
            return $"{domainId}-{campaignName}";
        }

        public static string BuildLink(string url, string text, string title)
        {
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                title = text;
            }

            return $"<a href='{StringHelper.AddHttpIfNecessary(url.Trim())}' target='_blank' title='{title.Trim()}'>{text.Trim()}</a>";
        }

        public static string ConvertLinkDescriptionIntoHtmlAnchor(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || !linkSyntaxRegex.IsMatch(text))
            {
                return text;
            }

            text = linkSyntaxRegex.Replace(text, p =>
            {
                string value = p.Value;
                int startIndex = value.IndexOf("(") + 1;
                int endIndex = value.LastIndexOf(")");
                var parts = value.Substring(startIndex, endIndex - startIndex).Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3)
                {
                    return BuildLink(parts[0], parts[1], parts[2]);
                }
                if (parts.Length == 2)
                {
                    return BuildLink(parts[0], parts[1], parts[1]);
                }

                return text;
            });

            return text;
        }

        public static string ReplaceMultipleOccurrencesOfCharacters(string inputText, char[] characters, int minOccurrenceCount, char replacementCharacter = ' ')
        {
            foreach (var character in characters)
            {
                if (inputText.Count(p => p == character) >= minOccurrenceCount)
                {
                    inputText = inputText.Replace(character, replacementCharacter);
                }
            }

            return inputText;
        }

        public static string ConvertStepsIntoHtmlLI(string text, bool useOrderedList = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var lines = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (lines.Count <= 1 || !lines.Any(p => numberedListRegex.IsMatch(p)))
            {
                return text;
            }

            StringBuilder sb = new StringBuilder();

            string listOpeningTag = useOrderedList ? HTML_OL_OPENING_TAG : HTML_UL_OPENING_TAG;
            string listClosingTag = useOrderedList ? HTML_OL_CLOSING_TAG : HTML_UL_CLOSING_TAG;

            bool isULOpened = false;
            foreach (var line in lines)
            {
                if (numberedListRegex.IsMatch(line))
                {
                    if (!isULOpened)
                    {
                        sb.Append(listOpeningTag);
                        isULOpened = true;
                    }
                    if (useOrderedList)
                    {
                        sb.Append($"{HTML_LI_OPENING_TAG}{numberedListRegex.Replace(line, string.Empty).Trim()}{HTML_LI_CLOSING_TAG}");
                    }
                    else
                    {
                        sb.Append($"{HTML_LI_OPENING_TAG}{line}{HTML_LI_CLOSING_TAG}");
                    }
                }
                else
                {
                    if (isULOpened)
                    {
                        sb.Append(listClosingTag);
                        isULOpened = false;
                    }
                    sb.AppendLine(line);
                }
            }

            if (isULOpened)
            {
                sb.Append(listClosingTag);
                isULOpened = false;
            }

            var result = sb.ToString();
            if (result.EndsWith(Environment.NewLine))
            {
                return result.Substring(0, result.Length - 2);
            }

            return result;
        }

        public static string ConvertToOnlyDigits(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char character in text)
            {
                if (char.IsDigit(character))
                {
                    sb.Append(character);
                }
            }

            return sb.ToString();
        }

        public static string ReplaceMultipleWhiteSpacesByOneWhiteSpace(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return multipleWhiteSpaceReplacedByOneWhiteSpaceRegex.Replace(text, " ");
        }

        public static string ExtractDomainName(string siteUrl)
        {

            if (Uri.TryCreate(siteUrl, UriKind.Absolute, out Uri uri))
            {
                return uri.Host.Replace("www.", string.Empty);
            }

            return siteUrl;
        }

        public static string CleanIPAddress(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip) || ip == "::1")
            {
                return ip;
            }

            ip = ipRegex.Replace(ip, string.Empty);

            return ip;
        }

        public static string TruncateIfLonger(string text, int maxLength, bool addPointsAtTheEnd = false)
        {
            string result;
            if (!string.IsNullOrEmpty(text) && text.Length > maxLength)
            {
                result = text.Substring(0, maxLength);

                if (addPointsAtTheEnd)
                {
                    result += "... ";
                }
            }
            else
            {
                result = text;
            }

            return result;
        }

        public static string CapitalizeFirstLetter(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                if (text == null)
                {
                    return null;
                }
                else
                {
                    return string.Empty;
                }
            }

            var finalText = Regex.Replace(text.ToLower(), " +", " ").Trim();

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(finalText);
        }

        public static string ConvertNewLineToHtmlBreak(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return text.Replace(Environment.NewLine, HTML_BREAK_TAG).Replace("\n", Environment.NewLine + HTML_BREAK_TAG);
        }

        public static string SplitStringIntoHtmlLines(string text, string separator)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return string.Join(HTML_BREAK_TAG, text.Split(separator, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string GetQueryStringValue(string url, string queryStringParameterName, bool removePlusSigns = true)
        {
            try
            {
                Uri uri = new Uri(url);

                var keywords = HttpUtility.ParseQueryString(uri.Query).Get(queryStringParameterName);
                if (removePlusSigns)
                {
                    keywords = keywords.Replace("+", string.Empty);
                }

                return keywords;
            }
            catch
            {
                return null;
            }
        }

        public static string MakeValidUrlFileName(string name)
        {
            var finalName = urlFileNameCleanupRegex.Replace(name, "-");
            if (string.IsNullOrWhiteSpace(finalName))
            {
                throw new Exception("File name has no basic characters");
            }

            return finalName;
        }

        public static string CleanHtmlCode(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return Regex.Replace(text, REGEX_FOR_HTML_TAGS, string.Empty).Replace("<", string.Empty).Replace(">", string.Empty);
        }

        public static string GetYesNoTextValue(bool? value, string nullValueText = "")
        {
            return !value.HasValue ? nullValueText : (value.Value ? "Yes" : "No");
        }

        public static string GetIntTextValue(int? value, string nullValueText = "")
        {
            return !value.HasValue ? nullValueText : value.Value.ToString();
        }

        public static string ReplaceCaseInsensitive(string originalString, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(originalString) || string.IsNullOrEmpty(oldValue))
            {
                return originalString;
            }

            if (newValue == null)
            {
                newValue = string.Empty;
            }

            return Regex.Replace(originalString,
                Regex.Escape(oldValue),
                Regex.Replace(newValue, "\\$[0-9]+", @"$$$0"),
                RegexOptions.IgnoreCase);
        }

        public static string ReplaceWords(string text, string search, string replacement, bool isCaseInsensitive = true)
        {
            if (isCaseInsensitive)
            {
                return Regex.Replace(text, $"\\b{search}\\b", replacement, RegexOptions.IgnoreCase);
            }

            return Regex.Replace(text, $"\\b{search}\\b", replacement);
        }

        public static string FormatJson(string json)
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        public static string KeepOnlyDigits(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return Regex.Replace(text, @"[^\d]", "");
        }

        public static string FormatAsMoney(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Contains("$"))
            {
                return text;
            }

            return "$" + text;
        }

        public static string CleanHttpProtocols(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return link;
            }

            return link.Replace("http://", string.Empty).Replace("https://", string.Empty);
        }

        public static string ExtractValue(string text, string startWord, string endWord, bool extractUntilTheEndIfEndWordIsNotFound = true, bool decodeUrl = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            int startWordPosition = text.IndexOf(startWord);
            if (startWordPosition <= 0)
            {
                return null;
            }
            startWordPosition += startWord.Length;

            int endWordPosition;
            if (!string.IsNullOrWhiteSpace(endWord))
            {
                endWordPosition = text.IndexOf(endWord, startWordPosition);
            }
            else
            {
                endWordPosition = -1;
            }

            string extractedValue;
            if (endWordPosition <= 0)
            {
                extractedValue = text.Substring(startWordPosition);
            }
            else
            {
                int wordLength = endWordPosition - startWordPosition;

                extractedValue = text.Substring(startWordPosition, wordLength);
            }

            if (decodeUrl)
            {
                extractedValue = DecodeUrl(extractedValue);
            }

            return extractedValue;
        }

        public static string DecodeUrl(string text)
        {
            return HttpUtility.UrlDecode(text);
        }

        public static bool ContainsWord(string text, string word, string prefix = null)
        {
            string pattern = $@"\b{word}\b";
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                pattern = string.Concat(prefix, pattern);
            }
            return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}, ", b);
            }

            return hex.ToString().TrimEnd(new[] { ' ', ',' });
        }

        private static Dictionary<string, string> searchCharactersToCleanUps = new Dictionary<string, string>()
        {
            {"á", "a" },
            {"é", "e" },
            {"í", "i" },
            {"ó", "o" },
            {"ú", "u" },
            {"ñ", "n" },
            {"'", string.Empty },
            { "`", string.Empty },
            { ".", string.Empty },
            { ",", string.Empty }
        };

        public static string CleanAsNumber(string value)
        {
            if (value == null)
                value = "0";

            value = value.Replace("$", "");
            value = value.Replace("U$S", "");
            value = value.Replace("$", "");
            value = value.Replace("-", "");
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

        public static string Clean(string value, string allowedCharacters)
        {
            return new string(value.Where(c => allowedCharacters.Contains(c)).ToArray());
        }

        public static string Truncate(string value, int length, bool addEllips)
        {
            if (value.Length > length)
            {
                if (length > 2 && addEllips)
                {
                    return value.Substring(0, length - 2) + ELLIPSIS;
                }
                return value.Substring(0, length);
            }

            return value;
        }

        public static string Truncate(string value, int length)
        {
            return Truncate(value, length, false);
        }

        public static string ClearAsPhone(string phone)
        {
            string plainNumber = phone.Replace(",", string.Empty).Replace(".", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);

            return plainNumber;
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

            return text;
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
    }
}
