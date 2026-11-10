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

        public static async Task FailStepAsync(
            this IJobControl jobControl,
            Exception exception,
            string scanId = null
        )
        {
            await
                jobControl
                    .UpdateStepStatusCallback(
                        new JobStatusEntry
                        {
                            ExecutionStatus = (int)ExecutionStatusEnum.Errored,
                            ScanId = scanId,
                            Exception = exception
                        }
                    );
        }
    }
}