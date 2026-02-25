using Rocket.Domain.Enum;

namespace Rocket.Api.Host.Extensions
{
    public static class RocketbookProductTypeEnumEx
    {
        public static string ToCode(this RocketbookProductTypeEnum value)
        {
            var result = (ushort)value;

            return
                new string(
                    new[]
                    {
                        (char)(result >> 8),
                        (char)(result & 0xFF)
                    }
                );
        }
    }
}