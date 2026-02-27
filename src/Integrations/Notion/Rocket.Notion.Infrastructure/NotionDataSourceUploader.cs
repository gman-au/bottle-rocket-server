using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Notion.Infrastructure.Definition.Common;
using Rocket.Notion.Infrastructure.Definition.DataSource;

namespace Rocket.Notion.Infrastructure
{
    public class NotionDataSourceUploader(ILogger<NotionDataSourceUploader> logger) : BaseNotionClient, INotionDataSourceUploader
    {
        public async Task UploadDataSourceAsync(
            string integrationSecret,
            string dataSourceId,
            IEnumerable<Tuple<string, string>> dataSourcePairs,
            CancellationToken cancellationToken
        )
        {
            using var httpClient = GetBaseHttpClient(integrationSecret);

            if ((dataSourcePairs ?? []).Any(o => string.IsNullOrEmpty(o.Item1)))
                throw new RocketException(
                    "One or more data source column names supplied in the Notion upload were empty. Please check the workflow configuration.",
                    ApiStatusCodeEnum.ServerConfigurationError
                );
            
            var request = new DataSourceRequest
            {
                Parent = new DataSourceParent
                {
                    DataSourceId = dataSourceId
                },
                Properties =
                    dataSourcePairs
                        .ToDictionary(
                            o => o.Item1,
                            o => new NotionRichTextProperty
                            {
                                RichText =
                                [
                                    new NotionProperty
                                    {
                                        Text = new NotionTextProperty
                                        {
                                            Content = o.Item2
                                        }
                                    }
                                ]
                            }
                        )
            };

            logger
                .LogDebug(
                    "Sending POST Json: {json}",
                    JsonSerializer.Serialize(request)
                );

            HttpResponseMessage response = null;
            try
            {
                response =
                    await
                        httpClient
                            .PostAsJsonAsync(
                                "v1/pages",
                                request,
                                cancellationToken
                            );

                response
                    .EnsureSuccessStatusCode();
            }
            catch (HttpRequestException)
            {
                await
                    HandleNotionExceptionAsync(
                        response,
                        cancellationToken
                    );
            }
        }
    }
}