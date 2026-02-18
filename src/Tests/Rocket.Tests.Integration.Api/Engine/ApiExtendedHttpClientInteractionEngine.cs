using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Achar.Domain.Testing;
using Achar.Domain.Testing.Exception;
using Achar.Infrastructure.Api.HttpClient;
using Achar.Infrastructure.Api.HttpClient.Options;
using Achar.Infrastructure.Testing.Extensions;
using Microsoft.Extensions.Options;

namespace Rocket.Tests.Integration.Api.Engine
{
    public class ApiExtendedHttpClientInteractionEngine(IOptions<ApiConfigurationOptions> apiConfigurationOptionsAccessor)
        : ApiHttpClientInteractionEngine(apiConfigurationOptionsAccessor), IApiExtendedInteractionEngine
    {
        private readonly ApiConfigurationOptions _apiConfigurationOptions = apiConfigurationOptionsAccessor.Value;
        private readonly IList<Tuple<string, string>> _requestHeaders = new List<Tuple<string, string>>();
        private HttpResponseMessage _lastResponse;
        private ApiRequest _lastRequest;

        public new Task SetRequestHeaderValueAsync(
            string headerKey,
            string headerValue)
        {
            _requestHeaders
                .Add(new Tuple<string, string>(headerKey, headerValue));

            return
                Task
                    .CompletedTask;
        }

        public async Task SendMultiPartRequestAsync(
            string method,
            string endpoint,
            byte[] bytes,
            string contentType,
            string fileName
        )
        {
            using var httpClient = new HttpClient();

            try
            {
                httpClient.BaseAddress = new Uri(_apiConfigurationOptions.BaseUrl);

                var httpRequestMessage =
                    new HttpRequestMessage(
                        method.ToHttpMethod(),
                        endpoint
                    );

                if (_requestHeaders.Any())
                {
                    httpRequestMessage
                        .Headers
                        .Clear();

                    foreach (var header in _requestHeaders)
                        httpRequestMessage
                            .Headers
                            .Add(header.Item1, header.Item2);
                }

                _lastResponse =
                    await
                        httpClient
                            .SendAsync(httpRequestMessage);

                using var form = new MultipartFormDataContent();

                var fileContent = new ByteArrayContent(bytes);

                fileContent.Headers.ContentType =
                    new MediaTypeHeaderValue(contentType);

                form
                    .Add(
                        fileContent,
                        "file",
                        fileName
                    );

                _lastResponse =
                    await
                        httpClient
                            .PostAsync(
                                endpoint,
                                form
                            );
            }
            catch (HttpRequestException ex)
            {
                throw new ApiRequestFailedException(_lastRequest, ex);
            }
        }
    }
}