using MudBlazor;
using Rocket.Domain.Enum;

namespace Rocket.Web.Host.Extensions
{
    internal static class IntEx
    {
        public static Color MapExecutionStatusToColor(this int? executionStatus)
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
        
        public static Color MapBatchStatusToColor(this int? batchStatus)
        {
            return batchStatus switch
            {
                (int)BatchStatusEnum.Open => Color.Success,
                (int)BatchStatusEnum.Closed => Color.Info,
                (int)BatchStatusEnum.Archived => Color.Warning,
                _ => Color.Default
            };
        }

        public static bool MapExecutionStatusToFinished(this int? executionStatus)
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