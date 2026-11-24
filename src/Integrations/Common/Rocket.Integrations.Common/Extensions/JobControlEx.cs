using System;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Integrations.Domain;
using Rocket.Interfaces;

namespace Rocket.Integrations.Common.Extensions
{
    public static class JobControlEx
    {
        public static async Task SucceedStepAsync(
            this IJobControl jobControl,
            string successMessage,
            string scanId = null
        )
        {
            await
                jobControl
                    .AppendLogMessageCallback(
                        new JobLogEntry
                        {
                            ScanId = scanId,
                            LogMessage = successMessage
                        }
                    );

            await
                jobControl
                    .UpdateStepStatusCallback(
                        new JobStatusEntry
                        {
                            ExecutionStatus = (int)ExecutionStatusEnum.Completed,
                            ScanId = scanId,
                            Exception = null
                        }
                    );
        }

        public static async Task StartStepAsync(
            this IJobControl jobControl,
            string scanId = null
        )
        {
            await
                jobControl
                    .UpdateStepStatusCallback(
                        new JobStatusEntry
                        {
                            ExecutionStatus = (int)ExecutionStatusEnum.Running,
                            ScanId = scanId,
                            Exception = null
                        }
                    );
        }

        public static async Task FailStepAsync(
            this IJobControl jobControl,
            Exception exception,
            string scanId = null
        )
        {
            var cancelled = exception is TaskCanceledException;

            var status =
                cancelled ? (int)ExecutionStatusEnum.Cancelled : (int)ExecutionStatusEnum.Errored;

            await
                jobControl
                    .UpdateStepStatusCallback(
                        new JobStatusEntry
                        {
                            ExecutionStatus = status,
                            ScanId = scanId,
                            Exception = exception
                        }
                    );
            
            if (cancelled)
                throw exception;
        }
    }
}