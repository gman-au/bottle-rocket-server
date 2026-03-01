using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.DataSource
{
    public class DataSourceParent
    {
        [JsonPropertyName("data_source_id")] 
        public string DataSourceId { get; set; }
    }
}