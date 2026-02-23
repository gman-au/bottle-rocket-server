using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Replicate.Domain;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure
{
    public class ReplicateClient(ILogger<ReplicateClient> logger) : BaseReplicateClient, IReplicateClient
    {
        private const int PollingIntervalInSeconds = 5;

        private static readonly JsonSerializerOptions DefaultJsonSerializationOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public async Task<(string, string)> UploadFileAsync(
            string apiToken,
            byte[] bytes,
            string fileName,
            string fileExtension,
            CancellationToken cancellationToken
        )
        {
            using var httpClient = GetBaseHttpClient(apiToken);

            var multipart =
                BuildMultipartContent(
                    bytes,
                    fileName,
                    $"image/{fileExtension.Replace(".", "")}"
                );

            HttpResponseMessage response = null;
            try
            {
                response =
                    await
                        httpClient
                            .PostAsync(
                                "v1/files",
                                multipart,
                                cancellationToken
                            );

                response
                    .EnsureSuccessStatusCode();

                var uploadResponse =
                    await
                        response
                            .Content
                            .ReadFromJsonAsync<ReplicateUploadResponse>(cancellationToken);

                var getUrl =
                    uploadResponse?
                        .Urls?
                        .GetUrl;

                var imageId =
                    uploadResponse?
                        .Id;

                if (string.IsNullOrEmpty(getUrl) || string.IsNullOrEmpty(imageId))
                    throw new RocketException(
                        "There was an error uploading the image to Replicate.",
                        ApiStatusCodeEnum.ThirdPartyServiceError
                    );

                return (getUrl, imageId);
            }
            catch (HttpRequestException)
            {
                throw
                    await
                        HandleReplicateExceptionAsync(
                            response,
                            cancellationToken
                        );
            }
        }

        public async Task<string> CreatePredictionAsync<T>(
            string apiToken,
            string version,
            T input,
            CancellationToken cancellationToken,
            string customEndpoint = null
        )
            where T : IReplicateInput
        {
            customEndpoint ??= "v1/predictions";

            using var httpClient = GetBaseHttpClient(apiToken);

            var request = new CreatePredictionRequest<T>
            {
                Version = version,
                Input = input
            };

            HttpResponseMessage response = null;
            try
            {
                response =
                    await
                        httpClient
                            .PostAsJsonAsync(
                                customEndpoint,
                                request,
                                DefaultJsonSerializationOptions,
                                cancellationToken
                            );

                response
                    .EnsureSuccessStatusCode();

                var uploadResponse =
                    await
                        response
                            .Content
                            .ReadFromJsonAsync<CreatePredictionResponse<T>>(cancellationToken);

                var predictionId = uploadResponse?.Id;

                if (string.IsNullOrEmpty(predictionId))
                    throw new RocketException(
                        "There was an error creating the prediction in Replicate.",
                        ApiStatusCodeEnum.ThirdPartyServiceError
                    );

                return predictionId;
            }
            catch (HttpRequestException)
            {
                throw
                    await
                        HandleReplicateExceptionAsync(
                            response,
                            cancellationToken
                        );
            }
        }

        public async Task<string> WaitUntilPredictionCompletesAsync(
            string apiToken,
            string predictionId,
            CancellationToken cancellationToken,
            int timeoutInSeconds = 300
        ) =>
            await
                GetPredictionStatusAsync<string>(
                    apiToken,
                    predictionId,
                    cancellationToken,
                    timeoutInSeconds
                );

        public async Task<T> WaitUntilPredictionCompletesAsync<T>(
            string apiToken,
            string predictionId,
            CancellationToken cancellationToken,
            int timeoutInSeconds = 300
        ) where T : IReplicateOutput =>
            await
                GetPredictionStatusAsync<T>(
                    apiToken,
                    predictionId,
                    cancellationToken,
                    timeoutInSeconds
                );

        private async Task<T> GetPredictionStatusAsync<T>(
            string apiToken,
            string predictionId,
            CancellationToken cancellationToken,
            int timeoutInSeconds = 300
        )
        {
            // loop (this is a background job so patience is OK)
            var status =
                ReplicateDomainConstants
                    .ReplicateStatusProcessing;

            ReplicatePredictionResponse<T> predictionStatusResponse = null;

            var stopwatch = new Stopwatch();

            stopwatch
                .Start();

            IEnumerable<string> transitiveStatuses =
                new[]
                    {
                        ReplicateDomainConstants.ReplicateStatusStarting,
                        ReplicateDomainConstants.ReplicateStatusProcessing
                    }
                    .ToArray();

            while (transitiveStatuses.Contains(status))
            {
                if (stopwatch.Elapsed.TotalSeconds > timeoutInSeconds)
                    throw new RocketException(
                        $"The Replicate prediction timed out ({timeoutInSeconds} seconds).",
                        ApiStatusCodeEnum.ThirdPartyServiceError
                    );

                predictionStatusResponse =
                    await
                        GetReplicatePredictionStatusAsync<T>(
                            apiToken,
                            predictionId,
                            cancellationToken
                        );

                status =
                    predictionStatusResponse
                        .Status;

                await
                    Task
                        .Delay(
                            5000,
                            cancellationToken
                        );

                logger
                    .LogInformation(
                        "Polling interval {pollingInterval} sec elapsed, retrying prediction status.",
                        PollingIntervalInSeconds
                    );
            }

            stopwatch
                .Stop();

            if (status != ReplicateDomainConstants.ReplicateStatusSucceeded)
            {
                throw new RocketException(
                    "There was an error processing the Replicate prediction.",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

            if (predictionStatusResponse == null || predictionStatusResponse != null && predictionStatusResponse.Output == null)
            {
                throw new RocketException(
                    "There was an error processing the Replicate prediction.",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

            return
                predictionStatusResponse
                    .Output;
        }

        private async Task<ReplicatePredictionResponse<T>> GetReplicatePredictionStatusAsync<T>(
            string apiToken,
            string predictionId,
            CancellationToken cancellationToken
        )
        {
            using var httpClient = GetBaseHttpClient(apiToken);

            HttpResponseMessage response = null;
            try
            {
                response =
                    await
                        httpClient
                            .GetAsync(
                                $"v1/predictions/{predictionId}",
                                cancellationToken
                            );

                response
                    .EnsureSuccessStatusCode();

                var predictionStatusResponse =
                    await
                        response
                            .Content
                            .ReadFromJsonAsync<ReplicatePredictionResponse<T>>(cancellationToken);

                return predictionStatusResponse;
            }
            catch (HttpRequestException)
            {
                throw
                    await
                        HandleReplicateExceptionAsync(
                            response,
                            cancellationToken
                        );
            }
        }

        public async Task DeleteUploadAsync(
            string apiToken,
            string fileId
        )
        {
            using var httpClient = GetBaseHttpClient(apiToken);

            try
            {
                await
                    httpClient
                        .DeleteAsync(
                            $"v1/files/{fileId}"
                        );
            }
            catch (HttpRequestException ex)
            {
                logger
                    .LogError(
                        "Could not delete file from Replicate: {error}",
                        ex.Message
                    );
            }
        }

        private static MultipartFormDataContent BuildMultipartContent(byte[] bytes, string fileName, string mimeType)
        {
            // NOTE: .NET quotes the boundary by default which Replicate rejects.
            // Parts must be added before modifying Content-Type or they will be lost.
            var boundary =
                Guid
                    .NewGuid()
                    .ToString("N");

            var fileContent = new ByteArrayContent(bytes);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

            fileContent
                .Headers
                .TryAddWithoutValidation(
                    "Content-Disposition",
                    $"form-data; name=\"content\"; filename=\"{fileName}\""
                );

            var filenameContent = new StringContent(fileName);

            filenameContent
                .Headers
                .TryAddWithoutValidation(
                    "Content-Disposition",
                    "form-data; name=\"filename\""
                );

            var multipart = new MultipartFormDataContent(boundary);

            multipart
                .Add(fileContent);
            multipart
                .Add(filenameContent);

            // Must come AFTER Add() calls or content is wiped
            multipart
                .Headers
                .Remove("Content-Type");

            multipart
                .Headers
                .TryAddWithoutValidation(
                    "Content-Type",
                    $"multipart/form-data; boundary={boundary}"
                );

            return multipart;
        }
    }
}