using System;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts.Transcriptions;
using Rocket.Domain.Executions;
using Rocket.Integrations.Common;
using Rocket.Interfaces;
using Rocket.Page.Schemas.ProjectTaskTracker;
using Rocket.Page.Schemas.WeeklyPlan;

namespace Rocket.Jobs.Service
{
    public class TranscriptionControlBuilder(
        ITranscriber transcriber
    ) : ITranscriptionControlBuilder
    {
        public ITranscriptionControl Build(
            string batchId,
            string userId,
            string executionId,
            BaseExecutionStep executionStep
        )
        {
            var addTextTranscriptionTask =
                new Func<string, string, Task>(async (scanId, text) =>
                    {
                        var textTranscription =
                            new TextTranscriptionSummary
                            {
                                ExecutionId = executionId,
                                ExecutionStepName = executionStep.StepName,
                                TranscriptionDate = executionStep.RunDate ?? DateTime.UtcNow,
                                Text = text
                            };

                        await
                            transcriber
                                .ApplyTranscriptionAsync(
                                    userId,
                                    batchId,
                                    scanId,
                                    textTranscription,
                                    CancellationToken.None
                                );
                    }
                );            
            
            var addProjectTranscriptionTask =
                new Func<string, ProjectTaskTrackerSchema, Task>(async (scanId, project) =>
                    {
                        var projectTranscription =
                            new ProjectTranscriptionSummary
                            {
                                ExecutionId = executionId,
                                ExecutionStepName = executionStep.StepName,
                                TranscriptionDate = executionStep.RunDate ?? DateTime.UtcNow,
                                Project = project
                            };

                        await
                            transcriber
                                .ApplyTranscriptionAsync(
                                    userId,
                                    batchId,
                                    scanId,
                                    projectTranscription,
                                    CancellationToken.None
                                );
                    }
                );     
            
            var addWeeklyPlanTranscriptionTask =
                new Func<string, WeeklyPlanSchema, Task>(async (scanId, weeklyPlan) =>
                    {
                        var weeklyPlanTranscription =
                            new WeeklyPlanTranscriptionSummary
                            {
                                ExecutionId = executionId,
                                ExecutionStepName = executionStep.StepName,
                                TranscriptionDate = executionStep.RunDate ?? DateTime.UtcNow,
                                WeeklyPlan = weeklyPlan
                            };

                        await
                            transcriber
                                .ApplyTranscriptionAsync(
                                    userId,
                                    batchId,
                                    scanId,
                                    weeklyPlanTranscription,
                                    CancellationToken.None
                                );
                    }
                );

            var jobControl =
                new TranscriptionControl
                {
                    AddTextTranscriptionCallback = addTextTranscriptionTask,
                    AddProjectTranscriptionCallback = addProjectTranscriptionTask,
                    AddWeeklyPlanTranscriptionCallback = addWeeklyPlanTranscriptionTask
                };

            return jobControl;
        }
    }
}