using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaHook : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is OllamaExtractExecutionStep;

        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            CancellationToken cancellationToken
        )
        {
            var artifact =
                context
                    .GetInputArtifact();

            var connector =
                await
                    context
                        .GetConnectorAsync<OllamaConnector>(
                            userId,
                            step,
                            cancellationToken
                        );

            var imageBytes = artifact.Artifact;
            var fileName = $"{Guid.NewGuid()}{artifact.FileExtension}";

            using var httpClient = new HttpClient();

            httpClient.BaseAddress =
                new Uri(
                    connector.Endpoint ??
                    throw new ConfigurationErrorsException(
                        nameof(connector.Endpoint)
                    )
                );

            var response =
                await
                    httpClient
                        .PostAsync(
                            "model/predict",
                            new MultipartFormDataContent
                            {
                                {
                                    new ByteArrayContent(imageBytes),
                                    "image",
                                    fileName
                                }
                            },
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();

            var ocrResponse =
                await
                    response
                        .Content
                        .ReadFromJsonAsync<OllamaResponse>(cancellationToken);

            var resultArtifact =
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                    Artifact =
                        Encoding
                            .Default
                            .GetBytes(
                                "Meet the flintstones!"
                                /*
                        string.Join(
                                    "\n",
                                    ocrResponse?.Text ?? []
                                )
                                */
                            ),
                    FileExtension = ".txt"
                };

            return resultArtifact;
        }
    }
}