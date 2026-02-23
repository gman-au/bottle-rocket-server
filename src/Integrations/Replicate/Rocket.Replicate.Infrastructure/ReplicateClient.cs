using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure
{
    public class ReplicateClient : BaseReplicateClient, IReplicateClient
    {
        public async Task<string> UploadFileAsync(
            string apiToken,
            byte[] bytes,
            string fileName,
            CancellationToken cancellationToken
        )
        {
            using var httpClient = GetBaseHttpClient(apiToken);

            var response =
                await
                    httpClient
                        .PostAsync(
                            "v1/files",
                            new MultipartFormDataContent
                            {
                                {
                                    new ByteArrayContent(bytes),
                                    "content",
                                    fileName
                                }
                            },
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();

            var uploadResponse =
                await
                    response
                        .Content
                        .ReadFromJsonAsync<ReplicateUploadResponse>(cancellationToken);

            var getUrl = uploadResponse?.Urls?.GetUrl;

            if (string.IsNullOrEmpty(getUrl))
                throw new RocketException(
                    "There was an error uploading the image to Replicate.",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );

            return getUrl;
        }

        public async Task<string> CreatePredictionAsync<T>(
            string apiToken,
            string version,
            T input,
            CancellationToken cancellationToken
        )
            where T : IReplicateInput
        {
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
                                "v1/predictions",
                                request,
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

        public async Task<ReplicatePredictionResponse<T>> GetPredictionStatusAsync<T>(
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
    }
}