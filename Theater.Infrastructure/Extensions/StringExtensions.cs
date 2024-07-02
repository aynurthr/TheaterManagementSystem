using System.Text.RegularExpressions;

namespace Theater.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }
        public static string TruncateHtml(this string input, int length)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = Regex.Replace(input, @"<[^>]*>", string.Empty);
            return result.Length <= length ? result : result.Substring(0, length) + "...";
        }


        public static string FixHtml(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = Regex.Replace(input, @"<[^>]*>", string.Empty);
            return result;
        }
    }
}
