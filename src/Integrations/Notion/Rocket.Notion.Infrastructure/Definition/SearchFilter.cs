using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition
{
    public class SearchFilter
    {
        [JsonPropertyName("property")] 
        public string Property { get; set; }
        
        [JsonPropertyName("value")] 
        public string Value { get; set; }
    }
}