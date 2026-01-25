using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;
using Rocket.Ollama.Infrastructure.Definition;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaHook(
        ILogger<OllamaHook> logger,
        IImageBase64Converter imageBase64Converter
    ) : IIntegrationHook
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

            var base64Image = 
                imageBase64Converter
                    .Perform(imageBytes);

            if (step is not OllamaExtractExecutionStep ollamaStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            using var httpClient = new HttpClient();

            httpClient.BaseAddress =
                new Uri(
                    connector.Endpoint ??
                    throw new ConfigurationErrorsException(
                        nameof(connector.Endpoint)
                    )
                );

            httpClient.Timeout =
                TimeSpan
                    .FromMinutes(10);

            var request = new OllamaOcrRequest
            {
                Model = ollamaStep.ModelName,
                Messages =
                [
                    new OllamaOcrRequestMessage
                    {
                        Role = "user",
                        Content = "Perform OCR on this image and return only the text.",
                        Images = [base64Image]
                    }
                ],
                Stream = false
            };

            var response =
                await
                    httpClient
                        .PostAsJsonAsync(
                            "api/chat",
                            request,
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();

            var ocrResponse =
                await
                    response
                        .Content
                        .ReadFromJsonAsync<OllamaOcrResponse>(cancellationToken);

            if (string.IsNullOrEmpty(ocrResponse?.Message?.Content))
                throw new RocketException(
                    "No OCR data was extracted from the image.",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );

            var resultArtifact =
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                    Artifact =
                        Encoding
                            .Default
                            .GetBytes(
                                ocrResponse.Message.Content
                            ),
                    FileExtension = ".txt"
                };

            return resultArtifact;
        }
    }
}