using System.Text.RegularExpressions;

namespace Rocket.Replicate.Infrastructure
{
    public class MarkdownStripper : IMarkdownStripper
    {
        private static readonly Regex IconRegex =
            new(
                @"!\[[^\]]*\]\([^)]*\)\s+[^!]*?(?:icon|qr code)\b",
                RegexOptions.IgnoreCase | RegexOptions.Compiled
            );

        public string StripFooter(string markdown)
        {
            var result = markdown;

            result =
                IconRegex
                    .Replace(
                        result,
                        string.Empty
                    );

            return result;
        }
    }
}