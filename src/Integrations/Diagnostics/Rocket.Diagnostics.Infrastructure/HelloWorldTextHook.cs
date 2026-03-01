using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Diagnostics.Domain;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldTextHook(ILogger<HelloWorldTextHook> logger) : HookBase<HelloWorldTextExecutionStep>(logger), IIntegrationHook
    {
        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            Func<string, string, Task> appendLogMessageCallback,
            CancellationToken cancellationToken
        )
        {
            return
                "Hello, world!"
                    .AsCompletedRawTextArtifact();
        }
    }
}