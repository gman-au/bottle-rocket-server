using System.Text.Json.Serialization;

namespace Rocket.Notion.Contracts
{
    public class NotionDataSourceProperty
    {
        [JsonPropertyName("id")]
        public string DataSourceId { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}