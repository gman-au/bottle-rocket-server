using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class SafeFileRetitler : IFileRetitler
    {
        private const int MaxFileNameLength = 150;
        private static readonly char[] InvalidChars = Path.GetInvalidFileNameChars();

        private static readonly char[] AdditionalBannedChars = ['#', '%', '&', '{', '}', '~', '`'];

        public string Retitle(string rawTextData)
        {
            if (string.IsNullOrWhiteSpace(rawTextData))
                return null;

            var titleLine =
                rawTextData
                    .Split(
                        ['\n', '\r'],
                        StringSplitOptions.RemoveEmptyEntries
                    )
                    .FirstOrDefault()
                    ?.Trim();

            if (string.IsNullOrWhiteSpace(titleLine))
                return null;

            var allBanned =
                InvalidChars
                    .Concat(AdditionalBannedChars)
                    .ToHashSet();

            var sanitised =
                new string(
                    titleLine
                        .Where(c => !allBanned.Contains(c))
                        .ToArray()
                );

            sanitised =
                Regex
                    .Replace(
                        sanitised,
                        @"[\s_]+",
                        " "
                    )
                    .Trim();

            // Windows reserved names (CON, PRN, AUX, NUL, COM1–COM9, LPT1–LPT9)
            if (Regex.IsMatch(
                    sanitised,
                    @"^(CON|PRN|AUX|NUL|COM\d|LPT\d)$",
                    RegexOptions.IgnoreCase
                ))
                return null;

            sanitised =
                sanitised
                    .TrimEnd(
                        '.',
                        ' '
                    );

            if (string.IsNullOrWhiteSpace(sanitised))
                return null;

            // Enforce length cap
            if (sanitised.Length > MaxFileNameLength)
                sanitised =
                    sanitised[..MaxFileNameLength]
                        .TrimEnd();

            return sanitised;
        }
    }
}