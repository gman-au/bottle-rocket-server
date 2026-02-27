using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Searching
{
    public class DataSourceProperty
    {
        [JsonPropertyName("id")] 
        public string Id { get; set; }
        
        [JsonPropertyName("name")] 
        public string Name { get; set; }
        
        [JsonPropertyName("description")] 
        public string Description { get; set; }
        
        [JsonPropertyName("type")] 
        public string Type { get; set; }
    }
}