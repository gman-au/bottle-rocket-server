using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Diagnostics.Domain;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Interfaces;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldProjectHook(ILogger<HelloWorldProjectHook> logger) : HookBase<HelloWorldProjectExecutionStep>(logger), IIntegrationHook
    {
        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            Func<string, string, Task> appendLogMessageCallback,
            CancellationToken cancellationToken
        )
        {
            var projectTaskData =
                new ProjectTaskTrackerSchema
                {
                    Notes = "Hello world notes!",
                    Items =
                    [
                        new ProjectTaskTrackerItem
                        {
                            Project = "PROJECT #1",
                            Task = "Say hello to the world!",
                            DueDate = DateTime.Now.AddDays(10).ToString("d"),
                            EstimatedTime = 30.ToString()
                        },
                        new ProjectTaskTrackerItem
                        {
                            Project = "PROJECT #2",
                            Task = "Buy an ice cream",
                            DueDate = DateTime.Now.AddDays(15).ToString("d"),
                            EstimatedTime = 10.ToString()
                        },
                        new ProjectTaskTrackerItem
                        {
                            Project = "PROJECT #3",
                            Task = "Run home",
                            DueDate = DateTime.Now.AddDays(20).ToString("d"),
                            EstimatedTime = 1400.ToString()
                        }
                    ]
                };

            var responseJson =
                JsonSerializer
                    .Serialize(
                        projectTaskData,
                        new JsonSerializerOptions { WriteIndented = true }
                    );

            await
                appendLogMessageCallback(
                    step.Id,
                    responseJson
                );

            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData,
                    Artifact = Encoding.Default.GetBytes(responseJson),
                    FileExtension = ".json"
                };
        }
    }
}