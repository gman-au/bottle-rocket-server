using System.Collections.Generic;

namespace Rocket.Web.Host.Infrastructure
{
    internal static class Languages
    {
        public const string DefaultLanguage = "en-US";

        public static readonly Dictionary<string, string> AvailableLanguages = new()
        {
            { "English", "en-US" },
            { "French", "fr-FR" }
        };
    }
}