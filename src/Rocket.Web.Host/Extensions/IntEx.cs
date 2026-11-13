using MudBlazor;
using Rocket.Domain.Enum;

namespace Rocket.Web.Host.Extensions
{
    internal static class IntEx
    {
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

        public static bool MapStatusToFinished(this int? executionStatus)
        {
            return executionStatus switch
            {
                (int)ExecutionStatusEnum.Cancelled => true,
                (int)ExecutionStatusEnum.Completed => true,
                (int)ExecutionStatusEnum.Errored => true,
                _ => false
            };
        }
    }
}