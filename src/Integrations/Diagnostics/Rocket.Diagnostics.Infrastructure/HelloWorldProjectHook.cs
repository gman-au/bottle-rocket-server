using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Diagnostics.Domain;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldProjectHook : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is HelloWorldProjectExecutionStep;

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
                            DueDate = DateTime.Now.AddDays(10),
                            EstimatedTime = 30
                        },
                        new ProjectTaskTrackerItem
                        {
                            Project = "PROJECT #2",
                            Task = "Buy an ice cream",
                            DueDate = DateTime.Now.AddDays(15),
                            EstimatedTime = 10
                        },
                        new ProjectTaskTrackerItem
                        {
                            Project = "PROJECT #3",
                            Task = "Run home",
                            DueDate = DateTime.Now.AddDays(20),
                            EstimatedTime = 1400
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