using System.Text.Json.Serialization;
using Rocket.Notion.Infrastructure.Definition.Common;

namespace Rocket.Notion.Infrastructure.Definition.Searching
{
    public class SearchResult
    {
        [JsonPropertyName("object")] 
        public string Object { get; set; }
        
        [JsonPropertyName("id")] 
        public string Id { get; set; }
        
        [JsonPropertyName("properties")] 
        public NotionProperties Properties { get; set; }
    }
}