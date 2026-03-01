using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Notion.Contracts
{
    public class NotionDataSourceSummary
    {
        [JsonPropertyName("data_source_id")]
        public string DataSourceId { get; set; }
        
        [JsonPropertyName("data_source_name")]
        public string DataSourceName { get; set; }
        
        [JsonPropertyName("fields")]
        public IEnumerable<NotionDataSourceProperty> Fields { get; set; }
    }
}