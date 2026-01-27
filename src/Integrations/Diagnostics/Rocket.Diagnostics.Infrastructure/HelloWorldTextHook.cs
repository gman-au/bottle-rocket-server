using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Diagnostics.Domain;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldTextHook : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is HelloWorldTextExecutionStep;

        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            Func<string, string, Task> appendLogMessageCallback,
            CancellationToken cancellationToken
        )
        {
            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                    Artifact = Encoding.Default.GetBytes("Hello, world!"),
                    FileExtension = ".txt"
                };
        }
    }
}