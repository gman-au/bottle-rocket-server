using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition
{
    public class SearchResult
    {
        [JsonPropertyName("object")] 
        public string Object { get; set; }
        
        [JsonPropertyName("id")] 
        public string Id { get; set; }
        
        [JsonPropertyName("properties")] 
        public SearchResultProperties Properties { get; set; }
    }
}