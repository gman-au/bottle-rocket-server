using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using Grpc.Core;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Jobs;
using Rocket.Gcp.Domain;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;

namespace Rocket.Gcp.Infrastructure
{
    public class VisionOcrService(
        IFileRetitler fileRetitler
    ) : IVisionOcrService
    {
        public async Task<ExecutionStepArtifact> ExtractHandwrittenTextAsync(
            IJobControl jobControl,
            ExecutionStepArtifact artifact,
            GcpCredential gcpCredential,
            int timeoutInMinutes,
            CancellationToken cancellationToken
        )
        {
            if (artifact == null)
                throw new Exception("Artifact is empty or undefined; please check configuration.");

            var elements = artifact.Elements ?? [];

            var artifactElements = new List<ArtifactDataElement>();

            // Convert your credential object back to JSON string
            var credentialJson =
                JsonSerializer
                    .Serialize(gcpCredential);

            // Create GoogleCredential from JSON content (new way)
            GoogleCredential credential;
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(credentialJson)))
            {
                credential =
                    await
                        GoogleCredential
                            .FromStreamAsync(
                                stream,
                                cancellationToken
                            );
            }

            // Create client with specific credentials
            var clientBuilder =
                new ImageAnnotatorClientBuilder
                {
                    Credential = credential
                };

            var client =
                await
                    clientBuilder
                        .BuildAsync(cancellationToken);

            var allCompleted = true;

            foreach (var element in elements)
            {
                try
                {
                    await
                        jobControl
                            .StartStepAsync(
                                element.ScanId
                            );

                    try
                    {
                        var response =
                            await
                                client
                                    .DetectDocumentTextAsync(
                                        Image.FromBytes(element.Artifact),
                                        null,
                                        CallSettings
                                            .FromExpiration(
                                                Expiration
                                                    .FromTimeout(
                                                        TimeSpan
                                                            .FromMinutes(timeoutInMinutes)
                                                    )
                                            )
                                    );

                        var ocrResult =
                            response
                                .Text;

                        await
                            jobControl
                                .SucceedStepAsync(
                                    $"GCP Vision response: \"{ocrResult}\"",
                                    element.ScanId
                                );

                        var resultArtifact =
                            new ArtifactDataElement
                            {
                                Artifact =
                                    Encoding
                                        .Default
                                        .GetBytes(
                                            ocrResult
                                        ),
                                FileExtension = CommonFileConstants.TextFileExtension,
                                ScanId = element.ScanId,
                                FileName =
                                    await
                                        fileRetitler
                                            .RetitleAsync(
                                                ocrResult,
                                                element.ScanId,
                                                artifact.UserId,
                                                cancellationToken
                                            ) ?? artifact.FileName
                            };

                        artifactElements
                            .Add(resultArtifact);
                    }
                    catch (RpcException ex)
                    {
                        if (ex.StatusCode == StatusCode.DeadlineExceeded)
                        {
                            throw new OperationCanceledException();
                        }

                        throw;
                    }
                }
                catch (Exception ex)
                {
                    allCompleted = false;

                    await
                        jobControl
                            .FailStepAsync(
                                ex,
                                element.ScanId
                            );
                }
            }

            if (!allCompleted)
            {
                throw new RocketException(
                    "There was an error extracting the text via GCP Vision",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

            // Retitle
            var uniqueElements =
                artifactElements
                    .ToArray()
                    .EnumerateUniquely();

            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                    BatchId = artifact.BatchId,
                    Elements = uniqueElements,
                    FileName = artifact.FileName,
                    UserId = artifact.UserId
                };
        }
    }
}