using System.Linq;
using Rocket.Domain.Utils;

namespace Rocket.Domain.Extensions
{
    public static class StringEx
    {
        public static bool IsErrorProperty(
            this string value
        ) => string.Equals(
            value,
            DomainConstants.ErrorInProperty
        );

        public static bool IsErrorProperty(
            this string[] values
        ) => values.Any(o => o.IsErrorProperty());
    }
}