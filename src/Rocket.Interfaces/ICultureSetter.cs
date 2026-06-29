using System.Globalization;

namespace Rocket.Interfaces
{
    public interface ICultureSetter
    {
        CultureInfo CurrentCulture { get; }

        void SetCulture(
            string languageCode
        );
    }
}