using System.Globalization;
using Rocket.Interfaces;

namespace Rocket.Localization
{
    public class CultureSetter(
        IAuthenticationManager authenticationManager = null
    ) : ICultureSetter
    {
        private static readonly CultureInfo DefaultCulture =
            CultureInfo
                .GetCultureInfo(Languages.DefaultLanguage);

        public string DefaultLanguage => Languages.DefaultLanguage;

        public CultureInfo CurrentCulture => CultureInfo.CurrentUICulture;

        // This cannot be made async as it will fire off on the non-rendering thread
        public void SetCulture(
            string languageCode
        )
        {
            var culture =
                string
                    .IsNullOrEmpty(languageCode)
                    ? DefaultCulture
                    : CultureInfo.GetCultureInfo(languageCode);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            authenticationManager?
                .SetCurrentLanguageAsync(languageCode)
                .Wait();
        }
    }
}