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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rocket.Tests.Integration.Api.Engine
{
    public class ApiExtendedHttpClientInteractionEngine(IOptions<ApiConfigurationOptions> apiConfigurationOptionsAccessor)
        : ApiHttpClientInteractionEngine(apiConfigurationOptionsAccessor), IApiExtendedInteractionEngine
    {
        private readonly ApiConfigurationOptions _apiConfigurationOptions = apiConfigurationOptionsAccessor.Value;
        private readonly IList<Tuple<string, string>> _requestHeaders = new List<Tuple<string, string>>();
        private HttpResponseMessage _lastResponse;
        private ApiRequest _lastRequest;
        private string _imageBase64;
        
        public void SetImageBase64(string imageBase64) => _imageBase64 = imageBase64;

        public new Task CreateRequestAsync(string endpoint)
        {
            _requestHeaders.Clear();

            _lastRequest = new ApiRequest
            {
                Endpoint = endpoint,
                Data = () => null
            };

            return
                Task
                    .CompletedTask;
        }

        public new Task SetRequestHeaderValueAsync(
            string headerKey,
            string headerValue
        )
        {
            _requestHeaders
                .Add(
                    new Tuple<string, string>(
                        headerKey,
                        headerValue
                    )
                );

            return
                Task
                    .CompletedTask;
        }

        public async Task SendMultiPartRequestAsync(
            string method,
            string contentType,
            string fileName
        )
        {
            using var httpClient = new HttpClient();
            
            var bytes = 
                Convert
                    .FromBase64String(_imageBase64 ?? string.Empty);

            try
            {
                httpClient.BaseAddress = new Uri(_apiConfigurationOptions.BaseUrl);

                var httpRequestMessage =
                    new HttpRequestMessage(
                        method.ToHttpMethod(),
                        _lastRequest.Endpoint
                    );

                if (_requestHeaders.Any())
                {
                    httpRequestMessage
                        .Headers
                        .Clear();

                    foreach (var header in _requestHeaders)
                        httpRequestMessage
                            .Headers
                            .Add(
                                header.Item1,
                                header.Item2
                            );
                }

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

                httpRequestMessage.Content = form;

                _lastResponse =
                    await
                        httpClient
                            .SendAsync(
                                httpRequestMessage
                            );
            }
            catch (HttpRequestException ex)
            {
                throw new ApiRequestFailedException(
                    _lastRequest,
                    ex
                );
            }
        }
        
        public new Task AssertResponseFailedAsync(int? expectedStatusCode = null)
        {
            if (_lastResponse == null)
                throw new HttpRequestException($"No response was received from endpoint {_apiConfigurationOptions.BaseUrl}");

            var actualStatusCode = (int)_lastResponse.StatusCode;

            if (expectedStatusCode.HasValue)
            {
                if (expectedStatusCode.Value != actualStatusCode)
                    throw new ApiResponseStatusCodeUnexpectedException(expectedStatusCode.Value, actualStatusCode, _lastRequest);
            }
            else
            {
                // throw if success
                try
                {
                    _lastResponse
                        .EnsureSuccessStatusCode();

                    throw new ApiResponseStatusCodeUnexpectedException(null, actualStatusCode, _lastRequest);
                }
                catch (HttpRequestException)
                {
                }
            }

            return
                Task
                    .CompletedTask;
        }
        
        public new async Task AssertJsonTokenPathValueEqualsAsync(
            string jsonTokenPath,
            string expectedValue)
        {
            var jsonBody =
                await
                    _lastResponse?
                        .Content?
                        .ReadAsStringAsync();

            var jObject =
                JObject
                    .Parse(jsonBody);

            var expectedObject =
                jObject
                    .SelectToken(jsonTokenPath);

            string actualValue;

            if (expectedObject?.Type != JTokenType.Object)
            {
                actualValue =
                    expectedObject?
                        .Value<string>();
            }
            else
            {
                var dict =
                    expectedObject
                        .Value<JObject>();

                actualValue =
                    dict
                        .ToString(Formatting.None);
            }

            if (!string.Equals(actualValue, expectedValue, StringComparison.InvariantCultureIgnoreCase))
                throw new ApiResponseValueUnexpectedException(jsonTokenPath, expectedValue, actualValue, _lastRequest);
        }
        
        public new Task AssertResponseSucceededAsync()
        {
            try
            {
                if (_lastResponse == null)
                    throw new HttpRequestException($"No response was received from endpoint {_apiConfigurationOptions.BaseUrl}");

                _lastResponse
                    .EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new ApiRequestFailedException(_lastRequest, ex);
            }

            return
                Task
                    .CompletedTask;
        }
    }
}