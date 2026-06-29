using System.Globalization;

namespace Rocket.Interfaces
{
    public interface ICultureSetter
    {
        string DefaultLanguage { get; }
        
        CultureInfo CurrentCulture { get; }

        void SetCulture(
            string languageCode
        );
    }
}