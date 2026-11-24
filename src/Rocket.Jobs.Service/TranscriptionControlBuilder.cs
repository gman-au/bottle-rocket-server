using System;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts.Transcriptions;
using Rocket.Domain.Executions;
using Rocket.Integrations.Common;
using Rocket.Interfaces;

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

            var jobControl =
                new TranscriptionControl
                {
                    AddTextTranscriptionCallback = addTextTranscriptionTask
                };

            return jobControl;
        }
    }
}