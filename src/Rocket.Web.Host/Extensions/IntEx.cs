using MudBlazor;
using Rocket.Domain.Enum;

namespace Rocket.Web.Host.Extensions
{
    internal static class IntEx
    {
        private const string BaseImagePath = "/img/icons";
        
        public static string GetRocketbookPageSymbolImagePath(this int? pageSymbol)
        {
            switch (pageSymbol)
            {
                case (int)PageSymbolEnum.Rocket:
                    return $"{BaseImagePath}/rocket.png";
                case (int)PageSymbolEnum.Diamond:
                    return $"{BaseImagePath}/diamond.png";
                case (int)PageSymbolEnum.Apple:
                    return $"{BaseImagePath}/apple.png";
                case (int)PageSymbolEnum.Bell:
                    return $"{BaseImagePath}/bell.png";
                case (int)PageSymbolEnum.Clover:
                    return $"{BaseImagePath}/clover.png";
                case (int)PageSymbolEnum.Star:
                    return $"{BaseImagePath}/star.png";
                case (int)PageSymbolEnum.Horseshoe:
                    return $"{BaseImagePath}/horseshoe.png";
                default:
                    return null;
            }
        }

        public static Color MapStatusToColor(this int? executionStatus)
        {
            return executionStatus switch
            {
                (int)ExecutionStatusEnum.Cancelled => Color.Warning,
                (int)ExecutionStatusEnum.Completed => Color.Success,
                (int)ExecutionStatusEnum.Errored => Color.Error,
                (int)ExecutionStatusEnum.Running => Color.Info,
                _ => Color.Default
            };
        }
    }
}