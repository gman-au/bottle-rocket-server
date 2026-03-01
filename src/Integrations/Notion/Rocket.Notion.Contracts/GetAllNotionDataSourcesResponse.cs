using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts;

namespace Rocket.Notion.Contracts
{
    public class GetAllNotionDataSourcesResponse : ApiResponse
    {
        [JsonPropertyName("data_sources")]
        public IEnumerable<NotionDataSourceSummary> DataSources { get; set; }
    }
}